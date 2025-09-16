using Supabase;
using Supabase.Gotrue;
using Supabase.Postgrest.Models;
using ShelfApp.Models;
using ShelfApp.Services;

namespace ShelfApp.Data;

/// <summary>
/// Service for interacting with Supabase database.
/// Handles all CRUD operations for shelves and items.
/// </summary>
public class SupabaseService
{
    private Supabase.Client? _supabase;

    /// <summary>
    /// Initializes the Supabase client with configuration.
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            var options = new SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            _supabase = new Supabase.Client(SupabaseConfig.SupabaseUrl, SupabaseConfig.SupabaseAnonKey, options);
            await _supabase.InitializeAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing Supabase: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Gets all shelves from the database.
    /// </summary>
    public async Task<List<ShelfItem>> GetAllShelvesAsync()
    {
        if (_supabase == null) throw new InvalidOperationException("Supabase not initialized");

        try
        {
            var response = await _supabase
                .From<ShelfItem>()
                .Select("*")
                .Get();

            return response.Models;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting shelves: {ex.Message}");
            return new List<ShelfItem>();
        }
    }

    /// <summary>
    /// Creates a new shelf in the database.
    /// </summary>
    public async Task<bool> CreateShelfAsync(string shelfNumber, string? description = null)
    {
        if (_supabase == null) throw new InvalidOperationException("Supabase not initialized");

        try
        {
            var shelf = new ShelfItem
            {
                ShelfNumber = shelfNumber,
                Description = description,
                Items = new List<string>()
            };

            await _supabase
                .From<ShelfItem>()
                .Insert(shelf);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating shelf: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Adds an item to a shelf in the database.
    /// </summary>
    public async Task<bool> AddItemToShelfAsync(string shelfNumber, string item, int quantity = 1, string? description = null)
    {
        if (_supabase == null) throw new InvalidOperationException("Supabase not initialized");

        try
        {
            // First, get the existing shelf
            var response = await _supabase
                .From<ShelfItem>()
                .Select("*")
                .Where(x => x.ShelfNumber == shelfNumber)
                .Single();

            if (response == null) return false;

            // Add the item to the shelf's items list
            if (!response.Items.Contains(item))
            {
                response.Items.Add(item);
                
                // Update the shelf in the database
                await _supabase
                    .From<ShelfItem>()
                    .Where(x => x.ShelfNumber == shelfNumber)
                    .Set(x => x.Items, response.Items)
                    .Update();
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding item to shelf: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Gets the shelf number that contains the specified item.
    /// </summary>
    public async Task<string?> GetShelfNumberByItemAsync(string item)
    {
        if (_supabase == null) throw new InvalidOperationException("Supabase not initialized");

        try
        {
            var response = await _supabase
                .From<ShelfItem>()
                .Select("*")
                .Get();

            var shelf = response.Models.FirstOrDefault(s => 
                s.Items.Any(i => i.Equals(item, StringComparison.OrdinalIgnoreCase)));

            return shelf?.ShelfNumber;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting shelf by item: {ex.Message}");
            return null;
        }
    }
}


