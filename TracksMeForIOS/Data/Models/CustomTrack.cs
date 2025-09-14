using SQLite;

namespace TracksMeForIOS.Data.Models;

public class CustomTrack
{

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    [Ignore]
    public IList<CustomLocation> Locations { get; set; } = new List<CustomLocation>();

    public CustomTrack()
    {
    }

    public CustomTrack(List<CustomLocation> Locations)
    {
        this.Locations = Locations ?? new List<CustomLocation>();
    }
}
