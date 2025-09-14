namespace TracksMeForIOS.Interfaces;

public interface ILocationService
{
    //Action<Location>? OnLocationUpdate { get; set; }
    event Action<Location> OnLocationUpdate;
    void StartTracking();
    void StopTracking();
}