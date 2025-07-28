-- Create and use the HYDRA database
CREATE DATABASE HYDRA;
GO

USE HYDRA;
GO

------------------------------------
-- 1. CORE USER AND LOOKUP TABLES --
------------------------------------

-- NEW: Stores the defined user roles
CREATE TABLE [dbo].[Roles] (
    [RoleID] INT IDENTITY(1,1) NOT NULL,
    [RoleName] NVARCHAR(50) NOT NULL UNIQUE,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([RoleID])
);
GO

-- Populate the Roles table with our defined roles
INSERT INTO [dbo].[Roles] (RoleName) VALUES ('Customer'), ('Manager'), ('Admin');
GO

-- MODIFIED: Users table now includes a RoleID to link to the Roles table
CREATE TABLE [dbo].[Users] (
    [UserID] INT IDENTITY(1,1) NOT NULL,
    [Username] NVARCHAR(100) NOT NULL UNIQUE,
    [Email] NVARCHAR(255) NOT NULL UNIQUE,
    [HashedPassword] NVARCHAR(MAX) NOT NULL,
    [DateCreated] DATETIME2 NOT NULL DEFAULT(GETDATE()),
    [RoleID] INT NOT NULL DEFAULT 1, -- Defaults to 1 (Customer)
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserID]),
    CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleID]) REFERENCES [Roles]([RoleID])
);

-- Stores developer and publisher names
CREATE TABLE [dbo].[Companies] (
    [CompanyID] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(150) NOT NULL UNIQUE,
    CONSTRAINT [PK_Companies] PRIMARY KEY ([CompanyID])
);

-- Stores game platforms like 'PC', 'PlayStation 5', etc.
CREATE TABLE [dbo].[Platforms] (
    [PlatformID] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL UNIQUE,
    CONSTRAINT [PK_Platforms] PRIMARY KEY ([PlatformID])
);

-- Stores game genres like 'RPG', 'Action', etc.
CREATE TABLE [dbo].[Genres] (
    [GenreID] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL UNIQUE,
    CONSTRAINT [PK_Genres] PRIMARY KEY ([GenreID])
);

-- Stores the user's status for a game, e.g., 'Playing', 'Backlog'
CREATE TABLE [dbo].[Statuses] (
    [StatusID] INT IDENTITY(1,1) NOT NULL,
    [StatusName] NVARCHAR(50) NOT NULL UNIQUE,
    CONSTRAINT [PK_Statuses] PRIMARY KEY ([StatusID])
);

---------------------------------
-- 2. GAME AND STORE TABLES    --
---------------------------------

-- The main table for game information and store details
CREATE TABLE [dbo].[Games] (
    [GameID] INT IDENTITY(1,1) NOT NULL,
    [Title] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [ReleaseDate] DATE NULL,
    [CoverArtPath] NVARCHAR(512) NULL,
    [PublisherID] INT NULL,
    [Price] DECIMAL(10, 2) NOT NULL DEFAULT 0.00,
    [IsForSale] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Games] PRIMARY KEY ([GameID]),
    CONSTRAINT [FK_Games_Companies] FOREIGN KEY ([PublisherID]) REFERENCES [Companies]([CompanyID])
);

-- Records of user purchases
CREATE TABLE [dbo].[Orders] (
    [OrderID] INT IDENTITY(1,1) NOT NULL,
    [UserID] INT NOT NULL,
    [OrderDate] DATETIME2 NOT NULL DEFAULT(GETDATE()),
    [TotalAmount] DECIMAL(10, 2) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([OrderID]),
    CONSTRAINT [FK_Orders_Users] FOREIGN KEY ([UserID]) REFERENCES [Users]([UserID])
);

-- Line items for each order
CREATE TABLE [dbo].[OrderItems] (
    [OrderItemID] INT IDENTITY(1,1) NOT NULL,
    [OrderID] INT NOT NULL,
    [GameID] INT NOT NULL,
    [PriceAtTimeOfPurchase] DECIMAL(10, 2) NOT NULL,
    CONSTRAINT [PK_OrderItems] PRIMARY KEY ([OrderItemID]),
    CONSTRAINT [FK_OrderItems_Orders] FOREIGN KEY ([OrderID]) REFERENCES [Orders]([OrderID]),
    CONSTRAINT [FK_OrderItems_Games] FOREIGN KEY ([GameID]) REFERENCES [Games]([GameID])
);

