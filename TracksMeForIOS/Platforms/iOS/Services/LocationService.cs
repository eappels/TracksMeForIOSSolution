using CoreLocation;

namespace TracksMeForIOS.Services;

public partial class LocationService
{

    public readonly CLLocationManager locationManager;
    private Location? previousLocation = new Location(0, 0);

    public LocationService()
    {
        OnLocationUpdate = _ => { };
        locationManager = new CLLocationManager();
        locationManager.PausesLocationUpdatesAutomatically = false;
        locationManager.DesiredAccuracy = CLLocation.AccuracyBestForNavigation;
        locationManager.AllowsBackgroundLocationUpdates = true;
        locationManager.ActivityType = CLActivityType.AutomotiveNavigation;
    }

    partial void StartTrackingInternal()
    {
        locationManager.LocationsUpdated += (object? sender, CLLocationsUpdatedEventArgs e) =>
        {
            var lastLocation = e.Locations?.LastOrDefault();
            if (lastLocation != null)
            {
                var newLocation = new Location(lastLocation.Coordinate.Latitude, lastLocation.Coordinate.Longitude);
                OnLocationUpdate?.Invoke(newLocation);
                previousLocation = newLocation;
            }
        };
        locationManager.StartUpdatingLocation();
    }

    partial void StopTrackingInternal()
    {
        locationManager.StopUpdatingLocation();
    }

    private static double GetDistanceInMeters(Location loc1, Location loc2)
    {
        double R = 6371000; // Radius of Earth in meters
        double lat1Rad = Math.PI * loc1.Latitude / 180.0;
        double lat2Rad = Math.PI * loc2.Latitude / 180.0;
        double deltaLat = lat2Rad - lat1Rad;
        double deltaLon = Math.PI * (loc2.Longitude - loc1.Longitude) / 180.0;

        double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                   Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                   Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }
}
