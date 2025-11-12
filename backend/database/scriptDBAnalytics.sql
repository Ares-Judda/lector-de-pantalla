IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'analytics_db')
BEGIN
    CREATE DATABASE analytics_db
    COLLATE Modern_Spanish_CS_AS; 
END
GO

USE analytics_db;
GO

-- -----------------------------------------------------
-- Tabla: usage_events
-- Almacena la telemetría: qué hace el usuario y dónde.
-- -----------------------------------------------------
CREATE TABLE usage_events (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER NOT NULL, 
    
    screen VARCHAR(100) NOT NULL, -- Ej. "TRANSACTION_AMOUNT_SCREEN"
    event_type VARCHAR(100) NOT NULL, -- Ej. "CLICK", "PAGE_VIEW", "ERROR", "HELP_REQUEST"
    timestamp DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    details NVARCHAR(MAX) NULL, 

    CONSTRAINT PK_usage_events PRIMARY KEY (id)
);
GO

CREATE INDEX IX_usage_events_user_timestamp ON usage_events (user_id, timestamp DESC);
GO


-- -----------------------------------------------------
-- Tabla: nudges
-- Almacena un registro de las "ayudas proactivas" (nudges) que se le han mostrado al usuario
-- -----------------------------------------------------
CREATE TABLE nudges (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER NOT NULL,
    
    screen VARCHAR(100) NOT NULL, -- Dónde se mostró
    nudge_type VARCHAR(100) NOT NULL, -- Ej. "HELP_OFFER", "SECURITY_WARNING"
    message NVARCHAR(500) NOT NULL, -- El texto que se le mostró
    trigger_reason VARCHAR(255) NULL, -- Por qué se mostró (Ej. "USER_IDLE_15_SECONDS")
    
    created_at DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    accepted BIT NOT NULL DEFAULT 0, 

    CONSTRAINT PK_nudges PRIMARY KEY (id)
);
GO

CREATE INDEX IX_nudges_user_id ON nudges (user_id);
GO