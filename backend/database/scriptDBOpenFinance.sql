IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'openfinance_db')
BEGIN
    CREATE DATABASE openfinance_db
    COLLATE Modern_Spanish_CS_AS; 
END
GO

USE openfinance_db;
GO

-- -----------------------------------------------------
-- Tabla: open_finance_connections
-- Almacena las conexiones (y tokens) a otras instituciones
-- -----------------------------------------------------
CREATE TABLE open_finance_connections (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER NOT NULL, 
    
    provider_name VARCHAR(100) NOT NULL, -- Ej. "BBVA", "Citibanamex", "Fintech X"
    scopes VARCHAR(MAX) NOT NULL, -- JSON o lista de permisos (Ej. "accounts_read, transactions_read")
    status VARCHAR(50) NOT NULL DEFAULT 'ACTIVE', -- (ACTIVE, REVOKED, EXPIRED)
    
    -- Este token debe estar encriptado en la BD
    -- Para el hackathon, lo guardamos en texto plano, pero en producción
    -- se usaría 'Always Encrypted' de SQL Server o encriptación a nivel de app.
    auth_token NVARCHAR(MAX) NOT NULL, 
    
    last_sync DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_open_finance_connections PRIMARY KEY (id)
);
GO

CREATE INDEX IX_connections_user_id ON open_finance_connections (user_id);
GO

-- -----------------------------------------------------
-- Tabla: external_products
-- Almacena los productos (tarjetas, préstamos) que se obtienen de las conexiones
-- -----------------------------------------------------
CREATE TABLE external_products (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    
    connection_id UNIQUEIDENTIFIER NOT NULL,
    
    provider VARCHAR(100) NOT NULL, 
    product_type VARCHAR(100) NOT NULL, -- Ej. "CREDIT_CARD", "LOAN", "SAVINGS_ACCOUNT"
    name VARCHAR(200) NOT NULL, -- Ej. "Tarjeta Oro", "Préstamo Personal"
    balance DECIMAL(19, 4) NOT NULL,
    currency VARCHAR(3) NOT NULL,
    
    next_payment_amount DECIMAL(19, 4) NULL,
    next_payment_date DATE NULL,
    
    last_sync DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_external_products PRIMARY KEY (id),

    CONSTRAINT FK_external_products_connections
        FOREIGN KEY (connection_id)
        REFERENCES open_finance_connections(id)
        ON DELETE CASCADE 
);
GO

CREATE INDEX IX_products_connection_id ON external_products (connection_id);
GO