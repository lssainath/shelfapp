using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using ShelfApp.Data;
using ShelfApp.Models;

namespace ShelfApp;

/// <summary>
/// Displays all shelves in a list format with Material Design styling.
/// Shows shelf number, description, item count, and item preview.
/// </summary>
public partial class SearchByShelfPage : ContentPage
{
    public SearchByShelfPage()
    {
        InitializeComponent();
        LoadShelves();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadShelves();
    }

    private async void LoadShelves()
    {
        try
        {
            LoadingIndicator.IsRunning = true;
            ShelvesCollectionView.IsVisible = false;

            // Wait a moment for any pending Supabase operations
            await Task.Delay(100);

            // Get all shelves from the repository
            var shelves = ShelfRepository.Shelves;
            
            // Update the collection view
            ShelvesCollectionView.ItemsSource = shelves;
            
            // Update the count label
            ShelfCountLabel.Text = $"{shelves.Count} shelf{(shelves.Count == 1 ? "" : "es")} found";
            
            LoadingIndicator.IsRunning = false;
            ShelvesCollectionView.IsVisible = true;
        }
        catch (Exception ex)
        {
            LoadingIndicator.IsRunning = false;
            await DisplayAlert("Error", $"Failed to load shelves: {ex.Message}", "OK");
        }
    }
}