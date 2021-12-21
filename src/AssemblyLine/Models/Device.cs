// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

public class Device
{
    public Device(string name, DeviceType type)
        : this(0, name, type)
    {
    }

    public Device(int id, string name, DeviceType type)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Type = type;

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentOutOfRangeException(nameof(name));
    }

    internal Device(int id)
    {
        Id = id;
    }

    private Device()
    {
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public DeviceType Type { get; private set; }

    public override string ToString() => Name;
}
