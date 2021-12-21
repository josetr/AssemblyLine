// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

public class Operation
{
    public const int MinOrder = 1;
    public const int MaxOrder = 100_000;

    public Operation(string name, int order, Device device)
        : this(0, name, order, device)
    {
    }

    public Operation(int id, string name, int order, Device device)
    {
        CheckOrder(order);

        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Order = order;
        Device = device;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentOutOfRangeException(nameof(name));
    }

    internal Operation(int id)
    {
        Id = id;
    }

    private Operation()
    {
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Order { get; internal set; }
    public byte[]? ImageData { get; private set; }
    public Device Device { get; internal set; } = default!;

    public void ChangeOrder(int order)
    {
        CheckOrder(order);
        Order = order;
    }

    public override string ToString() => Name;

    internal void SetTransientOrder()
    {
        Order = -1;
    }

    private static void CheckOrder(int order)
    {
        if (!(order >= MinOrder && order <= MaxOrder))
            throw new ArgumentOutOfRangeException(nameof(order));
    }
}