-- Tables for a persistent shopping cart
CREATE TABLE [dbo].[ShoppingCarts] (
    [CartID] INT IDENTITY(1,1) NOT NULL,
    [UserID] INT NOT NULL,
    [DateCreated] DATETIME2 NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT [PK_ShoppingCarts] PRIMARY KEY ([CartID]),
    CONSTRAINT [FK_ShoppingCarts_Users] FOREIGN KEY ([UserID]) REFERENCES [Users]([UserID])
);

CREATE TABLE [dbo].[CartItems] (
    [CartItemID] INT IDENTITY(1,1) NOT NULL,
    [CartID] INT NOT NULL,
    [GameID] INT NOT NULL,
    [DateAdded] DATETIME2 NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT [PK_CartItems] PRIMARY KEY ([CartItemID]),
    CONSTRAINT [FK_CartItems_ShoppingCarts] FOREIGN KEY ([CartID]) REFERENCES [ShoppingCarts]([CartID]) ON DELETE CASCADE,
    CONSTRAINT [FK_CartItems_Games] FOREIGN KEY ([GameID]) REFERENCES [Games]([GameID])
);

-----------------------------------------
-- 3. PERSONAL LIBRARY & IMAGE TABLES  --
-----------------------------------------

-- Connects a User to a Game they own
CREATE TABLE [dbo].[LibraryEntries] (
    [LibraryEntryID] INT IDENTITY(1,1) NOT NULL,
    [UserID] INT NOT NULL,
    [GameID] INT NOT NULL,
    [StatusID] INT NOT NULL,
    [UserRating] INT NULL,
    [HoursPlayed] DECIMAL(7, 2) NULL,
    [Notes] NVARCHAR(MAX) NULL,
    [DateAdded] DATETIME2 NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT [PK_LibraryEntries] PRIMARY KEY ([LibraryEntryID]),
    CONSTRAINT [FK_LibraryEntries_Users] FOREIGN KEY ([UserID]) REFERENCES [Users]([UserID]),
    CONSTRAINT [FK_LibraryEntries_Games] FOREIGN KEY ([GameID]) REFERENCES [Games]([GameID]),
    CONSTRAINT [FK_LibraryEntries_Statuses] FOREIGN KEY ([StatusID]) REFERENCES [Statuses]([StatusID]),
    CONSTRAINT [CK_UserRating] CHECK ([UserRating] IS NULL OR ([UserRating] >= 1 AND [UserRating] <= 10))
);

-- Stores paths to additional game images
CREATE TABLE [dbo].[GameImages] (
    [ImageID] INT IDENTITY(1,1) NOT NULL,
    [GameID] INT NOT NULL,
    [ImagePath] NVARCHAR(512) NOT NULL,
    [Description] NVARCHAR(255) NULL,
    CONSTRAINT [PK_GameImages] PRIMARY KEY ([ImageID]),
    CONSTRAINT [FK_GameImages_Games] FOREIGN KEY ([GameID]) REFERENCES [Games]([GameID])
);

-----------------------------------------
-- 4. JUNCTION TABLES (Many-to-Many)   --
-----------------------------------------

-- Links games to their developers
CREATE TABLE [dbo].[GameDevelopers] (
    [GameID] INT NOT NULL,
    [DeveloperID] INT NOT NULL,
    CONSTRAINT [PK_GameDevelopers] PRIMARY KEY ([GameID], [DeveloperID]),
    CONSTRAINT [FK_GameDevelopers_Games] FOREIGN KEY ([GameID]) REFERENCES [Games]([GameID]) ON DELETE CASCADE,
    CONSTRAINT [FK_GameDevelopers_Companies] FOREIGN KEY ([DeveloperID]) REFERENCES [Companies]([CompanyID]) ON DELETE CASCADE
);

