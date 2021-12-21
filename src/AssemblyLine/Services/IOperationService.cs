// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine;

public interface IOperationService
{
    Task<Operation[]> GetOperationsAsync();
    Task AddOperationAsync(Operation operation);
    Task DeleteOperationAsync(int id);
    Task EditOperationAsync(Operation operation);
    Task<bool> ChangeOrderAsync(Operation operation, int n);
    Task<int> GetDeviceUsageCountAsync(int deviceId);
    Task<Operation?> GetOperationAsync(int id);
    Task<int> GetNextOrderAsync();
}
