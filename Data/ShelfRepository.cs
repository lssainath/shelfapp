using System;
using System.Collections.ObjectModel;
using System.Linq;
using ShelfApp.Models;

namespace ShelfApp.Data;

/// <summary>
/// A simple in‑memory repository for managing shelves and their
/// associated items. In a production application you might replace
/// this with a database or persistent storage service. The API here
/// allows adding shelves, adding items to shelves, and querying for
/// shelves or items.
/// </summary>
public static class ShelfRepository
{
    // Internal collection of shelves. This list is static so that
    // it persists for the lifetime of the application. Each shelf
    // contains a collection of item names.
    private static readonly ObservableCollection<ShelfItem> shelves = new();

    /// <summary>
    /// Gets the collection of shelves. The collection can be used to
    /// observe changes from the UI layer.
    /// </summary>
    public static ObservableCollection<ShelfItem> Shelves => shelves;

    /// <summary>
    /// Finds a shelf by its shelf number. Comparison is case
    /// insensitive.
    /// </summary>
    /// <param name="number">The shelf number to look up.</param>
    /// <returns>The matching <see cref="ShelfItem"/> or <c>null</c> if no
    /// shelf is found.</returns>
    public static ShelfItem? GetShelfByNumber(string number)
    {
        return shelves.FirstOrDefault(s =>
            s.ShelfNumber.Equals(number, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets the shelf number that contains the specified item. If
    /// multiple shelves contain the same item, the first match is
    /// returned.
    /// </summary>
    /// <param name="item">The item name to search for.</param>
    /// <returns>The shelf number containing the item, or <c>null</c> if
    /// no shelf contains the item.</returns>
    public static string? GetShelfNumberByItem(string item)
    {
        var shelf = shelves.FirstOrDefault(s =>
            s.Items.Any(i => i.Equals(item, StringComparison.OrdinalIgnoreCase)));
        return shelf?.ShelfNumber;
    }

    /// <summary>
    /// Adds a new shelf to the repository if it does not already
    /// exist. Shelf numbers are compared case insensitively.
    /// </summary>
    /// <param name="number">The shelf number to add.</param>
    public static void AddShelf(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Shelf number cannot be empty.", nameof(number));

        if (!shelves.Any(s => s.ShelfNumber.Equals(number, StringComparison.OrdinalIgnoreCase)))
        {
            shelves.Add(new ShelfItem { ShelfNumber = number.Trim() });
        }
    }

    /// <summary>
    /// Adds an item to an existing shelf. If the shelf does not exist
    /// or the item is already on the shelf, the method does nothing.
    /// </summary>
    /// <param name="number">The shelf number to add the item to.</param>
    /// <param name="item">The item name.</param>
    public static void AddItemToShelf(string number, string item)
    {
        if (string.IsNullOrWhiteSpace(number) || string.IsNullOrWhiteSpace(item))
            throw new ArgumentException("Shelf number and item cannot be empty.");

        var shelf = GetShelfByNumber(number.Trim());
        if (shelf != null)
        {
            // Avoid duplicate items on the same shelf.
            if (!shelf.Items.Contains(item))
            {
                shelf.Items.Add(item.Trim());
            }
        }
    }
}