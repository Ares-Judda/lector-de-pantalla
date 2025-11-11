IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'auth_db')
BEGIN
    CREATE DATABASE auth_db
    COLLATE Modern_Spanish_CS_AS;
END
GO

USE auth_db;
GO

-- -----------------------------------------------------
-- Tabla: users
-- Almacena la identidad principal y credenciales.
-- -----------------------------------------------------
CREATE TABLE users (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    alias VARCHAR(100) NOT NULL,
    preferred_language VARCHAR(10) NOT NULL DEFAULT 'es-MX',
    demo_mode BIT NOT NULL DEFAULT 0,
    created_at DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    updated_at DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    hashed_password VARCHAR(255) NOT NULL,
    phone_number VARCHAR(20) NULL,
    email VARCHAR(255) NULL,
    
    CONSTRAINT PK_users PRIMARY KEY (id),
    
    CONSTRAINT UQ_users_alias UNIQUE (alias),
    
    CONSTRAINT UQ_users_phone_number UNIQUE (phone_number),
    CONSTRAINT UQ_users_email UNIQUE (email)
);
GO

-- -----------------------------------------------------
-- Tabla: accessibility_profiles
-- Almacena las preferencias de accesibilidad del usuario
-- -----------------------------------------------------
CREATE TABLE accessibility_profiles (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER NOT NULL,
    theme VARCHAR(50) NOT NULL DEFAULT 'light',
    screen_reader_mode BIT NOT NULL DEFAULT 0,
    font_scale DECIMAL(3, 2) NOT NULL DEFAULT 1.0,
    nudging_level VARCHAR(50) NOT NULL DEFAULT 'medium',
    color_contrast_ratio DECIMAL(4, 2) NOT NULL DEFAULT 4.5,
    voice_feedback BIT NOT NULL DEFAULT 0,
    updated_at DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_accessibility_profiles PRIMARY KEY (id),

    CONSTRAINT FK_accessibility_profiles_users 
        FOREIGN KEY (user_id) 
        REFERENCES users(id)
        ON DELETE CASCADE, 

    CONSTRAINT UQ_accessibility_profiles_user_id UNIQUE (user_id)
);
GO

-- -----------------------------------------------------
-- Tabla: consent_records
-- Almacena los consentimientos del usuario
-- -----------------------------------------------------
CREATE TABLE consent_records (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER NOT NULL,
    type VARCHAR(100) NOT NULL, -- Ej. 'OPEN_FINANCE_READ', 'MARKETING'
    granted BIT NOT NULL DEFAULT 0,
    timestamp DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    revoked_at DATETIME2 NULL, -- Es NULLEABLE porque puede no estar revocado

    CONSTRAINT PK_consent_records PRIMARY KEY (id),

    CONSTRAINT FK_consent_records_users 
        FOREIGN KEY (user_id) 
        REFERENCES users(id)
        ON DELETE CASCADE 
);
GO