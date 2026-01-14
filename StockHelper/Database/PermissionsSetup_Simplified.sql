-- ==================================================
-- SCRIPT DE PERMISOS SIMPLIFICADOS
-- Sistema de Permisos por Formulario/Módulo
-- ==================================================

-- ==================================================
-- PASO 1: LIMPIAR PERMISOS ANTIGUOS (Opcional - Solo si migras)
-- ==================================================

-- Crear backup antes de eliminar (IMPORTANTE)
-- SELECT * INTO PATENTS_BACKUP FROM PATENTS;
-- SELECT * INTO PATENTS_FAMILIES_BACKUP FROM PATENTS_FAMILIES;
-- SELECT * INTO USERS_FAMILIES_BACKUP FROM USERS_FAMILIES;
-- SELECT * INTO FAMILIES_BACKUP FROM FAMILIES;

-- Limpiar relaciones
-- DELETE FROM PATENTS_FAMILIES;
-- DELETE FROM USERS_FAMILIES;

-- Limpiar datos
-- DELETE FROM PATENTS;
-- DELETE FROM FAMILIES;

-- ==================================================
-- PASO 2: INSERTAR PERMISOS ATÓMICOS (PATENTS)
-- ==================================================

-- Management Modules
INSERT INTO PATENTS (Id, Name) VALUES 
(NEWID(), 'UserManagement'),
(NEWID(), 'PermissionManagement');

-- Inventory/Stock Modules
INSERT INTO PATENTS (Id, Name) VALUES 
(NEWID(), 'InventoryManagement'),
(NEWID(), 'ProductCatalog'),
(NEWID(), 'StockControl');

-- Sales & Purchases Modules
INSERT INTO PATENTS (Id, Name) VALUES 
(NEWID(), 'SalesManagement'),
(NEWID(), 'PurchaseManagement'),
(NEWID(), 'PointOfSale');

-- Reports & Analytics Modules
INSERT INTO PATENTS (Id, Name) VALUES 
(NEWID(), 'Reports'),
(NEWID(), 'SalesReports'),
(NEWID(), 'InventoryReports'),
(NEWID(), 'FinancialReports');

-- Customer & Supplier Modules
INSERT INTO PATENTS (Id, Name) VALUES 
(NEWID(), 'CustomerManagement'),
(NEWID(), 'SupplierManagement');

-- System Configuration Modules
INSERT INTO PATENTS (Id, Name) VALUES 
(NEWID(), 'SystemConfiguration'),
(NEWID(), 'SystemLogs'),
(NEWID(), 'DatabaseBackup');

-- Additional Modules
INSERT INTO PATENTS (Id, Name) VALUES 
(NEWID(), 'Dashboard'),
(NEWID(), 'PricingManagement'),
(NEWID(), 'WarehouseManagement');

-- ==================================================
-- PASO 3: CREAR ROLES (FAMILIES)
-- ==================================================

-- Variables para IDs de roles
DECLARE @AdminId UNIQUEIDENTIFIER = NEWID();
DECLARE @ManagerId UNIQUEIDENTIFIER = NEWID();
DECLARE @SalespersonId UNIQUEIDENTIFIER = NEWID();
DECLARE @InventoryManagerId UNIQUEIDENTIFIER = NEWID();
DECLARE @CashierId UNIQUEIDENTIFIER = NEWID();
DECLARE @AuditorId UNIQUEIDENTIFIER = NEWID();
DECLARE @WarehouseOperatorId UNIQUEIDENTIFIER = NEWID();
DECLARE @AccountantId UNIQUEIDENTIFIER = NEWID();
DECLARE @GuestId UNIQUEIDENTIFIER = NEWID();

-- Crear Roles
INSERT INTO FAMILIES (Id, Name) VALUES 
(@AdminId, 'Administrator'),
(@ManagerId, 'Manager'),
(@SalespersonId, 'Salesperson'),
(@InventoryManagerId, 'InventoryManager'),
(@CashierId, 'Cashier'),
(@AuditorId, 'Auditor'),
(@WarehouseOperatorId, 'WarehouseOperator'),
(@AccountantId, 'Accountant'),
(@GuestId, 'Guest');

-- ==================================================
-- PASO 4: ASIGNAR PERMISOS A ROLES
-- ==================================================

-- --------------------------------------------
-- ROL: ADMINISTRATOR (Acceso Total)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @AdminId FROM PATENTS;

-- --------------------------------------------
-- ROL: MANAGER (Gestión Operativa)
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

-- --------------------------------------------
-- ROL: SALESPERSON (Ventas y Clientes)
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

-- --------------------------------------------
-- ROL: INVENTORY MANAGER (Inventario)
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

