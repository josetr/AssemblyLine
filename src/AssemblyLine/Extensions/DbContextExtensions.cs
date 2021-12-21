// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

public static class DbContextExtensions
{
    public static bool Exists(this DbContext context)
    {
        var creator = context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
        return creator?.Exists() ?? throw new NotImplementedException();
    }
}
