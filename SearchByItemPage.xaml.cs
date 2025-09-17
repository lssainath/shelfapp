using System;
using System.Linq;
using Microsoft.Maui.Controls;
using ShelfApp.Data;

namespace ShelfApp;

/// <summary>
/// Handles querying for a shelf number by item name. Validates user
/// input and displays the shelf or a message if not found.
/// </summary>
public partial class SearchByItemPage : ContentPage
{
    public SearchByItemPage()
    {
        InitializeComponent();
    }

    private async void OnGetShelfClicked(object sender, EventArgs e)
    {
        var item = ItemEntry.Text?.Trim();
        if (string.IsNullOrEmpty(item))
        {
            ResultLabel.Text = "Please enter an item.";
            ResultLabel.TextColor = Colors.Red;
            return;
        }

        try
        {
            // Find all shelves that contain this item
            var allShelves = ShelfRepository.Shelves;
            var matchingShelves = allShelves.Where(shelf => 
                shelf.Items.Any(i => i.Equals(item, StringComparison.OrdinalIgnoreCase))).ToList();

            if (matchingShelves.Any())
            {
                if (matchingShelves.Count == 1)
                {
                    ResultLabel.Text = $"Item '{item}' is on shelf '{matchingShelves.First().ShelfNumber}'.";
                }
                else
                {
                    var shelfNumbers = string.Join(", ", matchingShelves.Select(s => s.ShelfNumber));
                    ResultLabel.Text = $"Item '{item}' is on {matchingShelves.Count} shelves: {shelfNumbers}.";
                }
                ResultLabel.TextColor = Colors.Green;
            }
            else
            {
                ResultLabel.Text = $"Item '{item}' not found in any shelf.";
                ResultLabel.TextColor = Colors.Red;
            }
        }
        catch (Exception ex)
        {
            ResultLabel.Text = $"Error searching for item: {ex.Message}";
            ResultLabel.TextColor = Colors.Red;
        }
    }
}