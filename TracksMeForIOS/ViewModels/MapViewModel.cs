using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Controls.Maps;
using System.Diagnostics;
using TracksMeForIOS.Data.Models;
using TracksMeForIOS.Interfaces;
using TracksMeForIOS.Messages;

namespace TracksMeForIOS.ViewModels;

public partial class MapViewModel : ObservableObject, IDisposable
{

    private readonly ILocationService locationService;

    public MapViewModel(ILocationService locationService)
    {
        Track = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };
        this.locationService = locationService ?? throw new ArgumentNullException(nameof(locationService));
        this.locationService.OnLocationUpdate += OnLocationUpdate;
    }

    private async void OnLocationUpdate(Location location)
    {
        if (Track != null)
            Track.Geopath.Add(location);
        WeakReferenceMessenger.Default.Send(new LocationUpdatedMessage(location));
    }

    public void Dispose()
    {
        if (locationService != null)
        {
            locationService.OnLocationUpdate -= OnLocationUpdate;
            locationService.StopTracking();
        }
    }

    [RelayCommand]
    private async Task StartStop()
    {
        if (StartStopButtonText == "Start")
        {
            locationService.StartTracking();
            StartStopButtonText = "Stop";
            StartStopButtonColor = Colors.Red;
        }
        else
        {
            locationService.StopTracking();
            StartStopButtonText = "Start";
            StartStopButtonColor = Colors.Green;

            if (Track?.Geopath?.Count > 0)
            {
                var window = App.Current?.Windows.FirstOrDefault();
                var page = window?.Page;
                if (page != null)
                {
                    var result = await page.DisplayAlert("Save Track", "Do you want to save the current track?", "Yes", "No");
                    if (result == true)
                    {
                        var customTrack = new CustomTrack();
                        result = await page.DisplayAlert("Track saved", "Do you want to display the saved track?", "Yes", "No");
                        if (result == true)
                        {
                            await Shell.Current.GoToAsync($"///HistoryView", true);
                        }
                    }
                }
                Track.Geopath.Clear();
                StartStopButtonColor = Colors.Green;
            }
        }
    }

    [ObservableProperty]
    public Polyline track;

    [ObservableProperty]
    public string startStopButtonText = "Start";

    [ObservableProperty]
    public Color startStopButtonColor = Colors.Green;
}