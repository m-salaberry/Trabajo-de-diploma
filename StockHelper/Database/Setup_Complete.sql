-- ==============================================================================
-- SCRIPT COMPLETO DE CONFIGURACION DE BASE DE DATOS - STOCKHELPER
-- Ejecutar en SQL Server (SQLCMD o SSMS)
-- Este script crea ambas bases de datos desde cero:
--   1. iam_db  - Autenticacion, usuarios, roles y permisos
--   2. core_db - Datos de negocio (inventario, productos, ordenes, proveedores)
-- ==============================================================================

-- ==============================================================================
-- PASO 1: CREAR LOGIN DE SQL SERVER Y USUARIO
-- ==============================================================================

USE master;
GO

-- Crear login a nivel de servidor (si no existe)
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = 'stock_helper_user')
BEGIN
    CREATE LOGIN stock_helper_user WITH PASSWORD = N's7bOGv66G''*c', CHECK_POLICY = OFF;
    PRINT 'Login "stock_helper_user" creado exitosamente.';
END
ELSE
    PRINT 'Login "stock_helper_user" ya existe.';
GO

-- ==============================================================================
-- PASO 2: CREAR BASE DE DATOS iam_db
-- ==============================================================================

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'iam_db')
BEGIN
    CREATE DATABASE iam_db;
    PRINT 'Base de datos "iam_db" creada.';
END
ELSE
    PRINT 'Base de datos "iam_db" ya existe.';
GO

USE iam_db;
GO

-- Crear usuario en iam_db y asignar permisos
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'stock_helper_user')
BEGIN
    CREATE USER stock_helper_user FOR LOGIN stock_helper_user;
    ALTER ROLE db_datareader ADD MEMBER stock_helper_user;
    ALTER ROLE db_datawriter ADD MEMBER stock_helper_user;
    PRINT 'Usuario "stock_helper_user" creado en iam_db con permisos de lectura/escritura.';
END
GO

-- ==============================================================================
-- PASO 3: CREAR TABLAS DE iam_db
-- ==============================================================================

-- Tabla USERS: Usuarios del sistema
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'USERS')
BEGIN
    CREATE TABLE USERS (
        Id          UNIQUEIDENTIFIER    NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Name        NVARCHAR(50)        NOT NULL UNIQUE,
        Password    NVARCHAR(255)       NOT NULL,
        Role        NVARCHAR(50)        NULL,
        IsActive    BIT                 NOT NULL DEFAULT 1,
        Email       NVARCHAR(255)       NULL,
        CreatedDate DATETIME            NOT NULL DEFAULT GETDATE(),
        ModifiedDate DATETIME           NULL,
        LastLoginDate DATETIME          NULL
    );

    CREATE NONCLUSTERED INDEX IX_USERS_Name ON USERS (Name);
    CREATE NONCLUSTERED INDEX IX_USERS_IsActive ON USERS (IsActive);

    PRINT 'Tabla USERS creada.';
END
GO

-- Tabla PATENTS: Permisos atomicos (hojas del patron Composite)
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PATENTS')
BEGIN
    CREATE TABLE PATENTS (
        Id          UNIQUEIDENTIFIER    NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Name        NVARCHAR(100)       NOT NULL UNIQUE,
        Description NVARCHAR(500)       NULL,
        CreatedDate DATETIME            NOT NULL DEFAULT GETDATE()
    );

    PRINT 'Tabla PATENTS creada.';
END
GO

-- Tabla FAMILIES: Roles (agrupaciones de permisos)
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'FAMILIES')
BEGIN
    CREATE TABLE FAMILIES (
        Id          UNIQUEIDENTIFIER    NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Name        NVARCHAR(100)       NOT NULL UNIQUE,
        Description NVARCHAR(500)       NULL,
        CreatedDate DATETIME            NOT NULL DEFAULT GETDATE(),
        ModifiedDate DATETIME           NULL
    );

    PRINT 'Tabla FAMILIES creada.';
