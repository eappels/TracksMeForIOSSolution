using SQLite;

namespace TracksMeForIOS.Data.Models;

public class CustomLocation
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int CustomTrackId { get; set; }

    public CustomLocation()
    {
    }

    public CustomLocation(double latitude, double longitude, int customTrackId)
    {
        Latitude = latitude;
        Longitude = longitude;
        CustomTrackId = customTrackId;
    }
}