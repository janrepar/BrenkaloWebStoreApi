PRAGMA foreign_keys=OFF;
BEGIN TRANSACTION;
CREATE TABLE UserSessions (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    token TEXT NOT NULL,
    request_IP TEXT DEFAULT NULL,
    valid_until TEXT NOT NULL,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE
);
CREATE TABLE Languages (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    code TEXT NOT NULL UNIQUE,
    code_table TEXT DEFAULT NULL,
    name TEXT NOT NULL,
    image_url TEXT NOT NULL,
    enabled INTEGER DEFAULT 1,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,
    updated_at TEXT DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(code, name)
);
INSERT INTO Languages VALUES(1,'en','eng','English','img/languages/en.jpg',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO Languages VALUES(2,'sl','slv','Slovenščina','img/languages/sl.jpg',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO Languages VALUES(3,'de','deu','German','img/languages/de.jpg',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
CREATE TABLE UserAddresses (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER NOT NULL,
    address_line1 TEXT NOT NULL,
    address_line2 TEXT,
    city TEXT NOT NULL,
    state TEXT,
    postal_code TEXT NOT NULL,
    country TEXT NOT NULL,
    is_default INTEGER DEFAULT 0,
    enabled INTEGER DEFAULT 1,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE
);
CREATE TABLE VATRates (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    country TEXT NOT NULL,
    vat_rate REAL NOT NULL,
    name TEXT NOT NULL,
    enabled INTEGER DEFAULT 1
);
CREATE TABLE Categories (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    parent_id INTEGER DEFAULT NULL,
    enabled INTEGER DEFAULT 1,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,
    updated_at TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (parent_id) REFERENCES Categories(id) ON DELETE SET NULL
);
CREATE TABLE CategoryTranslations (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    category_id INTEGER NOT NULL,
    language_id INTEGER NOT NULL,
    name TEXT NOT NULL,
    description TEXT DEFAULT NULL,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,
    updated_at TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES Categories(id) ON DELETE CASCADE,
    FOREIGN KEY (language_id) REFERENCES Languages(id) ON DELETE CASCADE,
    UNIQUE(category_id, language_id)
);
CREATE TABLE Products (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    sku TEXT NOT NULL UNIQUE,
    category_id INTEGER NOT NULL,
    subcategory_id INTEGER DEFAULT NULL,
    brand TEXT DEFAULT NULL,
    manufacturer TEXT DEFAULT NULL,
    model_number TEXT DEFAULT NULL,
    main_picture_url TEXT DEFAULT NULL,
    main_product_url TEXT DEFAULT NULL,
    is_featured INTEGER DEFAULT 0,
    price REAL NOT NULL,
    vat_id INTEGER DEFAULT NULL,
    is_on_sale INTEGER DEFAULT 0,
    sale_price REAL DEFAULT NULL,
    sale_start_date TEXT DEFAULT NULL,
    sale_end_date TEXT DEFAULT NULL,
    average_rating REAL DEFAULT 0.00,
    number_of_reviews INTEGER DEFAULT 0,
    popularity INTEGER DEFAULT 0,
    item_storage INTEGER DEFAULT 0,
    stock_status TEXT DEFAULT 'in_stock',
    minimum_order_quantity INTEGER DEFAULT 1,
    maximum_order_quantity INTEGER DEFAULT NULL,
    is_visible INTEGER DEFAULT 1,
    enabled INTEGER DEFAULT 1,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,
    updated_at TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES Categories(id) ON DELETE CASCADE,
    FOREIGN KEY (subcategory_id) REFERENCES Categories(id) ON DELETE SET NULL,
    FOREIGN KEY (vat_id) REFERENCES VATRates(id) ON DELETE SET NULL
);
CREATE TABLE ProductTranslations (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    product_id INTEGER NOT NULL,
    language_id INTEGER NOT NULL,
    name TEXT NOT NULL,
    description TEXT,
    status INTEGER DEFAULT 0,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,
    updated_at TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES Products(id) ON DELETE CASCADE,
    FOREIGN KEY (language_id) REFERENCES Languages(id) ON DELETE CASCADE,
    UNIQUE(product_id, language_id)
);
CREATE TABLE ProductDescriptionTypes (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    type_name TEXT NOT NULL UNIQUE,
    enabled INTEGER DEFAULT 1
);
CREATE TABLE ProductDescriptions (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    product_id INTEGER NOT NULL,
    language_id INTEGER NOT NULL,
    description TEXT NOT NULL,
    description_type_id INTEGER NOT NULL,
    FOREIGN KEY (product_id) REFERENCES Products(id) ON DELETE CASCADE,
    FOREIGN KEY (language_id) REFERENCES Languages(id) ON DELETE CASCADE,
    FOREIGN KEY (description_type_id) REFERENCES ProductDescriptionTypes(id) ON DELETE CASCADE
);
CREATE TABLE ProductPictures (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    parent_picture_id INTEGER DEFAULT NULL,
    product_id INTEGER NOT NULL,
    language_id INTEGER NOT NULL,
    picture_url TEXT NOT NULL,
    alt TEXT DEFAULT NULL,
    title TEXT DEFAULT NULL,
    description TEXT DEFAULT NULL,
    picture_type TEXT DEFAULT 'full',
    is_default INTEGER DEFAULT 0,
    sort_order INTEGER DEFAULT 0,
    enabled INTEGER DEFAULT 1,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,
    updated_at TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES Products(id) ON DELETE CASCADE,
    FOREIGN KEY (parent_picture_id) REFERENCES ProductPictures(id) ON DELETE SET NULL
);
CREATE TABLE ProductReviews (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    product_id INTEGER NOT NULL,
    user_id INTEGER NOT NULL,
    username TEXT NOT NULL,
    stars INTEGER NOT NULL CHECK (stars BETWEEN 1 AND 5),
    review_text TEXT,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (product_id) REFERENCES Products(id) ON DELETE CASCADE,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE
);
CREATE TABLE Orders (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    user_id INTEGER DEFAULT NULL,
    order_shipping_method TEXT NOT NULL,
    customer_name TEXT NOT NULL,
    customer_email TEXT NOT NULL,
    customer_phone TEXT DEFAULT NULL,
    shipping_address TEXT NOT NULL,
    billing_address TEXT DEFAULT NULL,
    order_status_id INTEGER NOT NULL,
    customer_notes TEXT DEFAULT NULL,
    total_amount REAL NOT NULL,
    vat_amount REAL DEFAULT 0.00,
    discount_amount REAL DEFAULT 0.00,
    payment_method TEXT DEFAULT 'credit_card',
    shipping_tracking_code TEXT DEFAULT NULL,
    created_at TEXT DEFAULT CURRENT_TIMESTAMP,
    updated_at TEXT DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE SET NULL,
    FOREIGN KEY (order_status_id) REFERENCES OrderStatuses(id)
);
CREATE TABLE Stores (
    id INTEGER PRIMARY KEY AUTOINCREMENT, -- Auto-incrementing primary key
    name TEXT NOT NULL, -- Store name
    address TEXT NOT NULL, -- Store address
    latitude REAL, -- Latitude (decimal equivalent)
    longitude REAL, -- Longitude (decimal equivalent)
    opening_hours TEXT NOT NULL, -- Opening hours
    contact_email TEXT, -- Contact email
    contact_phone TEXT, -- Contact phone
    manager_name TEXT, -- Manager's name
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP, -- Creation timestamp
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP -- Last update timestamp
);
INSERT INTO Stores VALUES(1,'Central Store','123 Main St, Ljubljana, Slovenia',46.0569460000000034,14.505751,'Mon-Fri: 8:00-20:00, Sat: 9:00-15:00','central@webshop.com','+38640123456','Janez Novak','2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO Stores VALUES(2,'North Store','45 North Rd, Maribor, Slovenia',46.5546500000000023,15.6458809999999992,'Mon-Fri: 9:00-18:00, Sat: 10:00-14:00','north@webshop.com','+38640123457','Marija Kranjc','2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO Stores VALUES(3,'South Store','78 South Ave, Koper, Slovenia',45.5480579999999974,13.7301870000000008,'Mon-Sun: 10:00-22:00','south@webshop.com','+38640123458','Aljoša Vidmar','2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO Stores VALUES(4,'East Store','34 East Blvd, Celje, Slovenia',46.2360169999999968,15.2677069999999996,'Mon-Fri: 8:00-19:00, Sat: 8:00-12:00','east@webshop.com','+38640123459','Bojan Horvat','2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO Stores VALUES(5,'West Store','90 West Str, Nova Gorica, Slovenia',45.9544410000000027,13.6491729999999993,'Mon-Fri: 7:00-19:00, Sat: 7:00-13:00','west@webshop.com','+38640123460','Katja Tomc','2024-12-07 19:20:23','2024-12-07 19:20:23');
CREATE TABLE StorePictures (
    id INTEGER PRIMARY KEY AUTOINCREMENT, -- Unique picture ID
    store_id INTEGER NOT NULL, -- Foreign key to Stores table
    language_id INTEGER NOT NULL, -- Foreign key to Languages table
    picture_url TEXT NOT NULL, -- URL of the picture
    alt TEXT, -- Alternative text for SEO
    title TEXT, -- Title of the picture
    description TEXT, -- Description of the picture
    is_default INTEGER DEFAULT 0, -- Default picture flag (0 = false, 1 = true)
    sort_order INTEGER DEFAULT 0, -- Order of the picture
    picture_type TEXT CHECK (picture_type IN ('thumbnail', 'medium', 'full')) DEFAULT 'full', -- Type of picture
    parent_picture_id INTEGER, -- Parent picture ID for different sizes
    size INTEGER, -- Size of the picture in bytes
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP, -- Creation timestamp
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP, -- Last update timestamp
    FOREIGN KEY (store_id) REFERENCES Stores(id) ON DELETE CASCADE, -- Cascade delete when store is removed
    FOREIGN KEY (parent_picture_id) REFERENCES StorePictures(id) ON DELETE SET NULL -- Set null if parent picture is removed
);
INSERT INTO StorePictures VALUES(1,1,1,'img/stores/central.jpg','Image of Central Store','Central Store','A beautiful image of the Central Store.',1,1,'full',NULL,204800,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StorePictures VALUES(2,2,1,'img/stores/north.jpg','Image of North Store','North Store','A beautiful image of the North Store.',1,1,'full',NULL,204800,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StorePictures VALUES(3,3,1,'img/stores/south.jpg','Image of South Store','South Store','A beautiful image of the South Store.',1,1,'full',NULL,204800,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StorePictures VALUES(4,4,1,'img/stores/east.jpg','Image of East Store','East Store','A beautiful image of the East Store.',1,1,'full',NULL,204800,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StorePictures VALUES(5,5,1,'img/stores/west.jpg','Image of West Store','West Store','A beautiful image of the West Store.',1,1,'full',NULL,204800,'2024-12-07 19:20:23','2024-12-07 19:20:23');
CREATE TABLE StoreMessages (
    id INTEGER PRIMARY KEY AUTOINCREMENT, -- Unique message ID
    store_id INTEGER NOT NULL, -- Foreign key to Stores table
    language_id INTEGER NOT NULL, -- Foreign key to Languages table
    user_id INTEGER, -- Foreign key to Users table
    title TEXT NOT NULL, -- Title of the message
    message TEXT NOT NULL, -- Message content
    status INTEGER DEFAULT 0, -- Message status (0 = unpublished, 1 = published)
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP, -- Creation timestamp
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP, -- Last update timestamp
    FOREIGN KEY (store_id) REFERENCES Stores(id) ON DELETE CASCADE, -- Cascade delete when store is removed
    FOREIGN KEY (language_id) REFERENCES Languages(id) ON DELETE CASCADE, -- Cascade delete when language is removed
    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE SET NULL -- Set null if user is removed
);
INSERT INTO StoreMessages VALUES(1,1,1,1,'Welcome to Central Store','We are delighted to have you visit our Central Store. Enjoy great deals!',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StoreMessages VALUES(2,1,2,1,'Central Store Hours','Our Central Store is open daily from 8:00 to 20:00.',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StoreMessages VALUES(3,2,1,2,'North Store Announcement','Join us at our North Store for exclusive discounts this week!',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StoreMessages VALUES(4,2,1,2,'Parking Information','Free parking is available at North Store for all visitors.',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StoreMessages VALUES(5,2,2,NULL,'North Store Opening','North Store opens at 9:00 on weekdays and 10:00 on weekends.',0,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StoreMessages VALUES(6,3,1,3,'South Store Update','Our South Store has extended hours this weekend!',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StoreMessages VALUES(7,3,2,1,'New Arrivals in South Store','Discover our new arrivals this month at South Store.',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StoreMessages VALUES(8,5,1,4,'Special Offer in West Store','Get up to 50% off selected items in West Store!',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO StoreMessages VALUES(9,5,2,4,'West Store Summer Hours','West Store is now open earlier during the summer season.',1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
CREATE TABLE IF NOT EXISTS "Users" (
	"id"	INTEGER,
	"userGUID"	TEXT,
	"username"	TEXT NOT NULL,
	"user_role"	TEXT NOT NULL,
	"firstname"	TEXT DEFAULT NULL,
	"lastname"	TEXT DEFAULT NULL,
	"pwd"	TEXT NOT NULL,
	"email"	TEXT NOT NULL UNIQUE,
	"user_language_id"	INTEGER DEFAULT NULL,
	"enabled"	INTEGER DEFAULT 1,
	"created_at"	TEXT DEFAULT CURRENT_TIMESTAMP,
	"updated_at"	TEXT DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY("id" AUTOINCREMENT),
	UNIQUE("userGUID"),
	UNIQUE("username")
);
INSERT INTO Users VALUES(1,'GUID-1','admin','1','Admin','User','hashed_password_1','admin@example.com',1,1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO Users VALUES(2,'GUID-2','manager','2','John','Doe','hashed_password_2','manager@example.com',2,1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO Users VALUES(3,'GUID-3','editor','3','Jane','Smith','hashed_password_3','editor@example.com',1,1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO Users VALUES(4,'GUID-4','user1','4','Alice','Johnson','hashed_password_4','alice@example.com',2,1,'2024-12-07 19:20:23','2024-12-07 19:20:23');
INSERT INTO Users VALUES(5,'GUID-5','user2','4','Bob','Brown','hashed_password_5','bob@example.com',NULL,0,'2024-12-07 19:20:23','2024-12-07 19:20:23');
CREATE TABLE OrderProducts (
    id INTEGER PRIMARY KEY AUTOINCREMENT,         -- Unikatni ID vnosa
    order_id INTEGER NOT NULL,                    -- Povezava na tabelo Orders
    product_id INTEGER NOT NULL,                  -- Povezava na tabelo Products (NE more biti NULL)
    product_name TEXT NOT NULL,                   -- Ime izdelka ob času naročila
    product_sku TEXT NOT NULL,                    -- SKU izdelka
    quantity INTEGER NOT NULL,                    -- Količina naročenih izdelkov
    price_per_unit REAL NOT NULL,                 -- Cena na enoto v času naročila (SQLite uses REAL type for decimals)
    total_price REAL NOT NULL,                    -- Skupna cena za izdelek (price_per_unit * quantity)
    vat_rate REAL DEFAULT 0.00,                   -- DDV stopnja v % (SQLite uses REAL type for decimals)
    vat_amount REAL DEFAULT 0.00,                 -- DDV znesek za ta izdelek
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP, -- Datum dodajanja izdelka v naročilo
    updated_at DATETIME DEFAULT CURRENT_TIMESTAMP, -- Datum zadnje posodobitve
    FOREIGN KEY (order_id) REFERENCES Orders(id) ON DELETE CASCADE, -- Če je naročilo izbrisano, izbriši tudi izdelke
    FOREIGN KEY (product_id) REFERENCES Products(id) ON DELETE CASCADE -- Če je izdelek izbrisan, izbriši tudi vrstico
);
DELETE FROM sqlite_sequence;
INSERT INTO sqlite_sequence VALUES('Users',5);
INSERT INTO sqlite_sequence VALUES('Languages',3);
INSERT INTO sqlite_sequence VALUES('Stores',5);
INSERT INTO sqlite_sequence VALUES('StorePictures',5);
INSERT INTO sqlite_sequence VALUES('StoreMessages',9);
COMMIT;
