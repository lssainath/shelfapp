-- ShelfApp Database Schema for Supabase
-- Run this SQL in your Supabase SQL Editor

-- Create shelves table
CREATE TABLE IF NOT EXISTS shelves (
    id SERIAL PRIMARY KEY,
    shelf_number VARCHAR(50) UNIQUE NOT NULL,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Create items table
CREATE TABLE IF NOT EXISTS items (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    shelf_id INTEGER NOT NULL REFERENCES shelves(id) ON DELETE CASCADE,
    quantity INTEGER DEFAULT 1,
    description TEXT,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Create indexes for better performance
CREATE INDEX IF NOT EXISTS idx_shelves_shelf_number ON shelves(shelf_number);
CREATE INDEX IF NOT EXISTS idx_items_shelf_id ON items(shelf_id);
CREATE INDEX IF NOT EXISTS idx_items_name ON items(name);

-- Enable Row Level Security (RLS)
ALTER TABLE shelves ENABLE ROW LEVEL SECURITY;
ALTER TABLE items ENABLE ROW LEVEL SECURITY;

-- Create policies for public access (adjust as needed for your security requirements)
CREATE POLICY "Allow public read access to shelves" ON shelves
    FOR SELECT USING (true);

CREATE POLICY "Allow public read access to items" ON items
    FOR SELECT USING (true);

CREATE POLICY "Allow public insert access to shelves" ON shelves
    FOR INSERT WITH CHECK (true);

CREATE POLICY "Allow public insert access to items" ON items
    FOR INSERT WITH CHECK (true);

CREATE POLICY "Allow public update access to shelves" ON shelves
    FOR UPDATE USING (true);

CREATE POLICY "Allow public update access to items" ON items
    FOR UPDATE USING (true);

CREATE POLICY "Allow public delete access to shelves" ON shelves
    FOR DELETE USING (true);

CREATE POLICY "Allow public delete access to items" ON items
    FOR DELETE USING (true);

-- Create function to update updated_at timestamp
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Create triggers to automatically update updated_at
CREATE TRIGGER update_shelves_updated_at BEFORE UPDATE ON shelves
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

CREATE TRIGGER update_items_updated_at BEFORE UPDATE ON items
    FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
