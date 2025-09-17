using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace ShelfApp;

/// <summary>
/// Configures and builds the MAUI application. This class is invoked by the
/// platform‑specific entry points to create the app and register fonts or
/// services. Adjust fonts or dependency injection here as needed.
/// </summary>
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // Register the main application class. This determines which page
        // is displayed on startup. Fonts are not registered here because
        // this sample does not include custom fonts. If you add fonts to
        // the Resources/Fonts folder you can register them via
        // ConfigureFonts.
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        return builder.Build();
    }
}