END
GO

-- Tabla PATENTS_FAMILIES: Relacion N:M entre permisos y roles
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PATENTS_FAMILIES')
BEGIN
    CREATE TABLE PATENTS_FAMILIES (
        PatentId    UNIQUEIDENTIFIER    NOT NULL,
        FamilyId    UNIQUEIDENTIFIER    NOT NULL,
        AssignedDate DATETIME           NOT NULL DEFAULT GETDATE(),

        CONSTRAINT PK_PATENTS_FAMILIES PRIMARY KEY (PatentId, FamilyId),
        CONSTRAINT FK_PF_Patent FOREIGN KEY (PatentId) REFERENCES PATENTS(Id) ON DELETE CASCADE,
        CONSTRAINT FK_PF_Family FOREIGN KEY (FamilyId) REFERENCES FAMILIES(Id) ON DELETE CASCADE
    );

    CREATE NONCLUSTERED INDEX IX_PATENTS_FAMILIES_PatentId ON PATENTS_FAMILIES (PatentId);
    CREATE NONCLUSTERED INDEX IX_PATENTS_FAMILIES_FamilyId ON PATENTS_FAMILIES (FamilyId);

    PRINT 'Tabla PATENTS_FAMILIES creada.';
END
GO

-- Tabla USERS_FAMILIES: Relacion N:M entre usuarios y roles
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'USERS_FAMILIES')
BEGIN
    CREATE TABLE USERS_FAMILIES (
        UserId      UNIQUEIDENTIFIER    NOT NULL,
        FamilyId    UNIQUEIDENTIFIER    NOT NULL,
        AssignedDate DATETIME           NOT NULL DEFAULT GETDATE(),
        AssignedBy  UNIQUEIDENTIFIER    NULL,

        CONSTRAINT PK_USERS_FAMILIES PRIMARY KEY (UserId, FamilyId),
        CONSTRAINT FK_UF_User FOREIGN KEY (UserId) REFERENCES USERS(Id) ON DELETE CASCADE,
        CONSTRAINT FK_UF_Family FOREIGN KEY (FamilyId) REFERENCES FAMILIES(Id) ON DELETE CASCADE
    );

    CREATE NONCLUSTERED INDEX IX_USERS_FAMILIES_UserId ON USERS_FAMILIES (UserId);
    CREATE NONCLUSTERED INDEX IX_USERS_FAMILIES_FamilyId ON USERS_FAMILIES (FamilyId);

    PRINT 'Tabla USERS_FAMILIES creada.';
END
GO

-- ==============================================================================
-- PASO 4: CREAR BASE DE DATOS core_db
-- ==============================================================================

USE master;
GO

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'core_db')
BEGIN
    CREATE DATABASE core_db;
    PRINT 'Base de datos "core_db" creada.';
END
ELSE
    PRINT 'Base de datos "core_db" ya existe.';
GO

USE core_db;
GO

-- Crear usuario en core_db y asignar permisos
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'stock_helper_user')
BEGIN
    CREATE USER stock_helper_user FOR LOGIN stock_helper_user;
    ALTER ROLE db_datareader ADD MEMBER stock_helper_user;
    ALTER ROLE db_datawriter ADD MEMBER stock_helper_user;
    PRINT 'Usuario "stock_helper_user" creado en core_db con permisos de lectura/escritura.';
END
GO

-- ==============================================================================
-- PASO 5: CREAR TABLAS DE core_db
-- ==============================================================================

-- Tabla ITEMS_CATEGORY: Categorias de insumos
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ITEMS_CATEGORY')
BEGIN
    CREATE TABLE ITEMS_CATEGORY (
        Id      INT             NOT NULL PRIMARY KEY IDENTITY(1,1),
        Name    NVARCHAR(100)   NOT NULL
    );

    PRINT 'Tabla ITEMS_CATEGORY creada.';
