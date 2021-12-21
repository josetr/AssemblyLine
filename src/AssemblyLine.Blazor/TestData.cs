// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

public static class TestData
{
    private static Device[] devices = new Device[]
    {
        new Device("BarcodeScanner3000", DeviceType.BarcodeScanner),
        new Device("Printer2000", DeviceType.Printer),
        new Device("4KCamera", DeviceType.Camera),
        new Device("Socket", DeviceType.SocketTray),
    };

    private static string[] operations = new[] { "Scan barcode", "Print logo", "Capture item state", "SocketTray" };

    public static void FillDatabaseWithTestData(AssemblyLineDbContext context)
    {
        context.Database.EnsureCreated();
        context.Devices.AddRange(devices);
        context.SaveChanges();
        for (var i = 0; i < operations.Length; ++i)
            context.Operations.Add(new Operation(operations[i], order: i + 1, devices[i]));
        context.SaveChanges();
    }
}
