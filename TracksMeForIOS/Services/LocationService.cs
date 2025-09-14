using TracksMeForIOS.Interfaces;

namespace TracksMeForIOS.Services;

public partial class LocationService : ILocationService
{

    public event Action<Location> OnLocationUpdate;

    public void StartTracking()
    {
        StartTrackingInternal();
    }

    public void StopTracking()
    {
        StopTrackingInternal();
    }

    partial void StartTrackingInternal();
    partial void StopTrackingInternal();
}