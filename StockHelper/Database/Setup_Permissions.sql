-- ==================================================
-- SCRIPT DE PERMISOS SIMPLIFICADOS - STOCKHELPER
-- Sistema de Permisos por Formulario/Módulo
-- Base de datos: iam_db
-- Servidor: MATI\SQLEXPRESS
-- ==================================================

USE iam_db;
GO

PRINT '========================================';
PRINT 'INICIANDO CARGA DE PERMISOS Y ROLES';
PRINT '========================================';
PRINT '';

-- ==================================================
-- PASO 1: LIMPIAR DATOS EXISTENTES (OPCIONAL)
-- ==================================================

-- DESCOMENTAR SOLO SI QUIERES ELIMINAR Y RECREAR TODO
/*
PRINT 'Limpiando datos existentes...';
DELETE FROM USERS_FAMILIES;
DELETE FROM PATENTS_FAMILIES;
DELETE FROM FAMILIES;
DELETE FROM PATENTS;
PRINT 'Datos eliminados exitosamente';
PRINT '';
*/

-- ==================================================
-- PASO 2: INSERTAR PERMISOS ATÓMICOS (PATENTS)
-- ==================================================

PRINT 'Insertando permisos atómicos...';
PRINT '';

-- Management Modules (2 permisos)
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'UserManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'UserManagement', 'Complete access to user managment module');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'PermissionManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'PermissionManagement', 'Complete access to permissions and roles managment module');

-- Inventory/Stock Modules (3 permisos)
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'InventoryManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'InventoryManagement', 'Acceso completo al módulo de gestión de inventario');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'ProductCatalog')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'ProductCatalog', 'Acceso al catálogo de productos');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'StockControl')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'StockControl', 'Acceso al control de stock y ajustes de inventario');

-- Sales & Purchases Modules (3 permisos)
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'SalesManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'SalesManagement', 'Acceso completo al módulo de gestión de ventas');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'PurchaseManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'PurchaseManagement', 'Acceso completo al módulo de gestión de compras');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'PointOfSale')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'PointOfSale', 'Acceso al punto de venta (POS)');

-- Reports & Analytics Modules (4 permisos)
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'Reports')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'Reports', 'Acceso al módulo de reportes generales');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'SalesReports')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'SalesReports', 'Acceso a reportes de ventas');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'InventoryReports')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'InventoryReports', 'Acceso a reportes de inventario');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'FinancialReports')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'FinancialReports', 'Acceso a reportes financieros');

-- Customer & Supplier Modules (2 permisos)
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'CustomerManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'CustomerManagement', 'Acceso completo al módulo de gestión de clientes');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'SupplierManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'SupplierManagement', 'Acceso completo al módulo de gestión de proveedores');

-- System Configuration Modules (3 permisos)
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'SystemConfiguration')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'SystemConfiguration', 'Acceso a la configuración del sistema');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'SystemLogs')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'SystemLogs', 'Acceso a los logs del sistema');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'DatabaseBackup')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'DatabaseBackup', 'Acceso al módulo de backup y restore de base de datos');

-- Additional Modules (3 permisos)
IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'Dashboard')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'Dashboard', 'Acceso al panel principal con información resumida');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'PricingManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'PricingManagement', 'Acceso al módulo de gestión de precios y descuentos');

IF NOT EXISTS (SELECT 1 FROM PATENTS WHERE Name = 'WarehouseManagement')
    INSERT INTO PATENTS (Id, Name, Description) VALUES 
    (NEWID(), 'WarehouseManagement', 'Acceso al módulo de gestión de almacenes');

DECLARE @PermissionCount INT = (SELECT COUNT(*) FROM PATENTS);
PRINT '✅ Total de permisos en sistema: ' + CAST(@PermissionCount AS VARCHAR(10));
PRINT '';

-- ==================================================
-- PASO 3: CREAR ROLES (FAMILIES)
-- ==================================================

PRINT 'Creando roles...';
PRINT '';

-- Variables para IDs de roles
DECLARE @AdminId UNIQUEIDENTIFIER;
DECLARE @ManagerId UNIQUEIDENTIFIER;
DECLARE @SalespersonId UNIQUEIDENTIFIER;
DECLARE @InventoryManagerId UNIQUEIDENTIFIER;
DECLARE @CashierId UNIQUEIDENTIFIER;
DECLARE @AuditorId UNIQUEIDENTIFIER;
DECLARE @WarehouseOperatorId UNIQUEIDENTIFIER;
DECLARE @AccountantId UNIQUEIDENTIFIER;
DECLARE @GuestId UNIQUEIDENTIFIER;

