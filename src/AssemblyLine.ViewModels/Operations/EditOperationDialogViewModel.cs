// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine.ViewModels.Operations;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class EditOperationDialogViewModel : ViewModelBase
{
    private readonly IOperationService _operationService;
    private readonly IDeviceService _deviceService;

    public EditOperationDialogViewModel(IOperationService operationService, IDeviceService deviceService)
    {
        _operationService = operationService;
        _deviceService = deviceService;
    }

    [Parameter] public int OperationId { get; set; } = new();
    public IAdditionalErrors? AdditionalErrors { get; set; }
    public OperationViewModel Operation { get; private set; } = new();
    public Device[] Devices { get; private set; } = Array.Empty<Device>();

    public override async Task OnInitializedAsync()
    {
        Devices = await _deviceService.GetDevicesAsync();
        var operation = await _operationService.GetOperationAsync(OperationId);
        if (operation == null)
            return;
        Operation.Name = operation.Name;
        Operation.Order = operation.Order;
        Operation.Device = operation.Device;
    }

    public async Task<Operation?> Submit()
    {
        var operation = new Operation(OperationId, Operation.Name, Operation.Order.Value, Operation.Device);

        try
        {
            await _operationService.EditOperationAsync(operation);
            return operation;
        }
        catch (Exception e)
        {
            AdditionalErrors?.DisplayError(e);
            return null;
        }
    }

    public class OperationViewModel
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required, NotNull]
        [Range(AssemblyLine.Operation.MinOrder, AssemblyLine.Operation.MaxOrder)]
        public int? Order { get; set; }

        [Required, NotNull]
        public Device? Device { get; set; }
    }
}
