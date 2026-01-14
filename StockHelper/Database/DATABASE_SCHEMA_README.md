# Database Schema - StockHelper

## ?? **ESQUEMA DE BASE DE DATOS**

Este documento describe el esquema completo de la base de datos StockHelper.

---

## ??? **TABLAS PRINCIPALES**

### **1. USERS**
Almacena información de usuarios del sistema.

```sql
CREATE TABLE USERS (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(50) UNIQUE NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NULL,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL,
    LastLoginDate DATETIME NULL
);
```

**Campos:**
- `Id`: Identificador único del usuario (GUID)
- `Name`: Nombre de usuario (único, requerido)
- `Password`: Contraseña (requerido, mín. 4 caracteres)
- `Role`: Campo legacy (no usado, mantener para compatibilidad)
- `IsActive`: Indica si el usuario está activo
- `CreatedDate`: Fecha de creación
- `ModifiedDate`: Fecha de última modificación
- `LastLoginDate`: Fecha del último login

**Constraints:**
- PK: `Id`
- UNIQUE: `Name`
- CHECK: `Name` no vacío
- CHECK: `Password` mínimo 4 caracteres

**Índices:**
- `IX_USERS_Name` en `Name`
- `IX_USERS_IsActive` en `IsActive`

---

### **2. PATENTS**
Almacena permisos atómicos (permisos individuales por módulo).

```sql
CREATE TABLE PATENTS (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(100) UNIQUE NOT NULL,
    Description NVARCHAR(500) NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);
```

**Campos:**
- `Id`: Identificador único del permiso
- `Name`: Nombre del permiso (ej: "UserManagement")
- `Description`: Descripción del permiso (opcional)
- `CreatedDate`: Fecha de creación

**Ejemplos de datos:**
```sql
INSERT INTO PATENTS (Id, Name) VALUES 
(NEWID(), 'UserManagement'),
(NEWID(), 'PermissionManagement'),
(NEWID(), 'InventoryManagement');
```

**Constraints:**
- PK: `Id`
- UNIQUE: `Name`
- CHECK: `Name` no vacío

**Índices:**
- `IX_PATENTS_Name` en `Name`

---

### **3. FAMILIES**
Almacena roles (agrupaciones de permisos).

```sql
CREATE TABLE FAMILIES (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(100) UNIQUE NOT NULL,
    Description NVARCHAR(500) NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ModifiedDate DATETIME NULL
);
```

**Campos:**
- `Id`: Identificador único del rol
- `Name`: Nombre del rol (ej: "Administrator", "Manager")
- `Description`: Descripción del rol (opcional)
- `CreatedDate`: Fecha de creación
- `ModifiedDate`: Fecha de última modificación

**Ejemplos de datos:**
```sql
INSERT INTO FAMILIES (Id, Name) VALUES 
(NEWID(), 'Administrator'),
(NEWID(), 'Manager'),
(NEWID(), 'Salesperson');
```

**Constraints:**
- PK: `Id`
- UNIQUE: `Name`
- CHECK: `Name` no vacío

**Índices:**
- `IX_FAMILIES_Name` en `Name`

---

### **4. PATENTS_FAMILIES**
Relación N:M entre Patents (permisos) y Families (roles).

```sql
CREATE TABLE PATENTS_FAMILIES (
    PatentId UNIQUEIDENTIFIER NOT NULL,
    FamilyId UNIQUEIDENTIFIER NOT NULL,
    AssignedDate DATETIME DEFAULT GETDATE(),
    
    PRIMARY KEY (PatentId, FamilyId),
    FOREIGN KEY (PatentId) REFERENCES PATENTS(Id) ON DELETE CASCADE,
    FOREIGN KEY (FamilyId) REFERENCES FAMILIES(Id) ON DELETE CASCADE
);
```

**Campos:**
- `PatentId`: ID del permiso
- `FamilyId`: ID del rol
- `AssignedDate`: Fecha de asignación

**Relación:**
- Un **permiso** puede estar en varios **roles**
- Un **rol** puede tener varios **permisos**

**Ejemplo:**
```sql
-- Asignar permiso "UserManagement" al rol "Administrator"
INSERT INTO PATENTS_FAMILIES (PatentId, FamilyId)
SELECT p.Id, f.Id 
FROM PATENTS p, FAMILIES f
WHERE p.Name = 'UserManagement' AND f.Name = 'Administrator';
```

**Constraints:**
- PK: `(PatentId, FamilyId)`
- FK: `PatentId` ? `PATENTS(Id)` con CASCADE
- FK: `FamilyId` ? `FAMILIES(Id)` con CASCADE

**Índices:**
- `IX_PATENTS_FAMILIES_PatentId` en `PatentId`
- `IX_PATENTS_FAMILIES_FamilyId` en `FamilyId`

