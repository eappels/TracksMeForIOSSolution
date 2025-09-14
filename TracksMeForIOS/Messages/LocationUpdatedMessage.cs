using CommunityToolkit.Mvvm.Messaging.Messages;

namespace TracksMeForIOS.Messages;

public class LocationUpdatedMessage : ValueChangedMessage<Location>
{
    public LocationUpdatedMessage(Location value)
        : base(value)
    {
    }
}