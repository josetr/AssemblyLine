// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

public interface IDeviceService
{
    Task AddDeviceAsync(Device device);
    Task<int> CountDevicesAsync();
    Task<Device?> GetDeviceAsync(int id);
    Task DeleteDeviceAsync(int device);
    Task<Device[]> GetDevicesAsync();
}