END
GO

-- Tabla ITEMS: Insumos del inventario
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ITEMS')
BEGIN
    CREATE TABLE ITEMS (
        Id              UNIQUEIDENTIFIER    NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Name            NVARCHAR(255)       NOT NULL,
        Unit            NVARCHAR(50)        NULL,
        IntegerUnit     BIT                 NOT NULL DEFAULT 0,
        ItemsCategoryId INT                 NOT NULL,
        CurrentStock    DECIMAL(18, 4)      NOT NULL DEFAULT 0,
        LastUpdate      DATETIME            NULL DEFAULT GETDATE(),
        CreatedDate     DATETIME            NOT NULL DEFAULT GETDATE(),
        ModifiedDate    DATETIME            NULL,

        CONSTRAINT FK_Items_Category FOREIGN KEY (ItemsCategoryId) REFERENCES ITEMS_CATEGORY(Id)
    );

    PRINT 'Tabla ITEMS creada.';
END
GO

-- Tabla PROVIDERS: Proveedores
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PROVIDERS')
BEGIN
    CREATE TABLE PROVIDERS (
        Id              UNIQUEIDENTIFIER    NOT NULL PRIMARY KEY DEFAULT NEWID(),
        Name            NVARCHAR(255)       NOT NULL,
        CUIT            NVARCHAR(20)        NOT NULL UNIQUE,
        CompanyName     NVARCHAR(255)       NULL,
        ContactTel      NVARCHAR(50)        NULL,
        Email           NVARCHAR(255)       NULL,
        ItemsCategoryId INT                 NOT NULL,
        CreatedDate     DATETIME            NOT NULL DEFAULT GETDATE(),
        ModifiedDate    DATETIME            NULL,

        CONSTRAINT FK_Providers_Category FOREIGN KEY (ItemsCategoryId) REFERENCES ITEMS_CATEGORY(Id)
    );

    PRINT 'Tabla PROVIDERS creada.';
END
GO

-- Tabla PRODUCTS: Productos terminados
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PRODUCTS')
BEGIN
    CREATE TABLE PRODUCTS (
        Id              INT             NOT NULL PRIMARY KEY IDENTITY(1,1),
        Code            INT             NOT NULL,
        Name            NVARCHAR(255)   NOT NULL,
        CreatedDate     DATETIME        NOT NULL DEFAULT GETDATE(),
        ModifiedDate    DATETIME        NULL
    );

    PRINT 'Tabla PRODUCTS creada.';
END
GO

-- Tabla PRODUCT_DETAILS: Detalle de insumos por producto (Bill of Materials)
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PRODUCT_DETAILS')
BEGIN
    CREATE TABLE PRODUCT_DETAILS (
        Id                  INT                 NOT NULL PRIMARY KEY IDENTITY(1,1),
        ProductId           INT                 NOT NULL,
        ItemId              UNIQUEIDENTIFIER    NOT NULL,
        QuantityToConsume   DECIMAL(18, 4)      NOT NULL,

        CONSTRAINT FK_PD_Product FOREIGN KEY (ProductId) REFERENCES PRODUCTS(Id) ON DELETE CASCADE,
        CONSTRAINT FK_PD_Item FOREIGN KEY (ItemId) REFERENCES ITEMS(Id)
    );

    PRINT 'Tabla PRODUCT_DETAILS creada.';
END
GO

-- Tabla REPLACEMENT_ORDERS: Ordenes de reposicion
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'REPLACEMENT_ORDERS')
BEGIN
    CREATE TABLE REPLACEMENT_ORDERS (
        Id                      UNIQUEIDENTIFIER    NOT NULL PRIMARY KEY DEFAULT NEWID(),
        ReplacementOrderNumber  NVARCHAR(50)        NOT NULL,
        ProviderId              UNIQUEIDENTIFIER    NOT NULL,
        CreatedDate             DATETIME            NOT NULL DEFAULT GETDATE(),
        ModifiedDate            DATETIME            NULL,

        CONSTRAINT FK_RO_Provider FOREIGN KEY (ProviderId) REFERENCES PROVIDERS(Id)
    );

    PRINT 'Tabla REPLACEMENT_ORDERS creada.';
