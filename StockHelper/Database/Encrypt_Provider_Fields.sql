-- ============================================================================
-- StockHelper - Encryption support for sensitive Provider fields (REQ.002 / CU.Arq.003-005)
-- ============================================================================
-- The columns CUIT, ContactTel and Email of PROVIDERS are now stored ENCRYPTED at rest
-- (format: "enc:v1:<base64>:<base64 hmac>"). This script prepares the schema.
--
-- RUN THIS ONCE, BEFORE starting the application with the encryption change, on core_db.
-- Existing plaintext rows keep working (they are read as-is) and become encrypted the next
-- time each provider is saved.
-- ============================================================================

USE core_db;
GO

-- 1) Drop the UNIQUE constraint on CUIT.
--    Encrypted CUITs use a random IV, so two identical CUITs produce different ciphertext:
--    a DB-level UNIQUE constraint can no longer enforce plaintext uniqueness (and the
--    application does not rely on it).
DECLARE @constraintName SYSNAME;

SELECT @constraintName = kc.name
FROM sys.key_constraints kc
JOIN sys.index_columns ic
    ON kc.parent_object_id = ic.object_id AND kc.unique_index_id = ic.index_id
JOIN sys.columns c
    ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE kc.parent_object_id = OBJECT_ID('dbo.PROVIDERS')
  AND kc.type = 'UQ'
  AND c.name = 'CUIT';

IF @constraintName IS NOT NULL
BEGIN
    EXEC('ALTER TABLE dbo.PROVIDERS DROP CONSTRAINT ' + @constraintName);
    PRINT 'Dropped UNIQUE constraint on PROVIDERS.CUIT: ' + @constraintName;
END
GO

-- 2) Widen the encrypted columns to hold the Base64 ciphertext + HMAC.
ALTER TABLE dbo.PROVIDERS ALTER COLUMN CUIT       NVARCHAR(512) NOT NULL;
ALTER TABLE dbo.PROVIDERS ALTER COLUMN ContactTel NVARCHAR(512) NULL;
ALTER TABLE dbo.PROVIDERS ALTER COLUMN Email      NVARCHAR(512) NULL;
GO

PRINT 'PROVIDERS sensitive columns widened for encrypted storage.';
GO