-- --------------------------------------------
-- ROL: CASHIER (Solo POS)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @CashierId FROM PATENTS 
WHERE Name IN (
    'PointOfSale',
    'Dashboard'
);

-- --------------------------------------------
-- ROL: AUDITOR (Solo Lectura - Reportes)
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

-- --------------------------------------------
-- ROL: WAREHOUSE OPERATOR (Almacén)
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

-- --------------------------------------------
-- ROL: ACCOUNTANT (Contabilidad)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @AccountantId FROM PATENTS 
WHERE Name IN (
    'FinancialReports',
    'PricingManagement',
    'Reports',
    'Dashboard'
);

-- --------------------------------------------
-- ROL: GUEST (Mínimo Acceso)
-- --------------------------------------------
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT Id, @GuestId FROM PATENTS 
WHERE Name IN (
    'Dashboard'
);

-- ==================================================
-- PASO 5: ASIGNAR ROLES A USUARIOS
-- ==================================================

-- Ejemplo: Asignar rol Administrator al usuario 'admin'
DECLARE @AdminUserId UNIQUEIDENTIFIER;
SELECT @AdminUserId = Id FROM USERS WHERE Name = 'admin';

IF @AdminUserId IS NOT NULL
BEGIN
    -- Eliminar roles anteriores del usuario
    DELETE FROM USERS_FAMILIES WHERE UserId = @AdminUserId;
    
    -- Asignar rol Administrator
    INSERT INTO USERS_FAMILIES (UserId, FamilyId)
    VALUES (@AdminUserId, @AdminId);
    
    PRINT 'Rol Administrator asignado a usuario admin';
END
ELSE
BEGIN
    PRINT 'Usuario admin no encontrado. Por favor, créalo primero.';
END

-- ==================================================
-- PASO 6: VERIFICACIÓN
-- ==================================================

-- Ver todos los permisos
SELECT * FROM PATENTS ORDER BY Name;

-- Ver todos los roles
SELECT * FROM FAMILIES ORDER BY Name;

-- Ver permisos por rol
SELECT 
    f.Name AS Role,
    p.Name AS Permission
FROM FAMILIES f
JOIN PATENTS_FAMILIES pf ON f.Id = pf.FamilyId
JOIN PATENTS p ON pf.PatentId = p.Id
ORDER BY f.Name, p.Name;

-- Ver usuarios y sus roles
SELECT 
    u.Name AS Username,
    f.Name AS Role
FROM USERS u
JOIN USERS_FAMILIES uf ON u.Id = uf.UserId
JOIN FAMILIES f ON uf.FamilyId = f.Id
ORDER BY u.Name;

-- Contar permisos por rol
SELECT 
    f.Name AS Role,
    COUNT(pf.PatentId) AS PermissionCount
FROM FAMILIES f
LEFT JOIN PATENTS_FAMILIES pf ON f.Id = pf.FamilyId
GROUP BY f.Name
ORDER BY PermissionCount DESC;

-- ==================================================
-- NOTAS IMPORTANTES
-- ==================================================

/*
1. BACKUP: Siempre haz backup antes de ejecutar este script en producción

2. PERSONALIZACIÓN: Ajusta los roles y permisos según las necesidades de tu negocio

3. ORDEN DE EJECUCIÓN:
   - Primero: Eliminar relaciones (PATENTS_FAMILIES, USERS_FAMILIES)
   - Segundo: Eliminar datos (PATENTS, FAMILIES)
   - Tercero: Insertar nuevos datos en este orden

4. TESTING: Prueba con usuarios de diferentes roles antes de ir a producción

5. USUARIOS: Si no existen usuarios, créalos primero:
   
   INSERT INTO USERS (Id, Name, Password, IsActive) VALUES 
   (NEWID(), 'admin', 'admin', 1),
   (NEWID(), 'manager', 'manager', 1),
   (NEWID(), 'salesperson', 'salesperson', 1),
   (NEWID(), 'cashier', 'cashier', 1);

6. PASSWORDS: En producción, usa passwords hasheados, no texto plano

7. AGREGAR MÁS MÓDULOS: Solo agrega permisos a PATENTS y asígnalos a roles
*/

-- ==================================================
-- FIN DEL SCRIPT
-- ==================================================

PRINT 'Script de permisos simplificados ejecutado exitosamente';
PRINT 'Total de permisos creados: ' + CAST((SELECT COUNT(*) FROM PATENTS) AS VARCHAR(10));
PRINT 'Total de roles creados: ' + CAST((SELECT COUNT(*) FROM FAMILIES) AS VARCHAR(10));
