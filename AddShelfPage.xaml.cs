using System;
using Microsoft.Maui.Controls;
using ShelfApp.Data;

namespace ShelfApp;

/// <summary>
/// Code‑behind for the AddShelfPage. Handles user interaction for
/// adding a new shelf number.
/// </summary>
public partial class AddShelfPage : ContentPage
{
    public AddShelfPage()
    {
        InitializeComponent();
    }

    private void OnAddShelfClicked(object sender, EventArgs e)
    {
        var shelfNumber = ShelfEntry.Text?.Trim();
        if (!string.IsNullOrEmpty(shelfNumber))
        {
            try
            {
                ShelfRepository.AddShelf(shelfNumber);
                MessageLabel.Text = $"Shelf '{shelfNumber}' added.";
                MessageLabel.TextColor = Colors.Green;
                ShelfEntry.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MessageLabel.Text = ex.Message;
                MessageLabel.TextColor = Colors.Red;
            }
        }
        else
        {
            MessageLabel.Text = "Please enter a shelf number.";
            MessageLabel.TextColor = Colors.Red;
        }
    }
}