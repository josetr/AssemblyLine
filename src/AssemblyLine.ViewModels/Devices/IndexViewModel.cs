// Copyright (c) 2021 Jose Torres. All rights reserved. Licensed under the Apache License, Version 2.0. See LICENSE.md file in the project root for full license information.

namespace AssemblyLine.ViewModels.Devices;

public class IndexViewModel : ViewModelBase
{
    private readonly IOperationService _operationService;
    private readonly IDeviceService _deviceService;
    private readonly IDialogService _dialogService;

    private readonly ISnackbar _snackbar;

    public IndexViewModel(IOperationService operationService, IDeviceService deviceService, IDialogService dialogService, ISnackbar snackbar)
    {
        _operationService = operationService;
        _deviceService = deviceService;
        _dialogService = dialogService;
        _snackbar = snackbar;
    }

    public List<Device> Devices { get; private set; } = new();

    public override async Task OnInitializedAsync()
    {
        Devices = (await _deviceService.GetDevicesAsync()).ToList();
    }

    public async Task OpenCreateDeviceDialogAsync()
    {
        var device = await _dialogService.Show<AddDeviceDialogViewModel, Device>("Add Device");
        if (device == null)
            return;

        Devices.Add(device);
        _snackbar.Show($"Added device {device.Name}", Severity.Success);
    }

    public async Task DeleteDeviceAsync(Device device)
    {
        if (await _dialogService.ShowMessageBox("Delete Device", $"Do you really want to delete the device '{device.Name}'?", noText: "no", yesText: "yes") != true)
            return;

        var deviceUsageCount = await _operationService.GetDeviceUsageCountAsync(device.Id);
        if (deviceUsageCount > 0)
        {
            var message = $"{deviceUsageCount} operations that depend on the device `{device.Name}' will also be deleted. Do you want to continue?";
            if (await _dialogService.ShowMessageBox("Delete Operations", message, noText: "no", yesText: "yes") != true)
                return;
        }

        await _deviceService.DeleteDeviceAsync(device.Id);
        Devices.Remove(device);
        _snackbar.Show($"The device '{device.Name}' has been deleted.", Severity.Success);
    }
}
