using System.Collections.ObjectModel;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ShelfApp.Models;

/// <summary>
/// Represents a shelf and the items stored on that shelf. Each
/// shelf has a unique identifier (ShelfNumber) and a collection of
/// items. Items are stored as a simple list of strings for this
/// demonstration. In a more sophisticated app you could create a
/// separate model for items with additional properties.
/// </summary>
[Table("shelves")]
public class ShelfItem : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the identifier for the shelf. Typically a shelf
    /// number or code used to locate the shelf in a warehouse.
    /// </summary>
    [Column("shelf_number")]
    public string ShelfNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the shelf.
    /// </summary>
    [Column("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the creation timestamp.
    /// </summary>
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the last updated timestamp.
    /// </summary>
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the collection of item names associated with this shelf.
    /// This is not stored directly in the database but populated from the items table.
    /// </summary>
    [Column("items")]
    public List<string> Items { get; set; } = new();

    /// <summary>
    /// Gets the items as an ObservableCollection for UI binding.
    /// </summary>
    public ObservableCollection<string> ItemsObservable => new(Items);
}