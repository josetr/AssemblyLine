// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OperationTypeConfiguration : IEntityTypeConfiguration<Operation>
{
    public virtual void Configure(EntityTypeBuilder<Operation> entityTypeBuilder)
    {
        entityTypeBuilder
            .HasIndex(x => new { x.Order })
            .IsUnique(true);
    }
}
