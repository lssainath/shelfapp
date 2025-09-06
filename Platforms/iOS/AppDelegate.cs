using Foundation;
using Microsoft.Maui;

namespace ShelfApp;

/// <summary>
/// The application delegate for the iOS version of the app. MAUI
/// requires this class to bootstrap the application on iOS. Most of
/// your app logic remains in the shared code; the platform projects
/// primarily provide entry points and host configuration.
/// </summary>
[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}