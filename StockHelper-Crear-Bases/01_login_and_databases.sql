-- ==============================================================
-- PASO 1: LOGIN DE ACCESO + CREACION DE BASES DE DATOS
-- ==============================================================
-- Ejecutar CONECTADO COMO ADMINISTRADOR del servidor (sa o un login
-- de Windows sysadmin). El login 'stock_helper_user' todavia no existe
-- en la primera instalacion, por eso NO se puede usar el aqui.
--
-- Requiere que SQL Server este en MODO MIXTO (SQL + Windows auth),
-- porque la aplicacion se conecta con el login SQL 'stock_helper_user'.
-- ==============================================================

USE master;
GO

-- ---------- LOGIN DE LA APLICACION ----------
-- Debe coincidir con {sqlUser}/{sqlPassword} de UI.dll.config (App.config).
-- CHECK_POLICY = OFF para que la contrasena no sea rechazada por la
-- politica de complejidad de Windows.
IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = N'stock_helper_user')
BEGIN
    CREATE LOGIN [stock_helper_user] WITH PASSWORD = N's7bOGv66G''*c', CHECK_POLICY = OFF;
    PRINT 'Login "stock_helper_user" creado.';
END
ELSE
    PRINT 'Login "stock_helper_user" ya existia.';
GO

-- ---------- BASES DE DATOS ----------
IF DB_ID(N'iam_db') IS NULL
BEGIN
    CREATE DATABASE [iam_db];
    PRINT 'Base "iam_db" creada.';
END
ELSE
    PRINT 'Base "iam_db" ya existia.';
GO

IF DB_ID(N'core_db') IS NULL
BEGIN
    CREATE DATABASE [core_db];
    PRINT 'Base "core_db" creada.';
END
ELSE
    PRINT 'Base "core_db" ya existia.';
GO

-- ---------- USUARIO DE BASE + PERMISOS (iam_db) ----------
USE [iam_db];
GO
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'stock_helper_user')
    CREATE USER [stock_helper_user] FOR LOGIN [stock_helper_user];
GO
ALTER ROLE [db_owner] ADD MEMBER [stock_helper_user];
GO

-- ---------- USUARIO DE BASE + PERMISOS (core_db) ----------
USE [core_db];
GO
IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'stock_helper_user')
    CREATE USER [stock_helper_user] FOR LOGIN [stock_helper_user];
GO
-- db_owner permite CRUD y ademas BACKUP DATABASE (respaldo diario, REQ.006).
ALTER ROLE [db_owner] ADD MEMBER [stock_helper_user];
GO
