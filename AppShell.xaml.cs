using Microsoft.Maui.Controls;

namespace ShelfApp;

/// <summary>
/// The AppShell wires together the navigation structure defined in
/// AppShell.xaml. It inherits from Shell and can handle navigation
/// events or configure route registrations if you're using URI based
/// navigation.
/// </summary>
public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        // Register routes for navigation
        Routing.RegisterRoute("ShelfDetailPage", typeof(ShelfDetailPage));
    }
}