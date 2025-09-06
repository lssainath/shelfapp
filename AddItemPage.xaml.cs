using System;
using Microsoft.Maui.Controls;
using ShelfApp.Data;

namespace ShelfApp;

/// <summary>
/// Handles adding items to shelves. Validates user input and
/// updates the repository accordingly.
/// </summary>
public partial class AddItemPage : ContentPage
{
    public AddItemPage()
    {
        InitializeComponent();
    }

    private void OnAddItemClicked(object sender, EventArgs e)
    {
        var shelfNumber = ShelfEntry.Text?.Trim();
        var item = ItemEntry.Text?.Trim();

        if (string.IsNullOrEmpty(shelfNumber) || string.IsNullOrEmpty(item))
        {
            MessageLabel.Text = "Please enter both a shelf number and an item.";
            MessageLabel.TextColor = Colors.Red;
            return;
        }

        var shelf = ShelfRepository.GetShelfByNumber(shelfNumber);
        if (shelf == null)
        {
            MessageLabel.Text = $"Shelf '{shelfNumber}' does not exist. Please add it first.";
            MessageLabel.TextColor = Colors.Red;
            return;
        }

        ShelfRepository.AddItemToShelf(shelfNumber, item);
        MessageLabel.Text = $"Item '{item}' added to shelf '{shelfNumber}'.";
        MessageLabel.TextColor = Colors.Green;
        ItemEntry.Text = string.Empty;
    }
}