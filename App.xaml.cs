using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShelfApp.Data;
using ShelfApp.Services;

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

    protected override async void OnStart()
    {
        base.OnStart();
        
        // Initialize configuration
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<App>()
            .AddEnvironmentVariables()
            .Build();
            
        SupabaseConfig.Initialize(configuration);
        
        // Initialize Supabase
        try
        {
            await ShelfRepository.InitializeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing Supabase: {ex.Message}");
            // Continue with local storage as fallback
        }
    }
}