END
GO

-- Tabla ORDER_ROWS: Filas/lineas de una orden de reposicion
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ORDER_ROWS')
BEGIN
    CREATE TABLE ORDER_ROWS (
        Id                  UNIQUEIDENTIFIER    NOT NULL PRIMARY KEY DEFAULT NEWID(),
        ReplacementOrderId  UNIQUEIDENTIFIER    NOT NULL,
        ItemId              UNIQUEIDENTIFIER    NOT NULL,
        Quantity            DECIMAL(18, 4)      NOT NULL,

        CONSTRAINT FK_OR_ReplacementOrder FOREIGN KEY (ReplacementOrderId) REFERENCES REPLACEMENT_ORDERS(Id) ON DELETE CASCADE,
        CONSTRAINT FK_OR_Item FOREIGN KEY (ItemId) REFERENCES ITEMS(Id)
    );

    PRINT 'Tabla ORDER_ROWS creada.';
END
GO

-- Tabla PURCHASE_ORDERS: Ordenes de compra
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PURCHASE_ORDERS')
BEGIN
    CREATE TABLE PURCHASE_ORDERS (
        Id                  UNIQUEIDENTIFIER    NOT NULL PRIMARY KEY DEFAULT NEWID(),
        ReplacementOrderId  UNIQUEIDENTIFIER    NOT NULL,
        Status              NVARCHAR(50)        NOT NULL,
        BillFilePath        NVARCHAR(500)       NULL,
        TotalAmount         DECIMAL(18, 2)      NOT NULL DEFAULT 0,
        IssuedDate          DATETIME            NOT NULL,
        CreatedDate         DATETIME            NOT NULL DEFAULT GETDATE(),
        ModifiedDate        DATETIME            NULL,

        CONSTRAINT FK_PO_ReplacementOrder FOREIGN KEY (ReplacementOrderId) REFERENCES REPLACEMENT_ORDERS(Id)
    );

    PRINT 'Tabla PURCHASE_ORDERS creada.';
END
GO

-- ==============================================================================
-- PASO 6: INSERTAR PERMISOS ATOMICOS (PATENTS) EN iam_db
-- ==============================================================================

USE iam_db;
GO

PRINT '';
PRINT '========================================';
PRINT 'INSERTANDO PERMISOS Y ROLES';
PRINT '========================================';

-- Permisos de gestion
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'UserManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'UserManagement', 'Acceso completo al modulo de gestion de usuarios');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'PermissionManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'PermissionManagement', 'Acceso completo al modulo de permisos y roles');

-- Permisos de inventario/stock
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'InventoryManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'InventoryManagement', 'Acceso completo al modulo de gestion de inventario');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'ProductCatalog')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'ProductCatalog', 'Acceso al catalogo de productos');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'StockControl')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'StockControl', 'Acceso al control de stock y ajustes de inventario');

-- Permisos de ventas y compras
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'SalesManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'SalesManagement', 'Acceso completo al modulo de gestion de ventas');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'PurchaseManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'PurchaseManagement', 'Acceso completo al modulo de gestion de compras');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'PointOfSale')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'PointOfSale', 'Acceso al punto de venta (POS)');

-- Permisos de reportes y analiticas
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'Reports')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'Reports', 'Acceso al modulo de reportes generales');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'SalesReports')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'SalesReports', 'Acceso a reportes de ventas');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'InventoryReports')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'InventoryReports', 'Acceso a reportes de inventario');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'FinancialReports')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'FinancialReports', 'Acceso a reportes financieros');

