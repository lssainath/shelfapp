using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using ShelfApp.Data;
using ShelfApp.Models;

namespace ShelfApp;

/// <summary>
/// Displays detailed view of a shelf with all its items.
/// Allows adding and deleting items with confirmation dialogs.
/// </summary>
[QueryProperty(nameof(ShelfNumber), "ShelfNumber")]
public partial class ShelfDetailPage : ContentPage
{
    private string _shelfNumber = string.Empty;
    private ShelfItem? _currentShelf;

    public string ShelfNumber
    {
        get => _shelfNumber;
        set
        {
            _shelfNumber = value;
            LoadShelfDetails();
        }
    }

    public ShelfDetailPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadShelfDetails();
    }

    private async void LoadShelfDetails()
    {
        if (string.IsNullOrWhiteSpace(ShelfNumber))
            return;

        try
        {
            LoadingIndicator.IsRunning = true;
            ItemsStackLayout.IsVisible = false;

            // Get shelf from repository
            _currentShelf = ShelfRepository.GetShelfByNumber(ShelfNumber);
            
            if (_currentShelf == null)
            {
                await DisplayAlert("Error", $"Shelf '{ShelfNumber}' not found.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            // Update UI
            ShelfNumberLabel.Text = $"Shelf {_currentShelf.ShelfNumber}";
            ShelfDescriptionLabel.Text = _currentShelf.Description ?? "No description";
            ShelfDescriptionLabel.IsVisible = !string.IsNullOrWhiteSpace(_currentShelf.Description);
            
            // Create item checkboxes dynamically
            CreateItemCheckboxes();
            
            // Update item count
            ItemCountLabel.Text = $"{_currentShelf.Items.Count} item{(_currentShelf.Items.Count == 1 ? "" : "s")}";
            
            // Show empty state if no items
            EmptyStateLayout.IsVisible = _currentShelf.Items.Count == 0;
            ItemsStackLayout.IsVisible = _currentShelf.Items.Count > 0;
            
            LoadingIndicator.IsRunning = false;
        }
        catch (Exception ex)
        {
            LoadingIndicator.IsRunning = false;
            await DisplayAlert("Error", $"Failed to load shelf details: {ex.Message}", "OK");
        }
    }

    private void CreateItemCheckboxes()
    {
        if (_currentShelf == null) return;

        // Clear existing items
        ItemsStackLayout.Children.Clear();

        // Create a checkbox for each item
        foreach (var item in _currentShelf.Items)
        {
            var itemFrame = new Frame
            {
                BackgroundColor = Colors.White,
                Padding = 0,
                Margin = new Thickness(0, 0, 0, 8),
                HasShadow = true,
                CornerRadius = 12
            };

            var itemGrid = new Grid
            {
                Padding = new Thickness(16),
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };

            // Item name with checkbox
            var itemLabel = new Label
            {
                Text = item,
                FontSize = 16,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.FromArgb("#212121"),
                VerticalOptions = LayoutOptions.Center
            };

            // Delete button
            var deleteButton = new Button
            {
                Text = "Delete",
                BackgroundColor = Color.FromArgb("#F44336"),
                TextColor = Colors.White,
                FontSize = 12,
                CornerRadius = 8,
                Padding = new Thickness(12, 8),
                CommandParameter = item
            };
            deleteButton.Clicked += OnDeleteItemClicked;

            Grid.SetColumn(itemLabel, 0);
            Grid.SetColumn(deleteButton, 1);

            itemGrid.Children.Add(itemLabel);
            itemGrid.Children.Add(deleteButton);
            itemFrame.Content = itemGrid;

            ItemsStackLayout.Children.Add(itemFrame);
        }
    }

    private async void OnAddItemClicked(object sender, EventArgs e)
    {
        if (_currentShelf == null)
            return;

        try
        {
            // Prompt user for item name
            string itemName = await DisplayPromptAsync("Add Item", $"Enter item name for shelf {_currentShelf.ShelfNumber}:", "Add", "Cancel", "e.g., iPhone, Book, etc.");
            
            if (string.IsNullOrWhiteSpace(itemName))
                return;

            // Add item to shelf
            bool success = await ShelfRepository.AddItemToShelfAsync(_currentShelf.ShelfNumber, itemName.Trim());
            
            if (success)
            {
                await DisplayAlert("Success", $"'{itemName}' added to shelf {_currentShelf.ShelfNumber}!", "OK");
                LoadShelfDetails(); // Refresh the view
            }
            else
            {
                await DisplayAlert("Error", $"'{itemName}' already exists on this shelf or could not be added.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add item: {ex.Message}", "OK");
        }
    }

    private async void OnDeleteItemClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is string itemName && _currentShelf != null)
        {
            try
            {
                // Show confirmation dialog
                bool confirm = await DisplayAlert(
                    "Delete Item", 
                    $"Are you sure you want to delete '{itemName}' from shelf {_currentShelf.ShelfNumber}?", 
                    "Yes", 
                    "No"
                );

                if (!confirm)
                    return;

                // Remove item from shelf
                bool success = await RemoveItemFromShelfAsync(_currentShelf.ShelfNumber, itemName);
                
                if (success)
                {
                    await DisplayAlert("Success", $"'{itemName}' deleted from shelf {_currentShelf.ShelfNumber}!", "OK");
                    LoadShelfDetails(); // Refresh the view
                }
                else
                {
                    await DisplayAlert("Error", $"Failed to delete '{itemName}' from shelf.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to delete item: {ex.Message}", "OK");
            }
        }
    }

    private async void OnDeleteShelfClicked(object sender, EventArgs e)
    {
        if (_currentShelf == null)
            return;

        try
        {
            // Show confirmation dialog
            bool confirm = await DisplayAlert(
                "Delete Shelf", 
                $"Are you sure you want to delete shelf '{_currentShelf.ShelfNumber}' and all its items?", 
                "Yes", 
                "No"
            );

            if (!confirm)
                return;

            // Remove shelf from repository
            bool success = await RemoveShelfAsync(_currentShelf.ShelfNumber);
            
            if (success)
            {
                await DisplayAlert("Success", $"Shelf '{_currentShelf.ShelfNumber}' deleted successfully!", "OK");
                await Shell.Current.GoToAsync(".."); // Navigate back
            }
            else
            {
                await DisplayAlert("Error", $"Failed to delete shelf '{_currentShelf.ShelfNumber}'.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to delete shelf: {ex.Message}", "OK");
        }
    }

    /// <summary>
    /// Removes an item from a shelf. This method should be added to ShelfRepository later.
    /// For now, we'll implement it here as a workaround.
    /// </summary>
    private Task<bool> RemoveItemFromShelfAsync(string shelfNumber, string itemName)
    {
        try
        {
            var shelf = ShelfRepository.GetShelfByNumber(shelfNumber);
            if (shelf != null && shelf.Items.Contains(itemName))
            {
                shelf.Items.Remove(itemName);
                
                // TODO: Update Supabase database here
                // For now, just update local collection
                
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        catch (Exception)
        {
            return Task.FromResult(false);
        }
    }

    /// <summary>
    /// Removes a shelf from the repository.
    /// </summary>
    private Task<bool> RemoveShelfAsync(string shelfNumber)
    {
        try
        {
            var shelf = ShelfRepository.GetShelfByNumber(shelfNumber);
            if (shelf != null)
            {
                ShelfRepository.Shelves.Remove(shelf);
                
                // TODO: Update Supabase database here
                // For now, just update local collection
                
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        catch (Exception)
        {
            return Task.FromResult(false);
        }
    }
}
