IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ledger_db')
BEGIN
    CREATE DATABASE ledger_db
    COLLATE Modern_Spanish_CS_AS;
END
GO

USE ledger_db;
GO

-- -----------------------------------------------------
-- Tabla: accounts
-- Almacena las cuentas bancarias internas del usuario (sus saldos)
-- -----------------------------------------------------
CREATE TABLE accounts (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER NOT NULL, 
    account_number VARCHAR(50) NOT NULL,
    account_type VARCHAR(50) NOT NULL DEFAULT 'DEFAULT',
    balance DECIMAL(19, 4) NOT NULL DEFAULT 0.00,
    currency VARCHAR(3) NOT NULL DEFAULT 'MXN',
    last_updated DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_accounts PRIMARY KEY (id),
    CONSTRAINT UQ_accounts_account_number UNIQUE (account_number)
);
GO

-- -----------------------------------------------------
-- Tabla: beneficiaries
-- Almacena los "contactos de confianza" a los que el usuario puede enviar dinero
-- -----------------------------------------------------
CREATE TABLE beneficiaries (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    user_id UNIQUEIDENTIFIER NOT NULL, 
    name VARCHAR(200) NOT NULL,
    alias VARCHAR(100) NULL,
    account_number VARCHAR(50) NOT NULL,
    bank_name VARCHAR(100) NOT NULL,
    is_favorite BIT NOT NULL DEFAULT 0,
    created_at DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_beneficiaries PRIMARY KEY (id),
    
    CONSTRAINT UQ_beneficiaries_user_account UNIQUE (user_id, account_number)
);
GO

-- -----------------------------------------------------
-- Tabla: transactions
-- El historial de todos los movimientos (envíos SPEI)
-- -----------------------------------------------------
CREATE TABLE transactions (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    
    from_account_id UNIQUEIDENTIFIER NOT NULL, 
    to_beneficiary_id UNIQUEIDENTIFIER NOT NULL, 
    -- ---------------------------------
    
    amount DECIMAL(19, 4) NOT NULL,
    currency VARCHAR(3) NOT NULL DEFAULT 'MXN',
    status VARCHAR(50) NOT NULL DEFAULT 'PENDING', -- (PENDING, COMPLETED, FAILED)
    risk_flags VARCHAR(MAX) NULL, -- Almacena un JSON o lista de flags
    spei_reference VARCHAR(100) NULL,
    created_at DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT PK_transactions PRIMARY KEY (id),

    CONSTRAINT FK_transactions_accounts
        FOREIGN KEY (from_account_id)
        REFERENCES accounts(id)
        ON DELETE NO ACTION, 

    CONSTRAINT FK_transactions_beneficiaries
        FOREIGN KEY (to_beneficiary_id)
        REFERENCES beneficiaries(id)
        ON DELETE NO ACTION 
);
GO

-- -----------------------------------------------------
-- Tabla: security_alerts
-- Alertas generadas por transacciones sospechosas
-- -----------------------------------------------------
CREATE TABLE security_alerts (
    id UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    
    transaction_id UNIQUEIDENTIFIER NOT NULL,
    
    user_id UNIQUEIDENTIFIER NOT NULL, 
    
    flag VARCHAR(100) NOT NULL, -- (Ej. "HIGH_AMOUNT", "NEW_BENEFICIARY")
    message NVARCHAR(500) NOT NULL,
    created_at DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    acknowledged BIT NOT NULL DEFAULT 0,

    CONSTRAINT PK_security_alerts PRIMARY KEY (id),

    CONSTRAINT FK_security_alerts_transactions
        FOREIGN KEY (transaction_id)
        REFERENCES transactions(id)
        ON DELETE CASCADE
);
GO