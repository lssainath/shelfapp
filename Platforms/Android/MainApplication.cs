using Android.App;
using Android.Runtime;
using Microsoft.Maui;
using System;

namespace ShelfApp;

/// <summary>
/// The Android application class is responsible for creating the
/// MAUI app. MAUI uses this entry point to initialise the framework
/// and register platform services. This class should not contain
/// application logic.
/// </summary>
[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}