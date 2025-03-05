-- Drop the existing database if it exists

-- DROP DATABASE AssetRepo;
GO

-- Create a new database
-- CREATE DATABASE AssetRepo;
GO

-- Use the new database
USE AssetRepo;

-- 1. Assets Table
CREATE TABLE Assets (
    AssetID INT PRIMARY KEY IDENTITY(1,1),
    AssetName NVARCHAR(255),  
    [Description] NVARCHAR(MAX),  
    CreatedDate DATETIME
);

-- 2. Projects Table
CREATE TABLE Projects (
    ProjectID INT PRIMARY KEY IDENTITY(1,1),
    ProjectName NVARCHAR(255),  
    [Description] NVARCHAR(MAX),  
    CreatedDate DATETIME,
    AssetID INT, -- Foreign key referencing the Assets table
    FOREIGN KEY (AssetID) REFERENCES Assets(AssetID) -- Establish one-to-many relationship
);

-- 3. Folders Table
CREATE TABLE Folders (
    FolderID BIGINT PRIMARY KEY IDENTITY(1,1),
    FolderName NVARCHAR(255), 
    ParentFolderID INT NULL, -- Parent folder ID (if any)
    AssetID INT, -- Foreign key referencing the Assets table
    CreatedDate DATETIME,
    FOREIGN KEY (AssetID) REFERENCES Assets(AssetID)
);

-- 4. Files Table (with added AssetID column and OriginalFileName column)
CREATE TABLE Files (
    FileID INT PRIMARY KEY IDENTITY(1,1),
    FolderID BIGINT NULL, -- Foreign key referencing the Folders table, can be NULL if file is directly in Asset
    AssetID INT, -- Foreign key referencing the Assets table
    [FileName] NVARCHAR(255),  
    OriginalFileName NVARCHAR(255),  -- New column to store the original file name
    TypeFile NVARCHAR(50),  
    FilePath NVARCHAR(MAX),
    CreatedDate DATETIME,
    FOREIGN KEY (FolderID) REFERENCES Folders(FolderID),
    FOREIGN KEY (AssetID) REFERENCES Assets(AssetID) -- Establishes direct link to Asset
);

-- 5. AssetSold Table
CREATE TABLE AssetSold (
    AssetSoldID INT PRIMARY KEY IDENTITY(1,1),
    AssetID INT UNIQUE,
    SalePrice DECIMAL(18,2),
    FOREIGN KEY (AssetID) REFERENCES Assets(AssetID)
);

-- Insert sample data into Assets (20 total assets)
INSERT INTO Assets (AssetName, [Description], CreatedDate)
VALUES 
(N'Asset1', N'asset for fighter', '2024-10-23'),
(N'Asset2', N'asset for environment', '2024-10-24'),
(N'Asset3', N'asset for graphics', '2024-10-25'),
(N'Asset4', N'asset for sound', '2024-10-26'),
(N'Asset5', N'asset for dramatic', '2024-10-27'),
(N'Asset6', N'asset for user interface', '2024-10-28'),
(N'Asset7', N'asset for animation', '2024-10-29'),
(N'Asset8', N'asset for sound effects', '2024-10-30'),
(N'Asset9', N'asset for lighting', '2024-10-31'),
(N'Asset10', N'asset for AI scripts', '2024-11-01'),
(N'Asset11', N'asset for textures', '2024-11-02'),
(N'Asset12', N'asset for particles', '2024-11-03'),
(N'Asset13', N'asset for UI design', '2024-11-04'),
(N'Asset14', N'asset for NPCs', '2024-11-05'),
(N'Asset15', N'asset for background music', '2024-11-06'),
(N'Asset16', N'asset for dialogue audio', '2024-11-07'),
(N'Asset17', N'asset for visual effects', '2024-11-08'),
(N'Asset18', N'asset for game levels', '2024-11-09'),
(N'Asset19', N'asset for character skins', '2024-11-10'),
(N'Asset20', N'asset for weapon models', '2024-11-11');

-- Insert sample data into Projects, linking each project to a specific Asset (7 projects, assets that are not in AssetSold)
INSERT INTO Projects (ProjectName, [Description], CreatedDate, AssetID)
VALUES 
(N'PUBG', N'Shooting Game', '2024-10-23', 1),
(N'DragonBall', N'Action Game', '2024-10-24', 3),
(N'CupHead', N'Adventure Game', '2024-10-25', 5),
(N'Dan For Man', N'Action Game Mobile', '2024-10-26', 7),
(N'Fortnite', N'Battle Royale Game', '2024-10-27', 9),
(N'Minecraft', N'Sandbox Game', '2024-10-28', 11),
(N'Tetris', N'Puzzle Game', '2024-10-29', 13);

-- Insert sample data into AssetSold (7 assets, distinct from those in Projects)
INSERT INTO AssetSold (AssetID, SalePrice)
VALUES 
(2, 600000.00),
(4, 900000.00),
(6, 1200000.00),
(8, 400000.00),
(10, 700000.00),
(12, 1100000.00),
(14, 500000.00);

-- Insert sample data into Folders
INSERT INTO Folders (FolderName, ParentFolderID, AssetID, CreatedDate)
VALUES 
(N'UI Elements', NULL, 6, '2024-10-28'),
(N'Animations', NULL, 7, '2024-10-29'),
(N'Sound FX', NULL, 8, '2024-10-30'),
(N'Lighting Effects', NULL, 9, '2024-10-31'),
(N'AI Scripts', NULL, 10, '2024-11-01'),
(N'Textures', NULL, 11, '2024-11-02'),
(N'Particles', NULL, 12, '2024-11-03');

-- Insert sample data into Files (with direct Asset links for files without FolderID and with OriginalFileName)
INSERT INTO Files (FolderID, AssetID, [FileName], OriginalFileName, TypeFile, FilePath, CreatedDate)
VALUES 
(1, 6, N'ButtonUI.png', N'ButtonUI_Original.png', N'Image', '/path/to/button_ui.png', '2024-10-28'),
(2, 7, N'CharacterAnimation.anim', N'CharacterAnimation_Original.anim', N'Animation', '/path/to/character_animation.anim', '2024-10-29'),
(3, 8, N'ExplosionSound.wav', N'ExplosionSound_Original.wav', N'Audio', '/path/to/explosion_sound.wav', '2024-10-30'),
(4, 9, N'LightSettings.json', N'LightSettings_Original.json', N'Config', '/path/to/light_settings.json', '2024-10-31'),
(5, 10, N'EnemyAIScript.cs', N'EnemyAIScript_Original.cs', N'Script', '/path/to/enemy_ai_script.cs', '2024-11-01'),
(6, 11, N'BrickTexture.png', N'BrickTexture_Original.png', N'Image', '/path/to/brick_texture.png', '2024-11-02'),
(7, 12, N'SmokeParticleEffect.prefab', N'SmokeParticleEffect_Original.prefab', N'Prefab', '/path/to/smoke_particle_effect.prefab', '2024-11-03'),
(NULL, 7, N'BackgroundMusic.mp3', N'BackgroundMusic_Original.mp3', N'Audio', '/path/to/background_music.mp3', '2024-11-08'); -- Direct file upload to Asset without Folder