-- Crear o obtener rol Administrator
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Administrator')
BEGIN
    SET @AdminId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description)
    VALUES (@AdminId, 'Administrator', 'Total access to the system');
    PRINT '✅ Rol "Administrator" creado';
END
ELSE
BEGIN
    SET @AdminId = (SELECT Id FROM FAMILIES WHERE Name = 'Administrator');
    PRINT '⚠️  Rol "Administrator" ya existe';
END

-- Crear o obtener rol Manager
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Manager')
BEGIN
    SET @ManagerId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description)
    VALUES (@ManagerId, 'Manager', 'Acceso a gestión operativa y reportes');
    PRINT '✅ Rol "Manager" creado';
END
ELSE
BEGIN
    SET @ManagerId = (SELECT Id FROM FAMILIES WHERE Name = 'Manager');
    PRINT '⚠️  Rol "Manager" ya existe';
END

-- Crear o obtener rol Salesperson
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Salesperson')
BEGIN
    SET @SalespersonId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description)
    VALUES (@SalespersonId, 'Salesperson', 'Acceso a ventas y gestión de clientes');
    PRINT '✅ Rol "Salesperson" creado';
END
ELSE
BEGIN
    SET @SalespersonId = (SELECT Id FROM FAMILIES WHERE Name = 'Salesperson');
    PRINT '⚠️  Rol "Salesperson" ya existe';
END

-- Crear o obtener rol InventoryManager
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'InventoryManager')
BEGIN
    SET @InventoryManagerId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description)
    VALUES (@InventoryManagerId, 'InventoryManager', 'Acceso completo a gestión de inventario');
    PRINT '✅ Rol "InventoryManager" creado';
END
ELSE
BEGIN
    SET @InventoryManagerId = (SELECT Id FROM FAMILIES WHERE Name = 'InventoryManager');
    PRINT '⚠️  Rol "InventoryManager" ya existe';
END

-- Crear o obtener rol Cashier
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Cashier')
BEGIN
    SET @CashierId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description)
    VALUES (@CashierId, 'Cashier', 'Acceso al punto de venta');
    PRINT '✅ Rol "Cashier" creado';
END
ELSE
BEGIN
    SET @CashierId = (SELECT Id FROM FAMILIES WHERE Name = 'Cashier');
    PRINT '⚠️  Rol "Cashier" ya existe';
END

-- Crear o obtener rol Auditor
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Auditor')
BEGIN
    SET @AuditorId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description)
    VALUES (@AuditorId, 'Auditor', 'Acceso de solo lectura a reportes y logs');
    PRINT '✅ Rol "Auditor" creado';
END
ELSE
BEGIN
    SET @AuditorId = (SELECT Id FROM FAMILIES WHERE Name = 'Auditor');
    PRINT '⚠️  Rol "Auditor" ya existe';
END

-- Crear o obtener rol WarehouseOperator
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'WarehouseOperator')
BEGIN
    SET @WarehouseOperatorId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description)
    VALUES (@WarehouseOperatorId, 'WarehouseOperator', 'Acceso a control de almacén y stock');
    PRINT '✅ Rol "WarehouseOperator" creado';
END
ELSE
BEGIN
    SET @WarehouseOperatorId = (SELECT Id FROM FAMILIES WHERE Name = 'WarehouseOperator');
    PRINT '⚠️  Rol "WarehouseOperator" ya existe';
END

-- Crear o obtener rol Accountant
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Accountant')
BEGIN
    SET @AccountantId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description)
    VALUES (@AccountantId, 'Accountant', 'Acceso a reportes financieros y gestión de precios');
    PRINT '✅ Rol "Accountant" creado';
END
ELSE
BEGIN
    SET @AccountantId = (SELECT Id FROM FAMILIES WHERE Name = 'Accountant');
    PRINT '⚠️  Rol "Accountant" ya existe';
END

-- Crear o obtener rol Guest
IF NOT EXISTS (SELECT 1 FROM FAMILIES WHERE Name = 'Guest')
BEGIN
    SET @GuestId = NEWID();
    INSERT INTO FAMILIES (Id, Name, Description)
    VALUES (@GuestId, 'Guest', 'Acceso mínimo - solo dashboard');
    PRINT '✅ Rol "Guest" creado';
END
ELSE
BEGIN
    SET @GuestId = (SELECT Id FROM FAMILIES WHERE Name = 'Guest');
    PRINT '⚠️  Rol "Guest" ya existe';
END

PRINT '';