-- Links games to their genres
CREATE TABLE [dbo].[GameGenres] (
    [GameID] INT NOT NULL,
    [GenreID] INT NOT NULL,
    CONSTRAINT [PK_GameGenres] PRIMARY KEY ([GameID], [GenreID]),
    CONSTRAINT [FK_GameGenres_Games] FOREIGN KEY ([GameID]) REFERENCES [Games]([GameID]) ON DELETE CASCADE,
    CONSTRAINT [FK_GameGenres_Genres] FOREIGN KEY ([GenreID]) REFERENCES [Genres]([GenreID]) ON DELETE CASCADE
);

-- Links games to the platforms they are available on
CREATE TABLE [dbo].[GamePlatforms] (
    [GameID] INT NOT NULL,
    [PlatformID] INT NOT NULL,
    CONSTRAINT [PK_GamePlatforms] PRIMARY KEY ([GameID], [PlatformID]),
    CONSTRAINT [FK_GamePlatforms_Games] FOREIGN KEY ([GameID]) REFERENCES [Games]([GameID]) ON DELETE CASCADE,
    CONSTRAINT [FK_GamePlatforms_Platforms] FOREIGN KEY ([PlatformID]) REFERENCES [Platforms]([PlatformID]) ON DELETE CASCADE
);
GO

-- Insert base lookup data
-- Insert statuses
INSERT INTO Statuses (StatusName) VALUES 
('In Library'), 
('Backlog'), 
('Playing'), 
('Completed'), 
('Dropped'), 
('Wishlist');

-- Insert companies (publishers/developers)
INSERT INTO Companies (Name) VALUES 
('Valve Corporation'),
('CD Projekt'),
('Rockstar Games'),
('Bethesda Softworks'),
('Epic Games'),
('Ubisoft'),
('Electronic Arts'),
('Activision Blizzard'),
('Square Enix'),
('Nintendo'),
('FromSoftware'),
('Kojima Productions');

-- Insert genres
INSERT INTO Genres (Name) VALUES 
('Action'),
('Adventure'),
('RPG'),
('Strategy'),
('Shooter'),
('Puzzle'),
('Simulation'),
('Horror'),
('Racing'),
('Sports'),
('Fighting'),
('Indie'),
('Platformer'),
('Survival');

-- Insert platforms
INSERT INTO Platforms (Name) VALUES 
('PC'),
('PlayStation 5'),
('Xbox Series X|S'),
('Nintendo Switch'),
('PlayStation 4'),
('Xbox One'),
('Mac'),
('Linux'),
('Mobile'),
('Steam Deck');