---

### **5. USERS_FAMILIES**
Relación N:M entre Users (usuarios) y Families (roles).

```sql
CREATE TABLE USERS_FAMILIES (
    UserId UNIQUEIDENTIFIER NOT NULL,
    FamilyId UNIQUEIDENTIFIER NOT NULL,
    AssignedDate DATETIME DEFAULT GETDATE(),
    AssignedBy UNIQUEIDENTIFIER NULL,
    
    PRIMARY KEY (UserId, FamilyId),
    FOREIGN KEY (UserId) REFERENCES USERS(Id) ON DELETE CASCADE,
    FOREIGN KEY (FamilyId) REFERENCES FAMILIES(Id) ON DELETE CASCADE
);
```

**Campos:**
- `UserId`: ID del usuario
- `FamilyId`: ID del rol
- `AssignedDate`: Fecha de asignación
- `AssignedBy`: ID del usuario que asignó el rol (opcional)

**Relación:**
- Un **usuario** puede tener varios **roles**
- Un **rol** puede estar asignado a varios **usuarios**

**Ejemplo:**
```sql
-- Asignar rol "Administrator" al usuario "admin"
INSERT INTO USERS_FAMILIES (UserId, FamilyId)
SELECT u.Id, f.Id 
FROM USERS u, FAMILIES f
WHERE u.Name = 'admin' AND f.Name = 'Administrator';
```

**Constraints:**
- PK: `(UserId, FamilyId)`
- FK: `UserId` ? `USERS(Id)` con CASCADE
- FK: `FamilyId` ? `FAMILIES(Id)` con CASCADE

**Índices:**
- `IX_USERS_FAMILIES_UserId` en `UserId`
- `IX_USERS_FAMILIES_FamilyId` en `FamilyId`

---

## ?? **DIAGRAMA DE RELACIONES**

```
???????????????
?   USERS     ?
?  (Usuarios) ?
???????????????
       ? 1
       ?
       ? N
?????????????????????
? USERS_FAMILIES    ? (Relación N:M)
? UserId, FamilyId  ?
?????????????????????
       ? N
       ?
       ? 1
???????????????         ???????????????????
?  FAMILIES   ?         ? PATENTS_FAMILIES? (Relación N:M)
?   (Roles)   ???1???N??? PatentId,       ?
???????????????         ? FamilyId        ?
                        ????????????????????
                                 ? N
                                 ?
                                 ? 1
                        ????????????????????
                        ?    PATENTS       ?
                        ?   (Permisos)     ?
                        ????????????????????
```

---

## ?? **VISTA: vw_UserPermissions**

Vista que muestra todos los permisos de cada usuario (desnormalizado).

```sql
CREATE VIEW vw_UserPermissions AS
SELECT 
    u.Id AS UserId,
    u.Name AS Username,
    u.IsActive AS UserIsActive,
    f.Id AS FamilyId,
    f.Name AS FamilyName,
    p.Id AS PatentId,
    p.Name AS PatentName,
    p.Description AS PatentDescription
FROM USERS u
LEFT JOIN USERS_FAMILIES uf ON u.Id = uf.UserId
LEFT JOIN FAMILIES f ON uf.FamilyId = f.Id
LEFT JOIN PATENTS_FAMILIES pf ON f.Id = pf.FamilyId
LEFT JOIN PATENTS p ON pf.PatentId = p.Id;
```

**Uso:**
```sql
-- Ver todos los permisos de un usuario
SELECT * FROM vw_UserPermissions WHERE Username = 'admin';

-- Ver usuarios con un permiso específico
SELECT DISTINCT Username 
FROM vw_UserPermissions 
WHERE PatentName = 'UserManagement';
```

---

## ?? **STORED PROCEDURES**

### **sp_GetUserPermissions**
Obtiene todos los permisos de un usuario específico.

```sql
EXEC sp_GetUserPermissions @Username = 'admin';
```

**Resultado:**
```
Id                                   Name                Description
------------------------------------ ------------------- -----------
12345678-1234-1234-1234-123456789012 UserManagement      Manage users
...
```

---

### **sp_AssignRoleToUser**
Asigna un rol a un usuario.

```sql
EXEC sp_AssignRoleToUser 
    @Username = 'john.doe', 
    @RoleName = 'Manager';
```

**Comportamiento:**
- Valida que exista el usuario
- Valida que exista el rol
- Verifica si ya está asignado
- Inserta si no existe

---

## ?? **FUNCIÓN: fn_UserHasPermission**

Verifica si un usuario tiene un permiso específico.

```sql
SELECT dbo.fn_UserHasPermission('admin', 'UserManagement');
-- Retorna: 1 (tiene permiso) o 0 (no tiene permiso)
```

