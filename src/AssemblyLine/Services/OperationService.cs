// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

using Microsoft.EntityFrameworkCore;

public class OperationService : IOperationService
{
    private readonly IDbContextFactory<AssemblyLineDbContext> dbFactory;

    public OperationService(IDbContextFactory<AssemblyLineDbContext> dbFactory)
    {
        this.dbFactory = dbFactory;
    }

    public Task<Operation[]> GetOperationsAsync()
    {
        using var context = dbFactory.CreateDbContext();
        return context.Operations.Include(x => x.Device).OrderBy(x => x.Order).ToArrayAsync();
    }

    public async Task<int> GetNextOrderAsync()
    {
        using var context = dbFactory.CreateDbContext();
        var result = await context.Operations.OrderByDescending(x => x.Order).Select(x => x.Order).FirstOrDefaultAsync();
        return Math.Clamp(result + 1, Operation.MinOrder, Operation.MaxOrder);
    }

    public async Task<int> GetDeviceUsageCountAsync(int deviceId)
    {
        using var context = dbFactory.CreateDbContext();
        return await context.Operations.CountAsync(x => EF.Property<int>(x, nameof(x.Device) + "Id") == deviceId);
    }

    public async Task AddOperationAsync(Operation operation)
    {
        using var context = dbFactory.CreateDbContext();

        if (context.Operations.Any(x => x.Order == operation.Order))
            throw new FieldTakenException(nameof(operation.Order));

        if (operation.Device != null)
            context.Devices.Attach(operation.Device);
        await context.Operations.AddAsync(operation);
        await context.SaveChangesAsync();
    }

    public async Task<Operation?> GetOperationAsync(int id)
    {
        using var context = dbFactory.CreateDbContext();
        return await context.Operations.Include(x => x.Device).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task EditOperationAsync(Operation operation)
    {
        using var context = dbFactory.CreateDbContext();

        if (context.Operations.Any(x => x.Order == operation.Order && x.Id != operation.Id))
            throw new FieldTakenException(nameof(operation.Order));

        context.Update(operation);
        await context.SaveChangesAsync();
    }

    public async Task<bool> ChangeOrderAsync(Operation operation, int n)
    {
        using var context = dbFactory.CreateDbContext();

        var savedOperation = n > 0 ? context.Operations.OrderBy(x => x.Order).FirstOrDefault(x => x.Order > operation.Order)
            : n < 0 ? context.Operations.OrderByDescending(x => x.Order).FirstOrDefault(x => x.Order < operation.Order)
            : null;

        if (savedOperation == null)
            return false;

        var operationOrder = operation.Order;
        var newOperationOrder = savedOperation.Order;

        using var transaction = context.Database.BeginTransaction();

        context.Attach(operation);
        operation.SetTransientOrder();
        await context.SaveChangesAsync();

        savedOperation.ChangeOrder(operationOrder);
        operation.ChangeOrder(newOperationOrder);
        await context.SaveChangesAsync();

        transaction.Commit();
        return true;
    }

    public async Task DeleteOperationAsync(int id)
    {
        using var context = dbFactory.CreateDbContext();
        context.Remove(new Operation(id));
        await context.SaveChangesAsync();
    }
}