-- Insert games with proper foreign key references
INSERT INTO Games (Title, Description, ReleaseDate, CoverArtPath, PublisherID, Price, IsForSale) VALUES 
(
    'The Witcher 3: Wild Hunt',
    'The Witcher 3: Wild Hunt is a story-driven, next-generation open world role-playing game set in a visually stunning fantasy universe full of meaningful choices and impactful consequences. In The Witcher you play as the professional monster hunter, Geralt of Rivia, tasked with finding the Child of Prophecy in a vast open world rich with merchant cities, viking pirate islands, dangerous mountain passes, and forgotten caverns to explore.',
    '2015-05-19',
    'https://cdn.cloudflare.steamstatic.com/steam/apps/292030/header.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'CD Projekt'),
    39.99,
    1
),
(
    'Cyberpunk 2077',
    'Cyberpunk 2077 is an open-world, action-adventure story set in Night City, a megalopolis obsessed with power, glamour and body modification. You play as V, a mercenary outlaw going after a one-of-a-kind implant that is the key to immortality.',
    '2020-12-10',
    'https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/header.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'CD Projekt'),
    59.99,
    1
),
(
    'Grand Theft Auto V',
    'Grand Theft Auto V for PC offers players the option to explore the award-winning world of Los Santos and Blaine County in resolutions of up to 4K and beyond, as well as the chance to experience the game running at 60 frames per second.',
    '2013-09-17',
    'https://cdn.cloudflare.steamstatic.com/steam/apps/271590/header.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'Rockstar Games'),
    29.99,
    1
),
(
    'The Elder Scrolls V: Skyrim Special Edition',
    'The Elder Scrolls V: Skyrim Special Edition is the legendary open-world masterpiece that brings to life a complete virtual world for you to explore any way you choose. Play any type of character you can imagine, and do whatever you want.',
    '2016-10-28',
    'https://cdn.cloudflare.steamstatic.com/steam/apps/489830/header.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'Bethesda Softworks'),
    39.99,
    1
),
(
    'Fortnite',
    'Fortnite is the completely free multiplayer game where you and your friends can jump into Battle Royale or Fortnite Creative. Download now and jump into the action.',
    '2017-07-25',
    'https://cdn2.unrealengine.com/fortnite-chapter-4-season-4-3840x2160-d35912cc5587.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'Epic Games'),
    0.00,
    1
),
(
    'Counter-Strike 2',
    'For over two decades, Counter-Strike has offered an elite competitive experience, one shaped by millions of players from across the globe. Counter-Strike 2 marks the largest technical leap in Counter-Strike''s history.',
    '2023-09-27',
    'https://cdn.cloudflare.steamstatic.com/steam/apps/730/header.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'Valve Corporation'),
    0.00,
    1
),
(
    'Assassin''s Creed Valhalla',
    'Become Eivor, a legendary Viking raider on a quest for glory. Explore England''s Dark Ages as you raid your enemies, grow your settlement, and build your political power.',
    '2020-11-10',
    'https://cdn.cloudflare.steamstatic.com/steam/apps/2208920/header.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'Ubisoft'),
    49.99,
    1
),
(
    'EA Sports FC 24',
    'EA SPORTS FC 24 welcomes you to The World''s Game: the most authentic football experience ever with HyperMotionV, PlayStyles optimised by Opta, and a revolutionized Frostbite Engine.',
    '2023-09-29',
    'https://cdn.cloudflare.steamstatic.com/steam/apps/2195250/header.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'Electronic Arts'),
    69.99,
    1
),
(
    'Call of Duty: Modern Warfare III',
    'In the direct sequel to the record-breaking Call of Duty: Modern Warfare II, Captain Price and Task Force 141 face off against the ultimate threat.',
    '2023-11-10',
    'https://cdn.cloudflare.steamstatic.com/steam/apps/2519060/header.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'Activision Blizzard'),
    69.99,
    1
),
(
    'Elden Ring',
    'THE NEW FANTASY ACTION RPG. Rise, Tarnished, and be guided by grace to brandish the power of the Elden Ring and become an Elden Lord in the Lands Between.',
    '2022-02-25',
    'https://cdn.cloudflare.steamstatic.com/steam/apps/1245620/header.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'FromSoftware'),
    59.99,
    1
),
(
    'Death Stranding',
    'From legendary game creator Hideo Kojima comes an all-new, genre-defying experience. Sam Bridges must brave a world utterly transformed by the Death Stranding.',
    '2019-11-08',
    'https://cdn.cloudflare.steamstatic.com/steam/apps/1190460/header.jpg',
    (SELECT CompanyID FROM Companies WHERE Name = 'Kojima Productions'),
    39.99,
    1
),
(
    'Super Mario Odyssey',
    'Use amazing new abilities—like the power to capture and control objects, animals, and enemies—to collect Power Moons so you can power up the Odyssey airship.',
    '2017-10-27',
    'https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/ncom/en_US/games/switch/s/super-mario-odyssey-switch/hero',
    (SELECT CompanyID FROM Companies WHERE Name = 'Nintendo'),
    59.99,
    1
);

-- Insert game images/screenshots
DECLARE @GameIds TABLE (Title NVARCHAR(255), GameID INT);
INSERT INTO @GameIds (Title, GameID)
SELECT Title, GameID FROM Games;

