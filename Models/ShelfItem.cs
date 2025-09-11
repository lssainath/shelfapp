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
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the shelf. Typically a shelf
    /// number or code used to locate the shelf in a warehouse.
    /// </summary>
    public string ShelfNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the shelf.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last updated timestamp.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the collection of item names associated with this shelf.
    /// This is not stored directly in the database but populated from the items table.
    /// </summary>
    public ObservableCollection<string> Items { get; set; } = new();
}