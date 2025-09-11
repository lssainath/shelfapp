# 🔐 Secure Supabase Setup Guide

This guide shows you how to securely configure your Supabase credentials without exposing them in your code.

## 🚨 **IMPORTANT: Never commit API keys to version control!**

## Method 1: Environment Variables (Recommended)

### Step 1: Set up your Supabase project
1. Go to [supabase.com](https://supabase.com) and create an account
2. Create a new project
3. Get your credentials from **Settings → API**

### Step 2: Set environment variables

#### **Windows (PowerShell):**
```powershell
# Set environment variables for current session
$env:SUPABASE_URL="https://your-project-id.supabase.co"
$env:SUPABASE_ANON_KEY="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Or set them permanently
[Environment]::SetEnvironmentVariable("SUPABASE_URL", "https://your-project-id.supabase.co", "User")
[Environment]::SetEnvironmentVariable("SUPABASE_ANON_KEY", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...", "User")
```

#### **Windows (Command Prompt):**
```cmd
setx SUPABASE_URL "https://your-project-id.supabase.co"
setx SUPABASE_ANON_KEY "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

#### **macOS/Linux:**
```bash
# Add to your ~/.bashrc or ~/.zshrc
export SUPABASE_URL="https://your-project-id.supabase.co"
export SUPABASE_ANON_KEY="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

### Step 3: Restart your IDE/terminal
After setting environment variables, restart your IDE and terminal for changes to take effect.

## Method 2: Local Configuration File (Development Only)

### Step 1: Create a local config file
1. Copy `Services/SupabaseConfig.template.cs` to `Services/SupabaseConfig.cs`
2. Replace the placeholder values with your actual credentials:

```csharp
public static class SupabaseConfig
{
    // Direct configuration (ONLY for development)
    public const string SupabaseUrl = "https://your-actual-project-id.supabase.co";
    public const string SupabaseAnonKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
}
```

### Step 2: Add to .gitignore
Make sure `Services/SupabaseConfig.cs` is in your `.gitignore`:

```
# Add this line to .gitignore
Services/SupabaseConfig.cs
```

## Method 3: User Secrets (Alternative)

For .NET projects, you can use user secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "Supabase:Url" "https://your-project-id.supabase.co"
dotnet user-secrets set "Supabase:AnonKey" "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

Then update `SupabaseConfig.cs` to read from user secrets.

## 🔒 Security Best Practices

1. **Never commit real credentials** to version control
2. **Use environment variables** for production deployments
3. **Rotate your API keys** regularly
4. **Use different keys** for development and production
5. **Monitor your Supabase usage** for unusual activity

## 🧪 Testing Your Setup

1. Build and run your app
2. Try adding a shelf and some items
3. Check your Supabase dashboard to see the data
4. If you see data in Supabase, your configuration is working!

## 🚨 If You Accidentally Committed Credentials

1. **Immediately rotate your API keys** in Supabase
2. **Remove the credentials** from your code
3. **Use git history rewriting** to remove them from version history
4. **Force push** to update the remote repository

## 📝 Environment Variables Reference

| Variable | Description | Example |
|----------|-------------|---------|
| `SUPABASE_URL` | Your Supabase project URL | `https://abcdefgh.supabase.co` |
| `SUPABASE_ANON_KEY` | Your Supabase anonymous key | `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...` |

## 🆘 Troubleshooting

- **"Invalid URL or Key"**: Double-check your credentials
- **Environment variables not working**: Restart your IDE/terminal
- **Permission denied**: Make sure you've set up the database schema
- **Still having issues**: Check the console output for detailed error messages