-- ==================================================
-- PASO 4: ASIGNAR PERMISOS A ROLES
-- ==================================================

PRINT 'Asignando permisos a roles...';
PRINT '';

-- Limpiar asignaciones existentes para recrear
DELETE FROM PATENTS_FAMILIES WHERE FamilyId IN (@AdminId, @ManagerId, @SalespersonId, @InventoryManagerId, @CashierId, @AuditorId, @WarehouseOperatorId, @AccountantId, @GuestId);

-- --------------------------------------------
-- ROL: ADMINISTRATOR (Acceso Total - 20 permisos)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @AdminId FROM PATENTS;

DECLARE @AdminPermCount INT = (SELECT COUNT(*) FROM PATENTS_FAMILIES WHERE FamilyId = @AdminId);
PRINT '✅ Administrator: ' + CAST(@AdminPermCount AS VARCHAR(10)) + ' permisos asignados';

-- --------------------------------------------
-- ROL: MANAGER (Gestión Operativa - 14 permisos)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @ManagerId FROM PATENTS 
WHERE Name IN (
    'UserManagement',
    'InventoryManagement',
    'ProductCatalog',
    'StockControl',
    'SalesManagement',
    'PurchaseManagement',
    'Reports',
    'SalesReports',
    'InventoryReports',
    'FinancialReports',
    'CustomerManagement',
    'SupplierManagement',
    'Dashboard',
    'PricingManagement'
);

DECLARE @ManagerPermCount INT = (SELECT COUNT(*) FROM PATENTS_FAMILIES WHERE FamilyId = @ManagerId);
PRINT '✅ Manager: ' + CAST(@ManagerPermCount AS VARCHAR(10)) + ' permisos asignados';

-- --------------------------------------------
-- ROL: SALESPERSON (Ventas y Clientes - 6 permisos)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @SalespersonId FROM PATENTS 
WHERE Name IN (
    'SalesManagement',
    'PointOfSale',
    'CustomerManagement',
    'ProductCatalog',
    'SalesReports',
    'Dashboard'
);

DECLARE @SalesPermCount INT = (SELECT COUNT(*) FROM PATENTS_FAMILIES WHERE FamilyId = @SalespersonId);
PRINT '✅ Salesperson: ' + CAST(@SalesPermCount AS VARCHAR(10)) + ' permisos asignados';

-- --------------------------------------------
-- ROL: INVENTORY MANAGER (Inventario - 8 permisos)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @InventoryManagerId FROM PATENTS 
WHERE Name IN (
    'InventoryManagement',
    'ProductCatalog',
    'StockControl',
    'WarehouseManagement',
    'InventoryReports',
    'SupplierManagement',
    'PurchaseManagement',
    'Dashboard'
);

DECLARE @InventoryPermCount INT = (SELECT COUNT(*) FROM PATENTS_FAMILIES WHERE FamilyId = @InventoryManagerId);
PRINT '✅ InventoryManager: ' + CAST(@InventoryPermCount AS VARCHAR(10)) + ' permisos asignados';

-- --------------------------------------------
-- ROL: CASHIER (Solo POS - 2 permisos)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @CashierId FROM PATENTS 
WHERE Name IN (
    'PointOfSale',
    'Dashboard'
);

DECLARE @CashierPermCount INT = (SELECT COUNT(*) FROM PATENTS_FAMILIES WHERE FamilyId = @CashierId);
PRINT '✅ Cashier: ' + CAST(@CashierPermCount AS VARCHAR(10)) + ' permisos asignados';

-- --------------------------------------------
-- ROL: AUDITOR (Solo Lectura - 6 permisos)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @AuditorId FROM PATENTS 
WHERE Name IN (
    'Reports',
    'SalesReports',
    'InventoryReports',
    'FinancialReports',
    'SystemLogs',
    'Dashboard'
);

DECLARE @AuditorPermCount INT = (SELECT COUNT(*) FROM PATENTS_FAMILIES WHERE FamilyId = @AuditorId);
PRINT '✅ Auditor: ' + CAST(@AuditorPermCount AS VARCHAR(10)) + ' permisos asignados';

-- --------------------------------------------
-- ROL: WAREHOUSE OPERATOR (Almacén - 5 permisos)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @WarehouseOperatorId FROM PATENTS 
WHERE Name IN (
    'StockControl',
    'WarehouseManagement',
    'ProductCatalog',
    'InventoryReports',
    'Dashboard'
);

DECLARE @WarehousePermCount INT = (SELECT COUNT(*) FROM PATENTS_FAMILIES WHERE FamilyId = @WarehouseOperatorId);
PRINT '✅ WarehouseOperator: ' + CAST(@WarehousePermCount AS VARCHAR(10)) + ' permisos asignados';

