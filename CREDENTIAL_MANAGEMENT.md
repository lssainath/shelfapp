# 🔐 Credential Management Guide

This guide explains different ways to manage your Supabase credentials securely across different computers and scenarios.

## 🎯 **Your Options (Ranked by Ease of Use):**

### **Option 1: User Secrets (Recommended for Development)**
**Best for:** Personal development, multiple computers, easy setup

**How it works:**
- Credentials stored locally on each machine
- Automatically synced with your project
- Easy to set up on new computers

**Setup:**
```bash
# Set your credentials (run once per computer)
dotnet user-secrets set "Supabase:Url" "https://your-project-id.supabase.co"
dotnet user-secrets set "Supabase:AnonKey" "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# List your secrets (to verify)
dotnet user-secrets list
```

**Pros:**
- ✅ Easy to set up on new computers
- ✅ Secure (not in code)
- ✅ Works with your existing setup
- ✅ Can be shared with team members

**Cons:**
- ❌ Need to set up on each computer
- ❌ Lost if computer dies (unless backed up)

---

### **Option 2: Environment Variables (Production)**
**Best for:** Production deployments, CI/CD, server environments

**Setup:**
```powershell
# Windows PowerShell
$env:SUPABASE_URL="https://your-project-id.supabase.co"
$env:SUPABASE_ANON_KEY="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Set permanently
[Environment]::SetEnvironmentVariable("SUPABASE_URL", "https://your-project-id.supabase.co", "User")
```

**Pros:**
- ✅ Industry standard
- ✅ Works in production
- ✅ Can be set by deployment systems

**Cons:**
- ❌ Manual setup on each computer
- ❌ Lost if computer dies

---

### **Option 3: Configuration File (Not Recommended)**
**Best for:** Quick testing only

**Setup:**
- Create `Services/SupabaseConfig.cs` with real credentials
- Add to `.gitignore`

**Pros:**
- ✅ Quick setup

**Cons:**
- ❌ Easy to accidentally commit
- ❌ Not secure
- ❌ Hard to manage across computers

---

## 🚀 **Recommended Workflow:**

### **For Development:**
1. **Use User Secrets** (easiest)
2. **Document your credentials** somewhere safe (password manager, encrypted note)
3. **Set up on each new computer** with the same commands

### **For Production:**
1. **Use Environment Variables** or cloud secret management
2. **Never store in code**

---

## 📝 **Setting Up on a New Computer:**

### **Method 1: User Secrets (Recommended)**
```bash
# Clone your project
git clone https://github.com/yourusername/shelfapp.git
cd shelfapp

# Set up credentials (you'll need your Supabase URL and key)
dotnet user-secrets set "Supabase:Url" "https://your-project-id.supabase.co"
dotnet user-secrets set "Supabase:AnonKey" "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Build and run
dotnet build
dotnet run
```

### **Method 2: Environment Variables**
```powershell
# Set environment variables
$env:SUPABASE_URL="https://your-project-id.supabase.co"
$env:SUPABASE_ANON_KEY="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Build and run
dotnet build
dotnet run
```

---

## 🔒 **Backing Up Your Credentials:**

### **Safe Places to Store:**
1. **Password Manager** (1Password, LastPass, Bitwarden)
2. **Encrypted Notes** (Apple Notes, OneNote with encryption)
3. **Secure Cloud Storage** (with encryption)
4. **Physical Safe** (for really important projects)

### **What to Store:**
- Supabase Project URL
- Supabase Anon Key
- Database Password (if you need it)
- Any other API keys

---

## 🆘 **If You Lose Your Credentials:**

1. **Go to your Supabase dashboard**
2. **Settings → API**
3. **Copy your URL and anon key again**
4. **Set them up on your new computer**

---

## 🎯 **My Recommendation for You:**

**Use User Secrets** because:
- ✅ Easiest to set up on new computers
- ✅ Secure and professional
- ✅ Works with your current setup
- ✅ You can easily share the setup commands with others

**Just run these commands on any new computer:**
```bash
dotnet user-secrets set "Supabase:Url" "YOUR_URL_HERE"
dotnet user-secrets set "Supabase:AnonKey" "YOUR_KEY_HERE"
```

**And keep your credentials documented somewhere safe!**