INSERT INTO GameImages (GameID, ImagePath, Description) VALUES 
-- The Witcher 3 screenshots
((SELECT GameID FROM @GameIds WHERE Title = 'The Witcher 3: Wild Hunt'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/292030/ss_aeb8d2c9b89cfc62b7685a9fd5d8eb70b1765a8e.1920x1080.jpg', 'Geralt exploring the vast open world'),
((SELECT GameID FROM @GameIds WHERE Title = 'The Witcher 3: Wild Hunt'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/292030/ss_f8a4aaea6523c022b5b632a5b0a0c6a8d8a23f9d.1920x1080.jpg', 'Epic combat against monsters'),
((SELECT GameID FROM @GameIds WHERE Title = 'The Witcher 3: Wild Hunt'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/292030/ss_landscapes.jpg', 'Beautiful fantasy landscapes'),

-- Cyberpunk 2077 screenshots
((SELECT GameID FROM @GameIds WHERE Title = 'Cyberpunk 2077'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/ss_night_city.jpg', 'Night City in all its neon glory'),
((SELECT GameID FROM @GameIds WHERE Title = 'Cyberpunk 2077'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/ss_character.jpg', 'Character customization and cyberware'),
((SELECT GameID FROM @GameIds WHERE Title = 'Cyberpunk 2077'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/1091500/ss_combat.jpg', 'High-tech combat and weapons'),

-- GTA V screenshots
((SELECT GameID FROM @GameIds WHERE Title = 'Grand Theft Auto V'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/271590/ss_cityscape.jpg', 'Los Santos cityscape'),
((SELECT GameID FROM @GameIds WHERE Title = 'Grand Theft Auto V'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/271590/ss_heist.jpg', 'Action-packed heist missions'),
((SELECT GameID FROM @GameIds WHERE Title = 'Grand Theft Auto V'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/271590/ss_vehicles.jpg', 'Vehicle variety and customization'),

-- Elden Ring screenshots
((SELECT GameID FROM @GameIds WHERE Title = 'Elden Ring'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/1245620/ss_boss_fight.jpg', 'Epic boss battles'),
((SELECT GameID FROM @GameIds WHERE Title = 'Elden Ring'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/1245620/ss_world.jpg', 'The Lands Between'),
((SELECT GameID FROM @GameIds WHERE Title = 'Elden Ring'), 'https://cdn.cloudflare.steamstatic.com/steam/apps/1245620/ss_magic.jpg', 'Powerful magic and combat');

-- Link games to genres (GameGenres junction table)
INSERT INTO GameGenres (GameID, GenreID) VALUES 
-- The Witcher 3: RPG, Adventure, Action
((SELECT GameID FROM @GameIds WHERE Title = 'The Witcher 3: Wild Hunt'), (SELECT GenreID FROM Genres WHERE Name = 'RPG')),
((SELECT GameID FROM @GameIds WHERE Title = 'The Witcher 3: Wild Hunt'), (SELECT GenreID FROM Genres WHERE Name = 'Adventure')),
((SELECT GameID FROM @GameIds WHERE Title = 'The Witcher 3: Wild Hunt'), (SELECT GenreID FROM Genres WHERE Name = 'Action')),

-- Cyberpunk 2077: RPG, Action, Adventure
((SELECT GameID FROM @GameIds WHERE Title = 'Cyberpunk 2077'), (SELECT GenreID FROM Genres WHERE Name = 'RPG')),
((SELECT GameID FROM @GameIds WHERE Title = 'Cyberpunk 2077'), (SELECT GenreID FROM Genres WHERE Name = 'Action')),
((SELECT GameID FROM @GameIds WHERE Title = 'Cyberpunk 2077'), (SELECT GenreID FROM Genres WHERE Name = 'Adventure')),

-- GTA V: Action, Adventure
((SELECT GameID FROM @GameIds WHERE Title = 'Grand Theft Auto V'), (SELECT GenreID FROM Genres WHERE Name = 'Action')),
((SELECT GameID FROM @GameIds WHERE Title = 'Grand Theft Auto V'), (SELECT GenreID FROM Genres WHERE Name = 'Adventure')),

-- Skyrim: RPG, Adventure, Action
((SELECT GameID FROM @GameIds WHERE Title = 'The Elder Scrolls V: Skyrim Special Edition'), (SELECT GenreID FROM Genres WHERE Name = 'RPG')),
((SELECT GameID FROM @GameIds WHERE Title = 'The Elder Scrolls V: Skyrim Special Edition'), (SELECT GenreID FROM Genres WHERE Name = 'Adventure')),
((SELECT GameID FROM @GameIds WHERE Title = 'The Elder Scrolls V: Skyrim Special Edition'), (SELECT GenreID FROM Genres WHERE Name = 'Action')),

-- Fortnite: Shooter, Action
((SELECT GameID FROM @GameIds WHERE Title = 'Fortnite'), (SELECT GenreID FROM Genres WHERE Name = 'Shooter')),
((SELECT GameID FROM @GameIds WHERE Title = 'Fortnite'), (SELECT GenreID FROM Genres WHERE Name = 'Action')),

-- Counter-Strike 2: Shooter, Action
((SELECT GameID FROM @GameIds WHERE Title = 'Counter-Strike 2'), (SELECT GenreID FROM Genres WHERE Name = 'Shooter')),
((SELECT GameID FROM @GameIds WHERE Title = 'Counter-Strike 2'), (SELECT GenreID FROM Genres WHERE Name = 'Action')),

-- AC Valhalla: Action, Adventure, RPG
((SELECT GameID FROM @GameIds WHERE Title = 'Assassin''s Creed Valhalla'), (SELECT GenreID FROM Genres WHERE Name = 'Action')),
((SELECT GameID FROM @GameIds WHERE Title = 'Assassin''s Creed Valhalla'), (SELECT GenreID FROM Genres WHERE Name = 'Adventure')),
((SELECT GameID FROM @GameIds WHERE Title = 'Assassin''s Creed Valhalla'), (SELECT GenreID FROM Genres WHERE Name = 'RPG')),

-- EA Sports FC 24: Sports
((SELECT GameID FROM @GameIds WHERE Title = 'EA Sports FC 24'), (SELECT GenreID FROM Genres WHERE Name = 'Sports')),

-- COD MW3: Shooter, Action
((SELECT GameID FROM @GameIds WHERE Title = 'Call of Duty: Modern Warfare III'), (SELECT GenreID FROM Genres WHERE Name = 'Shooter')),
((SELECT GameID FROM @GameIds WHERE Title = 'Call of Duty: Modern Warfare III'), (SELECT GenreID FROM Genres WHERE Name = 'Action')),

-- Elden Ring: RPG, Action
((SELECT GameID FROM @GameIds WHERE Title = 'Elden Ring'), (SELECT GenreID FROM Genres WHERE Name = 'RPG')),
((SELECT GameID FROM @GameIds WHERE Title = 'Elden Ring'), (SELECT GenreID FROM Genres WHERE Name = 'Action')),

-- Death Stranding: Action, Adventure
((SELECT GameID FROM @GameIds WHERE Title = 'Death Stranding'), (SELECT GenreID FROM Genres WHERE Name = 'Action')),
((SELECT GameID FROM @GameIds WHERE Title = 'Death Stranding'), (SELECT GenreID FROM Genres WHERE Name = 'Adventure')),

-- Super Mario Odyssey: Platformer, Adventure
((SELECT GameID FROM @GameIds WHERE Title = 'Super Mario Odyssey'), (SELECT GenreID FROM Genres WHERE Name = 'Platformer')),
((SELECT GameID FROM @GameIds WHERE Title = 'Super Mario Odyssey'), (SELECT GenreID FROM Genres WHERE Name = 'Adventure'));

-- Link games to platforms (GamePlatforms junction table)
INSERT INTO GamePlatforms (GameID, PlatformID) VALUES 
-- Multi-platform games
((SELECT GameID FROM @GameIds WHERE Title = 'The Witcher 3: Wild Hunt'), (SELECT PlatformID FROM Platforms WHERE Name = 'PC')),
((SELECT GameID FROM @GameIds WHERE Title = 'The Witcher 3: Wild Hunt'), (SELECT PlatformID FROM Platforms WHERE Name = 'PlayStation 5')),
((SELECT GameID FROM @GameIds WHERE Title = 'The Witcher 3: Wild Hunt'), (SELECT PlatformID FROM Platforms WHERE Name = 'Xbox Series X|S')),
((SELECT GameID FROM @GameIds WHERE Title = 'The Witcher 3: Wild Hunt'), (SELECT PlatformID FROM Platforms WHERE Name = 'Nintendo Switch')),

((SELECT GameID FROM @GameIds WHERE Title = 'Cyberpunk 2077'), (SELECT PlatformID FROM Platforms WHERE Name = 'PC')),
((SELECT GameID FROM @GameIds WHERE Title = 'Cyberpunk 2077'), (SELECT PlatformID FROM Platforms WHERE Name = 'PlayStation 5')),
((SELECT GameID FROM @GameIds WHERE Title = 'Cyberpunk 2077'), (SELECT PlatformID FROM Platforms WHERE Name = 'Xbox Series X|S')),

((SELECT GameID FROM @GameIds WHERE Title = 'Grand Theft Auto V'), (SELECT PlatformID FROM Platforms WHERE Name = 'PC')),
((SELECT GameID FROM @GameIds WHERE Title = 'Grand Theft Auto V'), (SELECT PlatformID FROM Platforms WHERE Name = 'PlayStation 5')),
((SELECT GameID FROM @GameIds WHERE Title = 'Grand Theft Auto V'), (SELECT PlatformID FROM Platforms WHERE Name = 'Xbox Series X|S')),

((SELECT GameID FROM @GameIds WHERE Title = 'Fortnite'), (SELECT PlatformID FROM Platforms WHERE Name = 'PC')),
((SELECT GameID FROM @GameIds WHERE Title = 'Fortnite'), (SELECT PlatformID FROM Platforms WHERE Name = 'PlayStation 5')),
((SELECT GameID FROM @GameIds WHERE Title = 'Fortnite'), (SELECT PlatformID FROM Platforms WHERE Name = 'Xbox Series X|S')),
((SELECT GameID FROM @GameIds WHERE Title = 'Fortnite'), (SELECT PlatformID FROM Platforms WHERE Name = 'Nintendo Switch')),
((SELECT GameID FROM @GameIds WHERE Title = 'Fortnite'), (SELECT PlatformID FROM Platforms WHERE Name = 'Mobile')),

-- PC only games
((SELECT GameID FROM @GameIds WHERE Title = 'Counter-Strike 2'), (SELECT PlatformID FROM Platforms WHERE Name = 'PC')),

-- Nintendo exclusive
((SELECT GameID FROM @GameIds WHERE Title = 'Super Mario Odyssey'), (SELECT PlatformID FROM Platforms WHERE Name = 'Nintendo Switch')),

-- Multi-platform recent games
((SELECT GameID FROM @GameIds WHERE Title = 'Elden Ring'), (SELECT PlatformID FROM Platforms WHERE Name = 'PC')),
((SELECT GameID FROM @GameIds WHERE Title = 'Elden Ring'), (SELECT PlatformID FROM Platforms WHERE Name = 'PlayStation 5')),
((SELECT GameID FROM @GameIds WHERE Title = 'Elden Ring'), (SELECT PlatformID FROM Platforms WHERE Name = 'Xbox Series X|S')),

((SELECT GameID FROM @GameIds WHERE Title = 'Death Stranding'), (SELECT PlatformID FROM Platforms WHERE Name = 'PC')),
((SELECT GameID FROM @GameIds WHERE Title = 'Death Stranding'), (SELECT PlatformID FROM Platforms WHERE Name = 'PlayStation 5'));

PRINT 'Database schema and sample data created successfully!';
PRINT 'Default users will be created automatically when the application starts.';
PRINT 'Default credentials:';
PRINT '  Admin: username=admin, password=1';
PRINT '  Customer: username=customer, password=1';