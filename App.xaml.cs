using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace ShelfApp;

/// <summary>
/// The App class is the entry point for the MAUI application. It sets
/// the main page of the application to the Shell defined in AppShell.
/// </summary>
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Set the root page to the AppShell which defines the flyout and
        // navigation structure for this application.
        MainPage = new AppShell();
    }
}