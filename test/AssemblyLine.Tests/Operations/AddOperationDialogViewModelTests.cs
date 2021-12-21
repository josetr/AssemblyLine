namespace AssemblyLine.Tests.Operations;

using AssemblyLine.ViewModels.Operations;
using Moq;
using Xunit;

public class AddOperationDialogViewModelTests : IDisposable
{
    private readonly TestDb db = new();

    public void Dispose()
    {
        db.Dispose();
    }

    [Fact]
    public async Task Submit()
    {
        var operationService = new OperationService(db.Factory);
        var deviceService = new DeviceService(db.Factory);
        var device = new Device("Printer", DeviceType.Printer);
        await deviceService.AddDeviceAsync(device);
        var additionalErrors = new Mock<IAdditionalErrors>();
        var model = new AddOperationDialogViewModel(operationService, deviceService);
        await model.OnInitializedAsync();
        var expectedOperation = new Operation("Operation", order: 1, device);
        model.AdditionalErrors = additionalErrors.Object;
        model.Operation.Name = expectedOperation.Name;
        model.Operation.Device = expectedOperation.Device;

        Assert.Equal(1, model.Operation.Order);

        await model.SubmitAsync();

        var addedOperation = await operationService.GetOperationAsync(1);
        Assert.Equal(1, addedOperation.Id);
        Assert.Equal(expectedOperation.Name, addedOperation.Name);
        Assert.Equal(expectedOperation.Order, addedOperation.Order);
        Assert.Equal(expectedOperation.Device.Id, addedOperation.Device.Id);
        Assert.Equal("Printer", addedOperation.Device.Name);
        Assert.Equal(DeviceType.Printer, addedOperation.Device.Type);

        await model.SubmitAsync();

        additionalErrors.Verify(x => x.DisplayError(It.Is<FieldTakenException>(x => x.Message == "Duplicate Order")));
    }
}