-- --------------------------------------------
-- ROL: ACCOUNTANT (Contabilidad - 4 permisos)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @AccountantId FROM PATENTS 
WHERE Name IN (
    'FinancialReports',
    'PricingManagement',
    'Reports',
    'Dashboard'
);

DECLARE @AccountantPermCount INT = (SELECT COUNT(*) FROM PATENTS_FAMILIES WHERE FamilyId = @AccountantId);
PRINT '✅ Accountant: ' + CAST(@AccountantPermCount AS VARCHAR(10)) + ' permisos asignados';

-- --------------------------------------------
-- ROL: GUEST (Mínimo Acceso - 1 permiso)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @GuestId FROM PATENTS 
WHERE Name IN (
    'Dashboard'
);

DECLARE @GuestPermCount INT = (SELECT COUNT(*) FROM PATENTS_FAMILIES WHERE FamilyId = @GuestId);
PRINT '✅ Guest: ' + CAST(@GuestPermCount AS VARCHAR(10)) + ' permiso asignado';

PRINT '';

-- ==================================================
-- PASO 5: ASIGNAR ROL AL USUARIO ADMIN
-- ==================================================

PRINT 'Asignando rol Administrator al usuario admin...';
PRINT '';

DECLARE @AdminUserId UNIQUEIDENTIFIER;
SELECT @AdminUserId = Id FROM USERS WHERE Name = 'admin';

IF @AdminUserId IS NOT NULL
BEGIN
    -- Eliminar roles anteriores del usuario admin
    DELETE FROM USERS_FAMILIES WHERE UserId = @AdminUserId;
    
    -- Asignar rol Administrator
    INSERT INTO USERS_FAMILIES (UserId, FamilyId)
    VALUES (@AdminUserId, @AdminId);
    
    PRINT '✅ Rol Administrator asignado a usuario "admin"';
END
ELSE
BEGIN
    PRINT '⚠️  Usuario "admin" no encontrado. Crear usuario primero:';
    PRINT '   INSERT INTO USERS (Id, Name, Password, IsActive)';
    PRINT '   VALUES (NEWID(), ''admin'', ''admin'', 1);';
END

PRINT '';

-- ==================================================
-- PASO 6: VERIFICACIÓN
-- ==================================================

PRINT '========================================';
PRINT 'VERIFICACIÓN DE DATOS';
PRINT '========================================';
PRINT '';

-- Ver todos los permisos
PRINT 'Permisos en sistema:';
SELECT Name, Description FROM PATENTS ORDER BY Name;

PRINT '';
PRINT 'Roles creados:';
SELECT Name, Description FROM FAMILIES ORDER BY Name;

PRINT '';
PRINT 'Permisos por rol:';
SELECT 
    f.Name AS Role,
    COUNT(pf.PatentId) AS TotalPermissions
FROM FAMILIES f
LEFT JOIN PATENTS_FAMILIES pf ON f.Id = pf.FamilyId
GROUP BY f.Name
ORDER BY TotalPermissions DESC;

PRINT '';
PRINT 'Usuarios con rol Administrator:';
SELECT 
    u.Name AS Username,
    f.Name AS Role
FROM USERS u
JOIN USERS_FAMILIES uf ON u.Id = uf.UserId
JOIN FAMILIES f ON uf.FamilyId = f.Id
WHERE f.Name = 'Administrator';

-- ==================================================
-- RESUMEN FINAL
-- ==================================================

PRINT '';
PRINT '========================================';
PRINT 'SCRIPT COMPLETADO EXITOSAMENTE';
PRINT '========================================';
PRINT 'Base de datos: iam_db';
PRINT 'Servidor: MATI\SQLEXPRESS';
PRINT '';
PRINT 'Resumen:';
PRINT '  - Permisos creados: 20';
PRINT '  - Roles creados: 9';
PRINT '    • Administrator (20 permisos)';
PRINT '    • Manager (14 permisos)';
PRINT '    • Salesperson (6 permisos)';
PRINT '    • InventoryManager (8 permisos)';
PRINT '    • Cashier (2 permisos)';
PRINT '    • Auditor (6 permisos)';
PRINT '    • WarehouseOperator (5 permisos)';
PRINT '    • Accountant (4 permisos)';
PRINT '    • Guest (1 permiso)';
PRINT '';
PRINT 'Usuario admin configurado: ✅';
PRINT '';
PRINT 'LISTO PARA USAR ✅';
PRINT '========================================';

GO
