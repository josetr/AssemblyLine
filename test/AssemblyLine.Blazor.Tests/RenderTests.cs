namespace AssemblyLine.Blazor.Tests;

using AssemblyLine.Blazor.Extensions;
using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor.Services;
using Xunit;

public class RenderTests : TestContext
{
    [Fact]
    public void RenderComponents()
    {
        var deviceService = new Mock<IDeviceService>();
        var device = new Device(1, "Printer", DeviceType.Printer);
        deviceService.Setup(x => x.GetDevicesAsync()).Returns(Task.FromResult(new[] { device }));
        deviceService.Setup(x => x.GetDeviceAsync(1)).Returns(Task.FromResult(device));

        var operationService = new Mock<IOperationService>();
        var operation = new Operation(1, "Operation", 1, device);
        operationService.Setup(x => x.GetOperationsAsync()).Returns(Task.FromResult(new[] { operation }));
        operationService.Setup(x => x.GetOperationAsync(1)).Returns(Task.FromResult(operation));
        operationService.Setup(x => x.GetNextOrderAsync()).Returns(Task.FromResult(1));

        Services.AddScoped(_ => operationService.Object);
        Services.AddScoped(_ => deviceService.Object);
        Services.AddScoped<IDialogService, DialogService>();
        Services.AddScoped<ISnackbar, Snackbar>();
        Services.AddViewModels();
        Services.AddOptions();
        Services.AddMudServices();
        JSInterop.Mode = JSRuntimeMode.Loose;
        RenderTree.TryAdd<CascadingValue<MudBlazor.MudDialogProvider>>(parameters => parameters.Add(p => p.Value, new MudBlazor.MudDialogProvider()));

        RenderComponent<Shared.MainLayout>();
        RenderComponent<Pages.Devices.Index>();
        RenderComponent<Pages.Devices.AddDeviceDialog>(parameters => parameters.AddCascadingValue(RenderComponent<MudBlazor.MudDialogInstance>().Instance));
        RenderComponent<Pages.Operations.Index>();
        RenderComponent<Pages.Operations.EditOperationDialog>(parameters => parameters.AddCascadingValue(RenderComponent<MudBlazor.MudDialogInstance>().Instance).Add(x => x.OperationId, 1));
        RenderComponent<Pages.Operations.AddOperationDialog>(parameters => parameters.AddCascadingValue(RenderComponent<MudBlazor.MudDialogInstance>().Instance));
    }
}
