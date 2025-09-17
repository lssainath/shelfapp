using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using ShelfApp.Data;
using ShelfApp.Models;

namespace ShelfApp;

/// <summary>
/// Displays all shelves in a list format with Material Design styling.
/// Shows shelf number, description, item count, and item preview.
/// Includes search functionality to filter shelves by item names.
/// </summary>
public partial class SearchByShelfPage : ContentPage
{
    private ObservableCollection<ShelfItem> _allShelves = new();
    private ObservableCollection<ShelfItem> _filteredShelves = new();

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

            // Wait a moment for any pending operations
            await Task.Delay(100);

            // Get all shelves from the repository (real data)
            var shelves = ShelfRepository.Shelves;
            
            // Update our collections
            _allShelves.Clear();
            foreach (var shelf in shelves)
            {
                _allShelves.Add(shelf);
            }
            
            // Apply current search filter
            ApplySearchFilter();
            
            LoadingIndicator.IsRunning = false;
            ShelvesCollectionView.IsVisible = true;
        }
        catch (Exception ex)
        {
            LoadingIndicator.IsRunning = false;
            await DisplayAlert("Error", $"Failed to load shelves: {ex.Message}", "OK");
        }
    }

    private void ApplySearchFilter()
    {
        var searchText = SearchEntry?.Text?.Trim().ToLowerInvariant();
        
        _filteredShelves.Clear();
        
        if (string.IsNullOrWhiteSpace(searchText))
        {
            // Show all shelves if no search text
            foreach (var shelf in _allShelves)
            {
                _filteredShelves.Add(shelf);
            }
        }
        else
        {
            // Filter shelves that contain the search item
            var matchingShelves = _allShelves.Where(shelf => 
                shelf.Items.Any(item => item.ToLowerInvariant().Contains(searchText)));
            
            foreach (var shelf in matchingShelves)
            {
                _filteredShelves.Add(shelf);
            }
        }
        
        // Update the collection view
        ShelvesCollectionView.ItemsSource = _filteredShelves;
        
        // Update the count label
        if (string.IsNullOrWhiteSpace(searchText))
        {
            ShelfCountLabel.Text = $"{_filteredShelves.Count} shelf{(_filteredShelves.Count == 1 ? "" : "es")} found";
        }
        else
        {
            ShelfCountLabel.Text = $"{_filteredShelves.Count} shelf{(_filteredShelves.Count == 1 ? "" : "es")} containing '{searchText}'";
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        ApplySearchFilter();
    }

    private void OnClearSearchClicked(object sender, EventArgs e)
    {
        SearchEntry.Text = string.Empty;
        ApplySearchFilter();
    }

    private async void OnShelfTapped(object sender, EventArgs e)
    {
        if (sender is Element element && element.BindingContext is ShelfItem shelf)
        {
            // Navigate to shelf detail page
            await Shell.Current.GoToAsync($"ShelfDetailPage?ShelfNumber={shelf.ShelfNumber}");
        }
    }

    private async void OnAddShelfClicked(object sender, EventArgs e)
    {
        try
        {
            // Prompt user for shelf number
            string shelfNumber = await DisplayPromptAsync("Add Shelf", "Enter shelf number:", "Add", "Cancel", "e.g., A1, B2, etc.");
            
            if (string.IsNullOrWhiteSpace(shelfNumber))
                return;

            // Check if shelf already exists (case-insensitive)
            var existingShelf = ShelfRepository.GetShelfByNumber(shelfNumber.Trim());
            if (existingShelf != null)
            {
                await DisplayAlert("Error", $"Shelf '{shelfNumber}' already exists! Please choose a different shelf number.", "OK");
                return;
            }

            // Prompt user for optional description
            string description = await DisplayPromptAsync("Add Shelf", "Enter description (optional):", "Add", "Skip", "e.g., Electronics, Books, etc.");

            // Add shelf to repository (real operation)
            bool success = await ShelfRepository.AddShelfAsync(shelfNumber.Trim(), description?.Trim());
            
            if (success)
            {
                await DisplayAlert("Success", $"Shelf '{shelfNumber}' added successfully!", "OK");
                LoadShelves(); // Refresh the list to show new shelf
            }
            else
            {
                await DisplayAlert("Error", $"Failed to add shelf '{shelfNumber}'. Please try again.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add shelf: {ex.Message}", "OK");
        }
    }
}