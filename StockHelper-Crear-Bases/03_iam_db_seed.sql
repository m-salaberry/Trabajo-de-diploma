-- ==============================================================
-- DATOS SEMILLA de iam_db  (permisos + rol Administrator + usuario admin)
-- Extraido del estado ACTUAL en vivo. Idempotente (IF NOT EXISTS).
--
-- Usuario admin inicial:  usuario = 'admin'   contrasena = 'admin'
-- (contrasena hasheada MD5/UTF-16LE por CryptographyService.HashMd5)
-- CAMBIAR la contrasena tras el primer inicio de sesion.
-- ==============================================================
USE [iam_db];
GO

-- ---------- PERMISOS (PATENTS) ----------
IF NOT EXISTS (SELECT 1 FROM dbo.PATENTS WHERE Name = N'Analytics')
    INSERT INTO dbo.PATENTS (Id, Name, Description, CreatedDate)
    VALUES ('97248E28-7B6A-433B-8031-D8D34A93BCA8', N'Analytics', N'Grants access to view and generate reports.', GETDATE());
IF NOT EXISTS (SELECT 1 FROM dbo.PATENTS WHERE Name = N'ItemCategoryManagment')
    INSERT INTO dbo.PATENTS (Id, Name, Description, CreatedDate)
    VALUES ('F5FB9B1E-672D-4254-BEED-2BE9C30FB4D7', N'ItemCategoryManagment', N'Permission to access the Item and Category Management module. Grants access to manage product categories and item details.', GETDATE());
IF NOT EXISTS (SELECT 1 FROM dbo.PATENTS WHERE Name = N'PermissionManagement')
    INSERT INTO dbo.PATENTS (Id, Name, Description, CreatedDate)
    VALUES ('5A0B5CEB-0DA0-4968-97A4-A69F0E30E254', N'PermissionManagement', N'Complete access to permissions and roles managment module', GETDATE());
IF NOT EXISTS (SELECT 1 FROM dbo.PATENTS WHERE Name = N'ProductBuilder')
    INSERT INTO dbo.PATENTS (Id, Name, Description, CreatedDate)
    VALUES ('FF046166-C47B-4962-8268-9A1969163B92', N'ProductBuilder', N'Permission to access the Product Builder module. Grants access to create and manage products.', GETDATE());
IF NOT EXISTS (SELECT 1 FROM dbo.PATENTS WHERE Name = N'PurchaseManagement')
    INSERT INTO dbo.PATENTS (Id, Name, Description, CreatedDate)
    VALUES ('F8F13E3F-395C-4C04-B7CC-9F86F4D3892B', N'PurchaseManagement', N'Grants full access to create and manage purchase orders.', GETDATE());
IF NOT EXISTS (SELECT 1 FROM dbo.PATENTS WHERE Name = N'StockManagment')
    INSERT INTO dbo.PATENTS (Id, Name, Description, CreatedDate)
    VALUES ('FCBE1DEE-CB2F-4140-BAEB-6E75AC4F39E4', N'StockManagment', N'Grants full access to manage stock levels.', GETDATE());
IF NOT EXISTS (SELECT 1 FROM dbo.PATENTS WHERE Name = N'SupplierManagment')
    INSERT INTO dbo.PATENTS (Id, Name, Description, CreatedDate)
    VALUES ('923394ED-550D-42A3-9DE6-88C2239F0759', N'SupplierManagment', N'Permission to access the Supplier Management module. Grants access to manage supplier information and contacts.', GETDATE());
IF NOT EXISTS (SELECT 1 FROM dbo.PATENTS WHERE Name = N'SystemLogs')
    INSERT INTO dbo.PATENTS (Id, Name, Description, CreatedDate)
    VALUES ('632D0410-ADA5-4DB2-9351-B43B32E9D02C', N'SystemLogs', N'Permission to access the logs registry', GETDATE());
IF NOT EXISTS (SELECT 1 FROM dbo.PATENTS WHERE Name = N'UserManagement')
    INSERT INTO dbo.PATENTS (Id, Name, Description, CreatedDate)
    VALUES ('4924F938-F2E3-40A2-85BD-CDAEB24CF7B5', N'UserManagement', N'Complete access to user managment module', GETDATE());
GO

-- ---------- ROL ADMINISTRATOR (FAMILY) ----------
IF NOT EXISTS (SELECT 1 FROM dbo.FAMILIES WHERE Name = N'Administrator')
    INSERT INTO dbo.FAMILIES (Id, Name, Description, CreatedDate)
    VALUES (NEWID(), N'Administrator', N'Total access to the system', GETDATE());
GO

-- ---------- ASIGNAR TODOS LOS PERMISOS AL ROL ADMINISTRATOR ----------
INSERT INTO dbo.PATENTS_FAMILIES (PatentId, FamilyId, AssignedDate)
SELECT p.Id, f.Id, GETDATE()
FROM dbo.PATENTS p
CROSS JOIN dbo.FAMILIES f
WHERE f.Name = N'Administrator'
  AND NOT EXISTS (SELECT 1 FROM dbo.PATENTS_FAMILIES pf WHERE pf.PatentId = p.Id AND pf.FamilyId = f.Id);
GO

-- ---------- USUARIO ADMIN INICIAL ----------
-- MD5(UTF-16LE) de 'admin' = 19a2854144b63a8f7617a6f225019b12
IF NOT EXISTS (SELECT 1 FROM dbo.USERS WHERE Name = N'admin')
    INSERT INTO dbo.USERS (Id, Name, Password, Role, IsActive, CreatedDate)
    VALUES (NEWID(), N'admin', N'19a2854144b63a8f7617a6f225019b12', N'Administrator', 1, GETDATE());
GO

-- ---------- VINCULAR admin AL ROL ADMINISTRATOR ----------
INSERT INTO dbo.USERS_FAMILIES (UserId, FamilyId, AssignedDate)
SELECT u.Id, f.Id, GETDATE()
FROM dbo.USERS u, dbo.FAMILIES f
WHERE u.Name = N'admin' AND f.Name = N'Administrator'
  AND NOT EXISTS (SELECT 1 FROM dbo.USERS_FAMILIES uf WHERE uf.UserId = u.Id AND uf.FamilyId = f.Id);
GO
