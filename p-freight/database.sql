-- 1. PRAGMAS
PRAGMA foreign_keys = ON;

-- 2. TENANT ISOLATION (ORGANISATIONS)
-- This table serves as the primary tenant table for multi-tenant isolation.
CREATE TABLE IF NOT EXISTS organisations (
    id TEXT PRIMARY KEY,
    name TEXT NOT NULL,
    country TEXT, -- Useful for SADC region specific targeting
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- 3. IDENTITY TABLES EXTENSION
-- Tenant isolation (organisation_id) added to the users table
-- Note: Caught and ignored in Program.cs if the column already exists (simulates ADD COLUMN IF NOT EXISTS)
ALTER TABLE users ADD COLUMN organisation_id TEXT REFERENCES organisations(id);


-- 4. CUSTOM DOMAIN TABLES (inDrive for trucks)

-- Vehicles/Trucks Registry
CREATE TABLE IF NOT EXISTS trucks (
    id TEXT PRIMARY KEY,
    organisation_id TEXT NOT NULL REFERENCES organisations(id),
    owner_id TEXT NOT NULL REFERENCES users(id),
    truck_type TEXT NOT NULL, -- e.g., '1-ton', '5-ton', '30-ton'
    registration_number TEXT NOT NULL,
    capacity_weight REAL,
    is_verified BOOLEAN DEFAULT 0,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Driver Profile (Extends User with logistics-specific properties)
CREATE TABLE IF NOT EXISTS driver_profiles (
    user_id TEXT PRIMARY KEY REFERENCES users(id),
    organisation_id TEXT NOT NULL REFERENCES organisations(id),
    license_number TEXT,
    is_verified BOOLEAN DEFAULT 0,
    rating REAL DEFAULT 0.0,
    total_trips INTEGER DEFAULT 0,
    current_location_lat REAL,
    current_location_lng REAL,
    is_online BOOLEAN DEFAULT 0,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Loads / Freight posted by customers
CREATE TABLE IF NOT EXISTS loads (
    id TEXT PRIMARY KEY,
    organisation_id TEXT NOT NULL REFERENCES organisations(id),
    customer_id TEXT NOT NULL REFERENCES users(id),
    pickup_address TEXT NOT NULL,
    pickup_lat REAL,
    pickup_lng REAL,
    dropoff_address TEXT NOT NULL,
    dropoff_lat REAL,
    dropoff_lng REAL,
    cargo_type TEXT NOT NULL, -- e.g. Furniture, Cement, Fuel
    weight REAL,
    special_notes TEXT,
    status TEXT DEFAULT 'PENDING', -- PENDING, BIDDING, ACCEPTED, IN_TRANSIT, DELIVERED, CANCELLED
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Load Bids (Offers placed by drivers on loads, core to inDrive model)
CREATE TABLE IF NOT EXISTS load_bids (
    id TEXT PRIMARY KEY,
    organisation_id TEXT NOT NULL REFERENCES organisations(id),
    load_id TEXT NOT NULL REFERENCES loads(id),
    driver_id TEXT NOT NULL REFERENCES users(id),
    truck_id TEXT REFERENCES trucks(id),
    offer_amount REAL NOT NULL,
    currency TEXT DEFAULT 'USD',
    status TEXT DEFAULT 'PENDING', -- PENDING, ACCEPTED, REJECTED, COUNTERED
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Active Trips / Deliveries
CREATE TABLE IF NOT EXISTS trips (
    id TEXT PRIMARY KEY,
    organisation_id TEXT NOT NULL REFERENCES organisations(id),
    load_id TEXT NOT NULL REFERENCES loads(id),
    driver_id TEXT NOT NULL REFERENCES users(id),
    truck_id TEXT NOT NULL REFERENCES trucks(id),
    agreed_price REAL NOT NULL,
    currency TEXT DEFAULT 'USD',
    status TEXT DEFAULT 'SCHEDULED', -- SCHEDULED, EN_ROUTE_TO_PICKUP, PICKED_UP, IN_TRANSIT, DELIVERED
    started_at DATETIME,
    completed_at DATETIME,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Trip Live Tracking (Location breadcrumbs)
CREATE TABLE IF NOT EXISTS trip_locations (
    id TEXT PRIMARY KEY,
    organisation_id TEXT NOT NULL REFERENCES organisations(id),
    trip_id TEXT NOT NULL REFERENCES trips(id),
    latitude REAL NOT NULL,
    longitude REAL NOT NULL,
    recorded_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Payments and Escrow
CREATE TABLE IF NOT EXISTS payments (
    id TEXT PRIMARY KEY,
    organisation_id TEXT NOT NULL REFERENCES organisations(id),
    trip_id TEXT NOT NULL REFERENCES trips(id),
    payer_id TEXT NOT NULL REFERENCES users(id),
    payee_id TEXT NOT NULL REFERENCES users(id),
    amount REAL NOT NULL,
    currency TEXT DEFAULT 'USD',
    payment_method TEXT, -- EcoCash, M-Pesa, Cash
    status TEXT DEFAULT 'PENDING', -- PENDING, IN_ESCROW, COMPLETED, FAILED
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Reviews and Ratings
CREATE TABLE IF NOT EXISTS reviews (
    id TEXT PRIMARY KEY,
    organisation_id TEXT NOT NULL REFERENCES organisations(id),
    trip_id TEXT NOT NULL REFERENCES trips(id),
    reviewer_id TEXT NOT NULL REFERENCES users(id), -- Either Customer rating Driver or vice versa
    reviewee_id TEXT NOT NULL REFERENCES users(id),
    rating INTEGER NOT NULL, 
    comment TEXT,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Negotiation Chats / Messages
CREATE TABLE IF NOT EXISTS messages (
    id TEXT PRIMARY KEY,
    organisation_id TEXT NOT NULL REFERENCES organisations(id),
    load_id TEXT NOT NULL REFERENCES loads(id),
    sender_id TEXT NOT NULL REFERENCES users(id),
    receiver_id TEXT NOT NULL REFERENCES users(id),
    content TEXT NOT NULL,
    is_read BOOLEAN DEFAULT 0,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- 5. INDEXES FOR PERFORMANCE
CREATE INDEX IF NOT EXISTS idx_users_org ON users(organisation_id);
CREATE INDEX IF NOT EXISTS idx_loads_org_status ON loads(organisation_id, status);
CREATE INDEX IF NOT EXISTS idx_load_bids_load ON load_bids(load_id);
CREATE INDEX IF NOT EXISTS idx_trips_driver ON trips(driver_id);
CREATE INDEX IF NOT EXISTS idx_trip_locs_trip ON trip_locations(trip_id);
