using System.Threading.Tasks;
using Microsoft.Maui.Controls.Maps;
using Moq;
using TracksMeForIOS.Data.Models;
using TracksMeForIOS.Interfaces;
using TracksMeForIOS.ViewModels;
using Xunit;

namespace TracksMeForIOSTests;

public class MapViewModelTests
{
    [Fact]
    public void Constructor_InitializesTrackAndButtonProperties()
    {
        var locationServiceMock = new Mock<ILocationService>();
        var vm = new MapViewModel(locationServiceMock.Object);

        Assert.NotNull(vm.Track);
        Assert.Equal("Start", vm.StartStopButtonText);
        Assert.Equal(Colors.Green, vm.StartStopButtonColor);
        Assert.Equal(Colors.Blue, vm.Track.StrokeColor);
        Assert.Equal(5, vm.Track.StrokeWidth);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenLocationServiceIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new MapViewModel(null));
    }

    [Fact]
    public void Dispose_UnsubscribesAndStopsTracking()
    {
        var locationServiceMock = new Mock<ILocationService>();
        var vm = new MapViewModel(locationServiceMock.Object);

        vm.Dispose();

        locationServiceMock.VerifyRemove(x => x.OnLocationUpdate -= It.IsAny<System.Action<Location>>(), Times.Once);
        locationServiceMock.Verify(x => x.StopTracking(), Times.Once);
    }

    [Fact]
    public async Task StartStop_StartsTrackingAndUpdatesButtonProperties()
    {
        var locationServiceMock = new Mock<ILocationService>();
        var vm = new MapViewModel(locationServiceMock.Object);

        await vm.StartStopCommand.ExecuteAsync(null);

        locationServiceMock.Verify(x => x.StartTracking(), Times.Once);
        Assert.Equal("Stop", vm.StartStopButtonText);
        Assert.Equal(Colors.Red, vm.StartStopButtonColor);
    }

    [Fact]
    public async Task StartStop_StopsTrackingAndUpdatesButtonProperties()
    {
        var locationServiceMock = new Mock<ILocationService>();
        var vm = new MapViewModel(locationServiceMock.Object);

        // Set state to "Stop"
        vm.StartStopButtonText = "Stop";

        await vm.StartStopCommand.ExecuteAsync(null);

        locationServiceMock.Verify(x => x.StopTracking(), Times.Once);
        Assert.Equal("Start", vm.StartStopButtonText);
        Assert.Equal(Colors.Green, vm.StartStopButtonColor);
    }

    [Fact]
    public void OnLocationUpdate_AddsLocationToTrack()
    {
        var locationServiceMock = new Mock<ILocationService>();
        var vm = new MapViewModel(locationServiceMock.Object);
        var location = new Location(1, 2);

        // Use reflection to invoke private method
        var method = typeof(MapViewModel).GetMethod("OnLocationUpdate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        method.Invoke(vm, new object[] { location });

        Assert.Contains(location, vm.Track.Geopath);
    }
}