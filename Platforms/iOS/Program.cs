using UIKit;

namespace ShelfApp;

/// <summary>
/// The main entry point for the iOS application. This method is
/// responsible for launching the iOS app. It specifies the
/// application delegate class that MAUI should use.
/// </summary>
public class Program
{
    static void Main(string[] args)
    {
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}