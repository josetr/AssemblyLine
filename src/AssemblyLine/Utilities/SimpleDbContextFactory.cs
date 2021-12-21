// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

using Microsoft.EntityFrameworkCore;

public class SimpleDbContextFactory<T> : IDbContextFactory<T>
    where T : DbContext
{
    private readonly Func<T> factory;

    public SimpleDbContextFactory(Func<T> factory)
    {
        this.factory = factory;
    }

    public T CreateDbContext()
    {
        return factory();
    }
}
