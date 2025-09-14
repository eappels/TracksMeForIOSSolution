using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Maps;
using System.ComponentModel;
using System.Diagnostics;
using TracksMeForIOS.Messages;
using TracksMeForIOS.ViewModels;

namespace TracksMeForIOS.Views;

public partial class MapView : ContentPage
{

    private double zoomLevel = 150;
    private bool isZooming = false;
    private PropertyChangedEventHandler mapPropertyChangedHandler;

    public MapView(MapViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

        WeakReferenceMessenger.Default.Register<LocationUpdatedMessage>(this, (r, m) =>
        {
            if (MyMap != null && m.Value != null)
            {
                if (MyMap.MapElements.Count == 0)
                {
                    var track = ((MapViewModel)BindingContext).Track;
                    if (track != null)
                        MyMap.MapElements.Add(track);
                }
                if (!isZooming)
                    MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Location(m.Value.Latitude, m.Value.Longitude), Distance.FromMeters(zoomLevel)));
            }
        });
        if (MyMap != null)
        {
            mapPropertyChangedHandler = (s, e) =>
            {
                isZooming = true;
                if (e.PropertyName == "VisibleRegion")
                {
                    if (MyMap.VisibleRegion != null)
                        zoomLevel = MyMap.VisibleRegion.Radius.Meters;
                }
            };
            MyMap.PropertyChanged += mapPropertyChangedHandler;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var location = await GetCachedLocation();
        if (MyMap != null && location != null)
        {
            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(location, Distance.FromMeters(zoomLevel)));
        }
    }

    public async Task<Location> GetCachedLocation()
    {
        try
        {
            Location? location = await Geolocation.Default.GetLastKnownLocationAsync();
            if (location != null)
                return location;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving cached location: {ex.Message}");
        }
        return new Location();
    }
}