using System;
using Microsoft.Maui.Controls;
using ShelfApp.Data;

namespace ShelfApp;

/// <summary>
/// Handles querying for items by shelf number. When the user presses
/// the "Get Items" button the repository is queried and the
/// CollectionView bound to the items collection.
/// </summary>
public partial class SearchByShelfPage : ContentPage
{
    public SearchByShelfPage()
    {
        InitializeComponent();
    }

    private void OnGetItemsClicked(object sender, EventArgs e)
    {
        var shelfNumber = ShelfEntry.Text?.Trim();
        if (string.IsNullOrEmpty(shelfNumber))
        {
            MessageLabel.Text = "Please enter a shelf number.";
            MessageLabel.TextColor = Colors.Red;
            ItemsCollectionView.ItemsSource = null;
            return;
        }

        var shelf = ShelfRepository.GetShelfByNumber(shelfNumber);
        if (shelf == null)
        {
            MessageLabel.Text = $"Shelf '{shelfNumber}' not found.";
            MessageLabel.TextColor = Colors.Red;
            ItemsCollectionView.ItemsSource = null;
        }
        else
        {
            ItemsCollectionView.ItemsSource = shelf.Items;
            MessageLabel.Text = string.Empty;
        }
    }
}