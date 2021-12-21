// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

using Microsoft.EntityFrameworkCore;

public sealed class AssemblyLineDbContext : DbContext
{
    public AssemblyLineDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Operation> Operations { get; set; } = default!;
    public DbSet<Device> Devices { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyLineDbContext).Assembly);
    }
}
