namespace AssemblyLine.Tests.Devices;

using AssemblyLine.ViewModels.Devices;
using Moq;
using Xunit;

public class AddDeviceDialogViewModelTests : IDisposable
{
    private readonly TestDb db = new();

    public void Dispose()
    {
        db.Dispose();
    }

    [Fact]
    public async Task Submit()
    {
        var deviceService = new DeviceService(db.Factory);
        var expectedDevice = new Device("Printer", DeviceType.Printer);
        var additionalErrors = new Mock<IAdditionalErrors>();
        var model = new AddDeviceDialogViewModel(deviceService);
        await model.OnInitializedAsync();
        model.AdditionalErrors = additionalErrors.Object;
        model.Device.Name = expectedDevice.Name;
        model.Device.Type = expectedDevice.Type;

        await model.SubmitAsync();

        var addedDevice = await deviceService.GetDeviceAsync(1);
        Assert.Equal(1, addedDevice.Id);
        Assert.Equal(expectedDevice.Name, addedDevice.Name);
        Assert.Equal(expectedDevice.Type, addedDevice.Type);
    }
}
