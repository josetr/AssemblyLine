// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine.ViewModels.Operations;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class AddOperationDialogViewModel : ViewModelBase
{
    private readonly IOperationService _operationService;
    private readonly IDeviceService _deviceService;

    public AddOperationDialogViewModel(IOperationService operationService, IDeviceService deviceService)
    {
        _operationService = operationService;
        _deviceService = deviceService;
    }

    public IAdditionalErrors? AdditionalErrors { get; set; }
    public OperationViewModel Operation { get; } = new();
    public Device[] Devices { get; private set; } = Array.Empty<Device>();

    public override async Task OnInitializedAsync()
    {
        Devices = await _deviceService.GetDevicesAsync();
        Operation.Order = await _operationService.GetNextOrderAsync();
    }

    public async Task<Operation?> SubmitAsync()
    {
        var operation = new Operation(Operation.Name, Operation.Order.Value, Operation.Device);

        try
        {
            await _operationService.AddOperationAsync(operation);
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
