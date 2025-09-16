using Microsoft.Extensions.Configuration;

namespace ShelfApp.Services;

/// <summary>
/// Configuration class for Supabase connection settings.
/// Reads from environment variables or user secrets for security.
/// </summary>
public static class SupabaseConfig
{
    private static IConfiguration? _configuration;
    
    public static void Initialize(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public static string SupabaseUrl => 
        _configuration?["Supabase:Url"] ?? 
        Environment.GetEnvironmentVariable("SUPABASE_URL") ?? 
        throw new InvalidOperationException("Supabase URL not configured. Set SUPABASE_URL environment variable or configure user secrets.");
    
    public static string SupabaseAnonKey => 
        _configuration?["Supabase:AnonKey"] ?? 
        Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY") ?? 
        throw new InvalidOperationException("Supabase Anon Key not configured. Set SUPABASE_ANON_KEY environment variable or configure user secrets.");
}
