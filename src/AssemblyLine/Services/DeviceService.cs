// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

using Microsoft.EntityFrameworkCore;

public class DeviceService : IDeviceService
{
    private readonly IDbContextFactory<AssemblyLineDbContext> dbFactory;

    public DeviceService(IDbContextFactory<AssemblyLineDbContext> dbFactory)
    {
        this.dbFactory = dbFactory;
    }

    public Task<Device[]> GetDevicesAsync()
    {
        using var context = dbFactory.CreateDbContext();
        return context.Devices.ToArrayAsync();
    }

    public async Task AddDeviceAsync(Device device)
    {
        using var context = dbFactory.CreateDbContext();
        await context.Devices.AddAsync(device);
        await context.SaveChangesAsync();
    }

    public async Task<Device?> GetDeviceAsync(int id)
    {
        using var context = dbFactory.CreateDbContext();
        return await context.Devices.FindAsync(id);
    }

    public async Task DeleteDeviceAsync(int id)
    {
        using var context = dbFactory.CreateDbContext();
        context.Devices.Remove(new Device(id));
        await context.SaveChangesAsync();
    }

    public Task<int> CountDevicesAsync()
    {
        using var context = dbFactory.CreateDbContext();
        return context.Devices.CountAsync();
    }
}