-- Permisos de clientes y proveedores
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'CustomerManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'CustomerManagement', 'Acceso completo al modulo de gestion de clientes');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'SupplierManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'SupplierManagement', 'Acceso completo al modulo de gestion de proveedores');

-- Permisos de configuracion del sistema
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'SystemConfiguration')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'SystemConfiguration', 'Acceso a la configuracion del sistema');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'SystemLogs')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'SystemLogs', 'Acceso a los logs del sistema');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'DatabaseBackup')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'DatabaseBackup', 'Acceso al modulo de backup y restore de base de datos');

-- Permisos adicionales
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'Dashboard')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'Dashboard', 'Acceso al panel principal con informacion resumida');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'PricingManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'PricingManagement', 'Acceso al modulo de gestion de precios y descuentos');
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'WarehouseManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES (NEWID(), 'WarehouseManagement', 'Acceso al modulo de gestion de almacenes');

PRINT 'Permisos insertados: 20';
GO

-- ==============================================================================
-- PASO 7: CREAR ROLES (FAMILIES)
-- ==============================================================================

DECLARE @AdminId UNIQUEIDENTIFIER;
DECLARE @ManagerId UNIQUEIDENTIFIER;
DECLARE @SalespersonId UNIQUEIDENTIFIER;
DECLARE @InventoryManagerId UNIQUEIDENTIFIER;
DECLARE @CashierId UNIQUEIDENTIFIER;
DECLARE @AuditorId UNIQUEIDENTIFIER;
DECLARE @WarehouseOperatorId UNIQUEIDENTIFIER;
DECLARE @AccountantId UNIQUEIDENTIFIER;
DECLARE @GuestId UNIQUEIDENTIFIER;

-- Administrator
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Administrator')
BEGIN
    SET @AdminId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description) VALUES (@AdminId, 'Administrator', 'Acceso total al sistema');
END
ELSE
    SET @AdminId = (SELECT Id FROM FAMILIES WHERE Name = 'Administrator');

-- Manager
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Manager')
BEGIN
    SET @ManagerId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description) VALUES (@ManagerId, 'Manager', 'Acceso a gestion operativa y reportes');
END
ELSE
    SET @ManagerId = (SELECT Id FROM FAMILIES WHERE Name = 'Manager');

-- Salesperson
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Salesperson')
BEGIN
    SET @SalespersonId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description) VALUES (@SalespersonId, 'Salesperson', 'Acceso a ventas y gestion de clientes');
END
ELSE
    SET @SalespersonId = (SELECT Id FROM FAMILIES WHERE Name = 'Salesperson');

-- InventoryManager
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'InventoryManager')
BEGIN
    SET @InventoryManagerId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description) VALUES (@InventoryManagerId, 'InventoryManager', 'Acceso completo a gestion de inventario');
END
ELSE
    SET @InventoryManagerId = (SELECT Id FROM FAMILIES WHERE Name = 'InventoryManager');

-- Cashier
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Cashier')
BEGIN
    SET @CashierId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description) VALUES (@CashierId, 'Cashier', 'Acceso al punto de venta');
END
ELSE
    SET @CashierId = (SELECT Id FROM FAMILIES WHERE Name = 'Cashier');

-- Auditor
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Auditor')
BEGIN
    SET @AuditorId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description) VALUES (@AuditorId, 'Auditor', 'Acceso de solo lectura a reportes y logs');
END
ELSE
    SET @AuditorId = (SELECT Id FROM FAMILIES WHERE Name = 'Auditor');

-- WarehouseOperator
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'WarehouseOperator')
BEGIN
    SET @WarehouseOperatorId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description) VALUES (@WarehouseOperatorId, 'WarehouseOperator', 'Acceso a control de almacen y stock');
END
ELSE
    SET @WarehouseOperatorId = (SELECT Id FROM FAMILIES WHERE Name = 'WarehouseOperator');

-- Accountant
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Accountant')
BEGIN
    SET @AccountantId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description) VALUES (@AccountantId, 'Accountant', 'Acceso a reportes financieros y gestion de precios');
