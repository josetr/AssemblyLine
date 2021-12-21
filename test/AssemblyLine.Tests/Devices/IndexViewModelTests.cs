namespace AssemblyLine.Tests.Devices;

using AssemblyLine.ViewModels.Devices;
using Moq;
using Xunit;

public class IndexViewModelTests : IDisposable
{
    private readonly TestDb db = new();

    public void Dispose()
    {
        db.Dispose();
    }

    [Fact]
    public async Task OnInitializedAsync()
    {
        var model = new IndexViewModel(new OperationService(db.Factory), new DeviceService(db.Factory), new Mock<IDialogService>().Object, new Mock<ISnackbar>().Object);
        await model.OnInitializedAsync();
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public async Task OpenCreateDeviceDialog(bool confirm, int expectedCount)
    {
        var dialogService = new Mock<IDialogService>();
        dialogService
            .Setup(x => x.Show<It.IsAnyType, Device>(It.IsAny<string>()))
            .Returns(Task.FromResult(confirm ? new Device("Printer", DeviceType.Printer) : null));
        var snackBar = new Mock<ISnackbar>();
        var model = new IndexViewModel(new OperationService(db.Factory), new DeviceService(db.Factory), dialogService.Object, snackBar.Object);

        await model.OpenCreateDeviceDialogAsync();

        snackBar.Verify(x => x.Show("Added device Printer", Severity.Success), Times.Exactly(expectedCount));
    }

    [Theory]
    [InlineData(true, 0)]
    [InlineData(false, 1)]
    public async Task DeleteDevice(bool confirm, int expectedCount)
    {
        var operationService = new OperationService(db.Factory);
        var deviceService = new DeviceService(db.Factory);
        var dialogService = new Mock<IDialogService>();
        var device = new Device("Printer", DeviceType.Printer);
        await deviceService.AddDeviceAsync(device);
        await operationService.AddOperationAsync(new Operation("Operation", 1, device));

        dialogService
         .Setup(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
         .Returns(Task.FromResult<bool?>(confirm));
        var model = new IndexViewModel(operationService, deviceService, dialogService.Object, new Mock<ISnackbar>().Object);
        await model.OnInitializedAsync();
        await model.DeleteDeviceAsync(device = model.Devices.First(x => x.Id == device.Id));

        Assert.Equal(expectedCount, model.Devices.Count);
        Assert.Equal(expectedCount, (await deviceService.GetDevicesAsync()).Length);
        Assert.Equal(expectedCount, (await operationService.GetOperationsAsync()).Length);
    }
}
