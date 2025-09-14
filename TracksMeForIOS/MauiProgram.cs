using Microsoft.Extensions.Logging;
using TracksMeForIOS.Interfaces;
using TracksMeForIOS.Services;
using TracksMeForIOS.ViewModels;
using TracksMeForIOS.Views;

namespace TracksMeForIOS;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<ILocationService, LocationService>();

        builder.Services.AddSingleton<MapViewModel>();
        builder.Services.AddTransient<MapView>();

        return builder.Build();
    }
}