END
ELSE
    SET @AccountantId = (SELECT Id FROM FAMILIES WHERE Name = 'Accountant');

-- Guest
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Guest')
BEGIN
    SET @GuestId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description) VALUES (@GuestId, 'Guest', 'Acceso minimo - solo dashboard');
END
ELSE
    SET @GuestId = (SELECT Id FROM FAMILIES WHERE Name = 'Guest');

PRINT 'Roles creados: 9';

-- ==============================================================================
-- PASO 8: ASIGNAR PERMISOS A ROLES
-- ==============================================================================

-- Limpiar asignaciones existentes para recrear
DELETE FROM PATENTS_FAMILIES WHERE FamilyId IN (
    @AdminId, @ManagerId, @SalespersonId, @InventoryManagerId,
    @CashierId, @AuditorId, @WarehouseOperatorId, @AccountantId, @GuestId
);

-- Administrator: todos los permisos (20)
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @AdminId FROM PATENTS;

-- Manager: 14 permisos
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @ManagerId FROM PATENTS
WHERE Name IN (
    'UserManagement', 'InventoryManagement', 'ProductCatalog', 'StockControl',
    'SalesManagement', 'PurchaseManagement', 'Reports', 'SalesReports',
    'InventoryReports', 'FinancialReports', 'CustomerManagement',
    'SupplierManagement', 'Dashboard', 'PricingManagement'
);

-- Salesperson: 6 permisos
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @SalespersonId FROM PATENTS
WHERE Name IN (
    'SalesManagement', 'PointOfSale', 'CustomerManagement',
    'ProductCatalog', 'SalesReports', 'Dashboard'
);

-- InventoryManager: 8 permisos
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @InventoryManagerId FROM PATENTS
WHERE Name IN (
    'InventoryManagement', 'ProductCatalog', 'StockControl', 'WarehouseManagement',
    'InventoryReports', 'SupplierManagement', 'PurchaseManagement', 'Dashboard'
);

-- Cashier: 2 permisos
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @CashierId FROM PATENTS
WHERE Name IN ('PointOfSale', 'Dashboard');

-- Auditor: 6 permisos
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @AuditorId FROM PATENTS
WHERE Name IN (
    'Reports', 'SalesReports', 'InventoryReports',
    'FinancialReports', 'SystemLogs', 'Dashboard'
);

-- WarehouseOperator: 5 permisos
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @WarehouseOperatorId FROM PATENTS
WHERE Name IN (
    'StockControl', 'WarehouseManagement', 'ProductCatalog',
    'InventoryReports', 'Dashboard'
);

-- Accountant: 4 permisos
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @AccountantId FROM PATENTS
WHERE Name IN (
    'FinancialReports', 'PricingManagement', 'Reports', 'Dashboard'
);

-- Guest: 1 permiso
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @GuestId FROM PATENTS
WHERE Name IN ('Dashboard');

PRINT 'Permisos asignados a roles.';

-- ==============================================================================
-- PASO 9: CREAR USUARIO ADMINISTRADOR POR DEFECTO
-- ==============================================================================

-- Password: "admin" hasheada con MD5 (Encoding.Unicode / UTF-16LE)
-- MD5 de "admin" en UTF-16LE = 19a2854144b63a8f7617a6f225019b12
DECLARE @AdminUserId UNIQUEIDENTIFIER = NEWID();

IF NOT EXISTS (SELECT 1 FROM USERS WHERE Name = 'admin')
BEGIN
    INSERT INTO USERS (Id, Name, Password, Role, IsActive)
    VALUES (@AdminUserId, 'admin', '19a2854144b63a8f7617a6f225019b12', 'Administrator', 1);

    -- Asignar rol Administrator al usuario admin
    INSERT INTO USERS_FAMILIES (UserId, FamilyId)
    VALUES (@AdminUserId, @AdminId);

    PRINT 'Usuario "admin" creado con rol Administrator. Password: admin';
