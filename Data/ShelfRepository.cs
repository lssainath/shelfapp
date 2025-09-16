using System;
using System.Collections.ObjectModel;
using System.Linq;
using ShelfApp.Models;

namespace ShelfApp.Data;

/// <summary>
/// Repository for managing shelves and their associated items.
/// Uses Supabase for data persistence.
/// </summary>
public static class ShelfRepository
{
    private static readonly ObservableCollection<ShelfItem> shelves = new();
    private static SupabaseService? _supabaseService;

    /// <summary>
    /// Gets the collection of shelves. The collection can be used to
    /// observe changes from the UI layer.
    /// </summary>
    public static ObservableCollection<ShelfItem> Shelves => shelves;

    /// <summary>
    /// Initializes the repository with Supabase service.
    /// </summary>
    public static async Task InitializeAsync()
    {
        try
        {
            _supabaseService = new SupabaseService();
            await _supabaseService.InitializeAsync();
            await LoadShelvesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing Supabase repository: {ex.Message}");
            // Continue with in-memory storage as fallback
        }
    }

    /// <summary>
    /// Loads all shelves from the database into the local collection.
    /// </summary>
    private static async Task LoadShelvesAsync()
    {
        if (_supabaseService == null) return;
        
        try
        {
            var dbShelves = await _supabaseService.GetAllShelvesAsync();
            shelves.Clear();
            foreach (var shelf in dbShelves)
            {
                shelves.Add(shelf);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading shelves: {ex.Message}");
        }
    }

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
    public static async Task<string?> GetShelfNumberByItemAsync(string item)
    {
        if (_supabaseService != null)
        {
            try
            {
                return await _supabaseService.GetShelfNumberByItemAsync(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting shelf number by item: {ex.Message}");
            }
        }
        
        // Fallback to local search
        var shelf = shelves.FirstOrDefault(s =>
            s.Items.Any(i => i.Equals(item, StringComparison.OrdinalIgnoreCase)));
        return shelf?.ShelfNumber;
    }

    /// <summary>
    /// Adds a new shelf to the repository if it does not already
    /// exist. Shelf numbers are compared case insensitively.
    /// </summary>
    /// <param name="number">The shelf number to add.</param>
    /// <param name="description">Optional description for the shelf.</param>
    public static async Task<bool> AddShelfAsync(string number, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Shelf number cannot be empty.", nameof(number));

        if (_supabaseService != null)
        {
            try
            {
                // Check if shelf already exists
                var existingShelf = GetShelfByNumber(number.Trim());
                if (existingShelf != null)
                {
                    return false; // Shelf already exists
                }

                // Create shelf in database
                var success = await _supabaseService.CreateShelfAsync(number.Trim(), description);
                if (success)
                {
                    await LoadShelvesAsync(); // Refresh local collection
                }
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding shelf: {ex.Message}");
            }
        }
        
        // Fallback to local storage
        try
        {
            if (!shelves.Any(s => s.ShelfNumber.Equals(number, StringComparison.OrdinalIgnoreCase)))
            {
                shelves.Add(new ShelfItem { ShelfNumber = number.Trim(), Description = description });
                return true;
            }
            return false; // Shelf already exists
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding shelf: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Adds an item to an existing shelf. If the shelf does not exist
    /// or the item is already on the shelf, the method does nothing.
    /// </summary>
    /// <param name="number">The shelf number to add the item to.</param>
    /// <param name="item">The item name.</param>
    /// <param name="quantity">The quantity of the item.</param>
    /// <param name="description">Optional description for the item.</param>
    public static async Task<bool> AddItemToShelfAsync(string number, string item, int quantity = 1, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(number) || string.IsNullOrWhiteSpace(item))
            throw new ArgumentException("Shelf number and item cannot be empty.");

        if (_supabaseService != null)
        {
            try
            {
                // Check if shelf exists
                var shelf = GetShelfByNumber(number.Trim());
                if (shelf == null)
                {
                    return false; // Shelf doesn't exist
                }

                // Add item to database
                var success = await _supabaseService.AddItemToShelfAsync(number.Trim(), item.Trim(), quantity, description);
                if (success)
                {
                    await LoadShelvesAsync(); // Refresh local collection
                }
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to shelf: {ex.Message}");
            }
        }
        
        // Fallback to local storage
        try
        {
            var shelf = GetShelfByNumber(number.Trim());
            if (shelf != null)
            {
                // Avoid duplicate items on the same shelf
                if (!shelf.Items.Contains(item))
                {
                    shelf.Items.Add(item.Trim());
                    return true;
                }
                return false; // Item already exists
            }
            return false; // Shelf doesn't exist
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding item to shelf: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Synchronous version for backward compatibility.
    /// </summary>
    public static void AddShelf(string number)
    {
        AddShelfAsync(number).Wait();
    }

    /// <summary>
    /// Synchronous version for backward compatibility.
    /// </summary>
    public static void AddItemToShelf(string number, string item)
    {
        AddItemToShelfAsync(number, item).Wait();
    }
}