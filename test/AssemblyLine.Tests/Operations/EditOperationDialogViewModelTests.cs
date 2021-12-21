namespace AssemblyLine.Tests.Operations;

using AssemblyLine.ViewModels.Operations;
using Moq;
using Xunit;

public class EditOperationDialogViewModelTests : IDisposable
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
        var camera = new Device("Camera", DeviceType.Camera);
        await deviceService.AddDeviceAsync(device);
        await deviceService.AddDeviceAsync(camera);
        await operationService.AddOperationAsync(new Operation("Operation 1", order: 1, device));
        await operationService.AddOperationAsync(new Operation("Operation 2", order: 2, device));
        var additionalErrors = new Mock<IAdditionalErrors>();
        var model = new EditOperationDialogViewModel(operationService, deviceService);
        model.OperationId = 1;
        await model.OnInitializedAsync();
        var expectedOperation = new Operation("Operation 2", order: 3, camera);
        model.AdditionalErrors = additionalErrors.Object;
        model.Operation.Name = expectedOperation.Name;
        model.Operation.Device = expectedOperation.Device;
        model.Operation.Order = 2;

        await model.Submit();
        additionalErrors.Verify(x => x.DisplayError(It.Is<FieldTakenException>(x => x.Message == "Duplicate Order")));

        model.Operation.Order = expectedOperation.Order;
        additionalErrors.Invocations.Clear();

        await model.Submit();

        additionalErrors.Verify(x => x.DisplayError(It.IsAny<Exception>()), Times.Never);

        var editedOperation = await operationService.GetOperationAsync(1);
        Assert.Equal(1, editedOperation.Id);
        Assert.Equal(model.Operation.Name, editedOperation.Name);
        Assert.Equal(model.Operation.Order, editedOperation.Order);
        Assert.Equal(model.Operation.Device.Id, editedOperation.Device.Id);
        Assert.Equal("Camera", editedOperation.Device.Name);
        Assert.Equal(DeviceType.Camera, editedOperation.Device.Type);
    }
}
