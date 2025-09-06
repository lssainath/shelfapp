using System.Collections.ObjectModel;

namespace ShelfApp.Models;

/// <summary>
/// Represents a shelf and the items stored on that shelf. Each
/// shelf has a unique identifier (ShelfNumber) and a collection of
/// items. Items are stored as a simple list of strings for this
/// demonstration. In a more sophisticated app you could create a
/// separate model for items with additional properties.
/// </summary>
public class ShelfItem
{
    /// <summary>
    /// Gets or sets the identifier for the shelf. Typically a shelf
    /// number or code used to locate the shelf in a warehouse.
    /// </summary>
    public string ShelfNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets the collection of item names associated with this shelf.
    /// An ObservableCollection is used so that any UI bound to this
    /// collection will update automatically when items are added or
    /// removed.
    /// </summary>
    public ObservableCollection<string> Items { get; set; } = new();
}