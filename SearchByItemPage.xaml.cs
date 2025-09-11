using System;
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
            var shelfNumber = await ShelfRepository.GetShelfNumberByItemAsync(item);
            if (!string.IsNullOrEmpty(shelfNumber))
            {
                ResultLabel.Text = $"Item '{item}' is on shelf '{shelfNumber}'.";
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