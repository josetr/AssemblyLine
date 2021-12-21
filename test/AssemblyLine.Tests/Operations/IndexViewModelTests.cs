namespace AssemblyLine.Tests.Operations;

using AssemblyLine.ViewModels.Operations;
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
        var model = new IndexViewModel(new OperationService(db.Factory), new Mock<IDialogService>().Object, new Mock<ISnackbar>().Object);
        await model.OnInitializedAsync();
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public async Task OpenCreateOperationDialog(bool confirm, int expectedCount)
    {
        var dialogService = new Mock<IDialogService>();
        dialogService
            .Setup(x => x.Show<It.IsAnyType, Operation>(It.IsAny<string>()))
            .Returns(Task.FromResult(confirm ? new Operation("Print", order: 1, new Device("Printer", DeviceType.Printer)) : null));
        var snackBar = new Mock<ISnackbar>();
        var model = new IndexViewModel(new OperationService(db.Factory), dialogService.Object, snackBar.Object);

        await model.OpenCreateOperationDialogAsync();

        snackBar.Verify(x => x.Show("Added operation Print", Severity.Success), Times.Exactly(expectedCount));
    }

    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public async Task OpenEditOperationDialog(bool confirm, int expectedCount)
    {
        var dialogService = new Mock<IDialogService>();
        var operation = new Operation("Print", order: 1, new Device("Printer", DeviceType.Printer));
        dialogService
            .Setup(x => x.Show<It.IsAnyType, Operation, It.IsAnyType>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<It.IsAnyType>()))
            .Returns(Task.FromResult(confirm ? operation : null));
        var snackBar = new Mock<ISnackbar>();
        var model = new IndexViewModel(new OperationService(db.Factory), dialogService.Object, snackBar.Object);

        await model.OpenEditOperationDialogAsync(operation);

        snackBar.Verify(x => x.Show("Updated operation", Severity.Success), Times.Exactly(expectedCount));
    }

    [Fact]
    public async Task MoveOperation()
    {
        var operationService = new OperationService(db.Factory);
        var deviceService = new DeviceService(db.Factory);
        var device = new Device("Printer", DeviceType.Printer);
        await deviceService.AddDeviceAsync(device);
        var operation = new Operation("Operation 1", order: 1, device);
        await operationService.AddOperationAsync(operation);
        await operationService.AddOperationAsync(new Operation("Operation 2", order: 2, device));
        var model = new IndexViewModel(operationService, new Mock<IDialogService>().Object, new Mock<ISnackbar>().Object);

        await model.MoveOperationDownAsync(operation);
        Assert.Equal(2, (await operationService.GetOperationAsync(1)).Order);
        Assert.Equal(1, (await operationService.GetOperationAsync(2)).Order);

        await model.MoveOperationUpAsync(operation);
        Assert.Equal(1, (await operationService.GetOperationAsync(1)).Order);
        Assert.Equal(2, (await operationService.GetOperationAsync(2)).Order);
    }

    [Theory]
    [InlineData(true, 0)]
    [InlineData(false, 1)]
    public async Task DeleteOperation(bool confirm, int expectedCount)
    {
        var operationService = new OperationService(db.Factory);
        var deviceService = new DeviceService(db.Factory);
        var dialogService = new Mock<IDialogService>();
        var device = new Device("Printer", DeviceType.Printer);
        await deviceService.AddDeviceAsync(device);
        var operation = new Operation("Operation 1", order: 1, device);
        await operationService.AddOperationAsync(operation);
        dialogService
         .Setup(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
         .Returns(Task.FromResult<bool?>(confirm));
        var model = new IndexViewModel(operationService, dialogService.Object, new Mock<ISnackbar>().Object);
        await model.OnInitializedAsync();
        await model.DeleteOperationAsync(operation = model.Operations.First(x => x.Id == operation.Id));

        Assert.Equal(expectedCount, model.Operations.Count);
        Assert.Equal(expectedCount, (await operationService.GetOperationsAsync()).Length);
    }
}