**Uso en queries:**
```sql
SELECT * FROM USERS 
WHERE dbo.fn_UserHasPermission(Name, 'UserManagement') = 1;
```

---

## ?? **CONSULTAS ÚTILES**

### **Listar todos los usuarios con sus roles**
```sql
SELECT 
    u.Name AS Username,
    f.Name AS RoleName,
    u.IsActive
FROM USERS u
JOIN USERS_FAMILIES uf ON u.Id = uf.UserId
JOIN FAMILIES f ON uf.FamilyId = f.Id
ORDER BY u.Name, f.Name;
```

### **Listar permisos de un rol**
```sql
SELECT 
    f.Name AS RoleName,
    p.Name AS PermissionName,
    p.Description
FROM FAMILIES f
JOIN PATENTS_FAMILIES pf ON f.Id = pf.FamilyId
JOIN PATENTS p ON pf.PatentId = p.Id
WHERE f.Name = 'Administrator'
ORDER BY p.Name;
```

### **Contar usuarios por rol**
```sql
SELECT 
    f.Name AS RoleName,
    COUNT(uf.UserId) AS UserCount
FROM FAMILIES f
LEFT JOIN USERS_FAMILIES uf ON f.Id = uf.FamilyId
GROUP BY f.Name
ORDER BY UserCount DESC;
```

### **Usuarios sin roles asignados**
```sql
SELECT u.Name
FROM USERS u
LEFT JOIN USERS_FAMILIES uf ON u.Id = uf.UserId
WHERE uf.FamilyId IS NULL;
```

### **Permisos no asignados a ningún rol**
```sql
SELECT p.Name
FROM PATENTS p
LEFT JOIN PATENTS_FAMILIES pf ON p.Id = pf.PatentId
WHERE pf.FamilyId IS NULL;
```

---

## ?? **SEGURIDAD Y MEJORES PRÁCTICAS**

### **1. Passwords**
- ?? **IMPORTANTE:** En producción, usar passwords hasheados
- Recomendado: BCrypt, PBKDF2, o Argon2
- Nunca almacenar contraseñas en texto plano

```csharp
// En C#, usar:
// BCrypt.Net.BCrypt.HashPassword(password)
```

### **2. Índices**
- Todos los campos que se usan en JOINs tienen índices
- `Name` en USERS, PATENTS, FAMILIES están indexados
- Claves foráneas están indexadas

### **3. Cascade Delete**
- ? Configurado en todas las FK
- Eliminar un rol ? elimina sus relaciones
- Eliminar un usuario ? elimina sus roles asignados

### **4. Constraints**
- Validación a nivel BD (no solo en código)
- CHECK constraints para validar longitudes
- UNIQUE constraints para prevenir duplicados

---

## ?? **ESTADÍSTICAS ESPERADAS**

| Tabla | Registros Típicos |
|-------|-------------------|
| USERS | 10-100 usuarios |
| PATENTS | ~20 permisos |
| FAMILIES | 5-10 roles |
| PATENTS_FAMILIES | 50-150 relaciones |
| USERS_FAMILIES | 10-100 relaciones |

---

## ?? **ORDEN DE EJECUCIÓN DE SCRIPTS**

1. **DatabaseSetup_Complete.sql** ? Crear tablas, vistas, SPs
2. **PermissionsSetup_Simplified.sql** ? Cargar permisos y roles
3. Crear usuarios adicionales según necesidad

---

## ?? **MANTENIMIENTO**

### **Backup**
```sql
BACKUP DATABASE StockHelper 
TO DISK = 'C:\Backups\StockHelper.bak'
WITH FORMAT, INIT, NAME = 'Full Backup';
```

### **Restore**
```sql
RESTORE DATABASE StockHelper 
FROM DISK = 'C:\Backups\StockHelper.bak'
WITH REPLACE;
```

### **Limpiar datos de prueba**
```sql
-- Eliminar todos los datos pero mantener estructura
DELETE FROM USERS_FAMILIES;
DELETE FROM PATENTS_FAMILIES;
DELETE FROM USERS WHERE Name != 'admin';
DELETE FROM FAMILIES;
DELETE FROM PATENTS;
```

---

## ?? **SOPORTE**

Si encuentras problemas con el esquema:

1. Verificar que todas las tablas existan
2. Verificar que todas las FK estén creadas
3. Verificar que los índices existan
4. Revisar logs de SQL Server

**Consultas de diagnóstico:**
```sql
-- Ver todas las tablas
SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';

-- Ver todas las FK
SELECT * FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS;

-- Ver todos los índices
SELECT * FROM sys.indexes WHERE object_id IN (
    SELECT object_id FROM sys.tables WHERE name IN ('USERS', 'PATENTS', 'FAMILIES')
);
```
