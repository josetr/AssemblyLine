// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine.ViewModels.Devices;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

public class AddDeviceDialogViewModel : ViewModelBase
{
    private readonly IDeviceService _deviceService;

    public AddDeviceDialogViewModel(IDeviceService deviceService)
    {
        _deviceService = deviceService;
    }

    public IAdditionalErrors? AdditionalErrors { get; set; }
    public DeviceViewModel Device { get; } = new();
    public Device[] Devices { get; private set; } = Array.Empty<Device>();

    public override async Task OnInitializedAsync()
    {
        Devices = await _deviceService.GetDevicesAsync();
    }

    public async Task<Device?> SubmitAsync()
    {
        var device = new Device(Device.Name, Device.Type.Value);

        try
        {
            await _deviceService.AddDeviceAsync(device);
            return device;
        }
        catch (Exception e)
        {
            AdditionalErrors?.DisplayError(e);
            return null;
        }
    }

    public class DeviceViewModel
    {
        [Required]
        public string Name { get; set; } = default!;

        [Required, NotNull]
        public DeviceType? Type { get; set; }
    }
}
