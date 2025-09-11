# Supabase Setup Instructions

This guide will help you set up Supabase for the ShelfApp to enable data persistence.

## Step 1: Create a Supabase Project

1. Go to [supabase.com](https://supabase.com)
2. Sign up or log in to your account
3. Click "New Project"
4. Choose your organization
5. Enter project details:
   - Name: `shelfapp` (or any name you prefer)
   - Database Password: Create a strong password
   - Region: Choose the closest region to you
6. Click "Create new project"
7. Wait for the project to be created (this may take a few minutes)

## Step 2: Get Your Project Credentials

1. In your Supabase dashboard, go to **Settings** → **API**
2. Copy the following values:
   - **Project URL** (looks like: `https://your-project-id.supabase.co`)
   - **anon public** key (starts with `eyJ...`)

## Step 3: Update Configuration

1. Open `Services/SupabaseConfig.cs` in your project
2. Replace the placeholder values:
   ```csharp
   public const string SupabaseUrl = "YOUR_ACTUAL_SUPABASE_URL_HERE";
   public const string SupabaseAnonKey = "YOUR_ACTUAL_SUPABASE_ANON_KEY_HERE";
   ```

## Step 4: Set Up Database Schema

1. In your Supabase dashboard, go to **SQL Editor**
2. Click "New query"
3. Copy the contents of `Database/schema.sql`
4. Paste it into the SQL editor
5. Click "Run" to execute the script

This will create:
- `shelves` table for storing shelf information
- `items` table for storing item information
- Proper indexes for performance
- Row Level Security policies
- Triggers for automatic timestamp updates

## Step 5: Test the Connection

1. Build and run your app
2. Try adding a shelf and some items
3. Check your Supabase dashboard → **Table Editor** to see the data

## Step 6: (Optional) Configure Authentication

If you want to add user authentication later:
1. Go to **Authentication** → **Settings** in your Supabase dashboard
2. Configure your preferred authentication providers
3. Update the app to use authentication

## Troubleshooting

### Common Issues:

1. **"Invalid URL or Key" error**: Double-check your Supabase URL and anon key
2. **"Permission denied" error**: Make sure you've run the schema.sql script
3. **"Table doesn't exist" error**: Verify the database schema was created successfully

### Getting Help:

- Check the [Supabase Documentation](https://supabase.com/docs)
- Visit the [Supabase Discord](https://discord.supabase.com)
- Check the app console output for detailed error messages

## Security Notes

- The current setup allows public read/write access to all data
- For production apps, you should implement proper authentication and row-level security
- Consider using environment variables or secure configuration for your API keys