END
ELSE
BEGIN
    -- Si el usuario ya existe, asegurar que tenga el rol Administrator
    SET @AdminUserId = (SELECT Id FROM USERS WHERE Name = 'admin');

    IF NOT EXISTS (SELECT 1 FROM USERS_FAMILIES WHERE UserId = @AdminUserId AND FamilyId = @AdminId)
    BEGIN
        INSERT INTO USERS_FAMILIES (UserId, FamilyId) VALUES (@AdminUserId, @AdminId);
    END

    PRINT 'Usuario "admin" ya existe. Rol Administrator verificado.';
END

GO

-- ==============================================================================
-- PASO 10: VERIFICACION FINAL
-- ==============================================================================

USE iam_db;
GO

PRINT '';
PRINT '========================================';
PRINT 'VERIFICACION DE iam_db';
PRINT '========================================';

SELECT 'Permisos totales' AS Concepto, COUNT(*) AS Cantidad FROM PATENTS
UNION ALL
SELECT 'Roles totales', COUNT(*) FROM FAMILIES
UNION ALL
SELECT 'Usuarios totales', COUNT(*) FROM USERS;

PRINT '';
PRINT 'Permisos por rol:';
SELECT
    f.Name AS Rol,
    COUNT(pf.PatentId) AS Permisos
FROM FAMILIES f
LEFT JOIN PATENTS_FAMILIES pf ON f.Id = pf.FamilyId
GROUP BY f.Name
ORDER BY Permisos DESC;

USE core_db;
GO

PRINT '';
PRINT '========================================';
PRINT 'VERIFICACION DE core_db';
PRINT '========================================';

SELECT 'ITEMS_CATEGORY' AS Tabla, COUNT(*) AS Registros FROM ITEMS_CATEGORY
UNION ALL
SELECT 'ITEMS', COUNT(*) FROM ITEMS
UNION ALL
SELECT 'PROVIDERS', COUNT(*) FROM PROVIDERS
UNION ALL
SELECT 'PRODUCTS', COUNT(*) FROM PRODUCTS
UNION ALL
SELECT 'PRODUCT_DETAILS', COUNT(*) FROM PRODUCT_DETAILS
UNION ALL
SELECT 'REPLACEMENT_ORDERS', COUNT(*) FROM REPLACEMENT_ORDERS
UNION ALL
SELECT 'ORDER_ROWS', COUNT(*) FROM ORDER_ROWS
UNION ALL
SELECT 'PURCHASE_ORDERS', COUNT(*) FROM PURCHASE_ORDERS;

PRINT '';
PRINT '========================================';
PRINT 'CONFIGURACION COMPLETADA';
PRINT '========================================';
PRINT '';
PRINT 'Resumen:';
PRINT '  Servidor: MATI\SQLEXPRESS (cambiar segun su instancia)';
PRINT '  Login SQL: stock_helper_user';
PRINT '  Password:  s7bOGv66G''*c';
PRINT '';
PRINT '  Base de datos iam_db:';
PRINT '    - USERS (con usuario "admin" / password "admin")';
PRINT '    - PATENTS (20 permisos)';
PRINT '    - FAMILIES (9 roles)';
PRINT '    - PATENTS_FAMILIES';
PRINT '    - USERS_FAMILIES';
PRINT '';
PRINT '  Base de datos core_db:';
PRINT '    - ITEMS_CATEGORY';
PRINT '    - ITEMS';
PRINT '    - PROVIDERS';
PRINT '    - PRODUCTS';
PRINT '    - PRODUCT_DETAILS';
PRINT '    - REPLACEMENT_ORDERS';
PRINT '    - ORDER_ROWS';
PRINT '    - PURCHASE_ORDERS';
PRINT '';
PRINT 'LISTO PARA USAR';
PRINT '========================================';
GO
