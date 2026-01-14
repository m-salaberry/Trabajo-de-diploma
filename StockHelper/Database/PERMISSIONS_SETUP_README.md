# ?? Scripts de Permisos - Guía de Uso

## ?? **Scripts Disponibles**

### **1. Setup_Permissions.sql** (RECOMENDADO)
Script completo con validaciones y mensajes detallados.

**Características:**
- ? Verifica si existen permisos antes de insertar
- ? No elimina datos existentes (seguro)
- ? Muestra mensajes detallados de progreso
- ? Incluye descripciones en los permisos
- ? Verificación final de datos
- ? Resumen completo al finalizar

**Uso:**
```sql
-- En SQL Server Management Studio
1. Conectar a: MATI\SQLEXPRESS
2. Seleccionar base de datos: iam_db
3. Abrir: Database\Setup_Permissions.sql
4. Ejecutar (F5)
```

**Resultado:** Permisos y roles cargados sin eliminar datos existentes

---

### **2. Quick_Setup_Permissions.sql**
Script rápido para recrear todo desde cero.

**Características:**
- ?? **ELIMINA** todos los permisos y roles existentes
- ? Muy rápido (sin validaciones)
- ? Útil para desarrollo/testing
- ? **NO usar en producción con datos reales**

**Uso:**
```sql
-- CUIDADO: Elimina todo
-- Solo usar en desarrollo
1. Conectar a: MATI\SQLEXPRESS
2. Seleccionar base de datos: iam_db
3. Abrir: Database\Quick_Setup_Permissions.sql
4. Ejecutar (F5)
```

**Resultado:** Todo eliminado y recreado

---

## ?? **Permisos Creados (20)**

### **Management (2)**
- `UserManagement` - Gestión de usuarios
- `PermissionManagement` - Gestión de permisos y roles

### **Inventory (3)**
- `InventoryManagement` - Gestión de inventario
- `ProductCatalog` - Catálogo de productos
- `StockControl` - Control de stock

### **Sales & Purchases (3)**
- `SalesManagement` - Gestión de ventas
- `PurchaseManagement` - Gestión de compras
- `PointOfSale` - Punto de venta (POS)

### **Reports (4)**
- `Reports` - Reportes generales
- `SalesReports` - Reportes de ventas
- `InventoryReports` - Reportes de inventario
- `FinancialReports` - Reportes financieros

### **Customers & Suppliers (2)**
- `CustomerManagement` - Gestión de clientes
- `SupplierManagement` - Gestión de proveedores

### **System (3)**
- `SystemConfiguration` - Configuración del sistema
- `SystemLogs` - Logs del sistema
- `DatabaseBackup` - Backup y restore

### **Additional (3)**
- `Dashboard` - Panel principal
- `PricingManagement` - Gestión de precios
- `WarehouseManagement` - Gestión de almacenes

---

## ?? **Roles Creados (9)**

| Rol | Permisos | Descripción |
|-----|----------|-------------|
| **Administrator** | 20/20 | Acceso total al sistema |
| **Manager** | 14/20 | Gestión operativa y reportes |
| **Salesperson** | 6/20 | Ventas y clientes |
| **InventoryManager** | 8/20 | Inventario y almacén |
| **Cashier** | 2/20 | Solo punto de venta |
| **Auditor** | 6/20 | Solo reportes (read-only) |
| **WarehouseOperator** | 5/20 | Almacén y stock |
| **Accountant** | 4/20 | Finanzas y precios |
| **Guest** | 1/20 | Solo dashboard |

---

## ?? **Guía de Ejecución**

### **Escenario 1: Primera Vez (Base de Datos Nueva)**

```sql
-- Paso 1: Ejecutar DatabaseSetup_Complete.sql (crear tablas)
-- Paso 2: Ejecutar Setup_Permissions.sql (cargar permisos)
```

---

### **Escenario 2: Actualizar Permisos (Mantener Datos)**

```sql
-- Ejecutar: Setup_Permissions.sql
-- Resultado: Agrega permisos que faltan, no elimina existentes
```

---

### **Escenario 3: Recrear Todo (Desarrollo/Testing)**

```sql
-- Ejecutar: Quick_Setup_Permissions.sql
-- Resultado: Elimina todo y recrea desde cero
```

---

## ? **Verificación Post-Ejecución**

### **1. Verificar Permisos**
```sql
SELECT COUNT(*) AS TotalPermisos FROM PATENTS;
-- Resultado esperado: 20
```

### **2. Verificar Roles**
```sql
SELECT COUNT(*) AS TotalRoles FROM FAMILIES;
-- Resultado esperado: 9
```

### **3. Verificar Permisos por Rol**
```sql
SELECT 
    f.Name AS Rol,
    COUNT(pf.PatentId) AS CantidadPermisos
FROM FAMILIES f
LEFT JOIN PATENTS_FAMILIES pf ON f.Id = pf.FamilyId
GROUP BY f.Name
ORDER BY CantidadPermisos DESC;
```

**Resultado esperado:**
```
Rol                 CantidadPermisos
------------------- ----------------
Administrator       20
Manager             14
InventoryManager    8
Salesperson         6
Auditor             6
WarehouseOperator   5
Accountant          4
Cashier             2
Guest               1
```

### **4. Verificar Usuario Admin**
```sql
SELECT 
    u.Name AS Usuario,
    f.Name AS Rol
FROM USERS u
JOIN USERS_FAMILIES uf ON u.Id = uf.UserId
JOIN FAMILIES f ON uf.FamilyId = f.Id
WHERE u.Name = 'admin';
```

**Resultado esperado:**
```
Usuario  Rol
-------- -------------
admin    Administrator
```

---

## ?? **Solución de Problemas**

### **Problema: "Usuario admin no encontrado"**

**Solución:** Crear usuario admin primero
```sql
INSERT INTO USERS (Id, Name, Password, IsActive)
VALUES (NEWID(), 'admin', 'admin', 1);
```

Luego ejecutar el script de permisos nuevamente.

---

### **Problema: "Los permisos ya existen"**

**Solución Normal:** El script `Setup_Permissions.sql` valida y no inserta duplicados.

**Solución Forzada:** Si quieres recrear todo, usar `Quick_Setup_Permissions.sql`

---

### **Problema: "Foreign Key Constraint Error"**

**Causa:** Intentar eliminar permisos que tienen usuarios asignados.

**Solución:**
```sql
-- Eliminar en orden correcto:
DELETE FROM USERS_FAMILIES;
DELETE FROM PATENTS_FAMILIES;
DELETE FROM FAMILIES;
DELETE FROM PATENTS;
```

---

## ?? **Alineación con Código C#**

Los permisos en SQL coinciden con `PermissionNames.cs`:

```csharp
// En C# (Services\Domain\PermissionNames.cs)
public static class PermissionNames
{
    public const string UserManagement = "UserManagement";
    public const string PermissionManagement = "PermissionManagement";
    public const string InventoryManagement = "InventoryManagement";
    // ... etc (20 permisos)
}
```

```sql
-- En SQL (Setup_Permissions.sql)
INSERT INTO PATENTS (Id, Name) VALUES 
(NEWID(), 'UserManagement'),
(NEWID(), 'PermissionManagement'),
(NEWID(), 'InventoryManagement');
-- ... etc (20 permisos)
```

**? Nombres idénticos = Sin errores de typos**

---

## ?? **Seguridad**

### **Desarrollo**
```sql
-- Passwords en texto plano (OK en desarrollo)
Password = 'admin'
```

### **Producción**
```sql
-- Cambiar a passwords hasheados
-- Usar BCrypt, PBKDF2, o Argon2
Password = '$2a$10$...' -- Hash de BCrypt
```

---

## ?? **Estado Después de Ejecutar**

```
? 20 permisos en tabla PATENTS
? 9 roles en tabla FAMILIES
? ~100 relaciones en PATENTS_FAMILIES
? Usuario 'admin' con rol Administrator
? Sistema listo para usar
```

---

## ?? **Siguiente Paso**

Después de ejecutar el script:

1. ? Abrir Visual Studio
2. ? Verificar `App.config` con connection string correcto
3. ? Ejecutar aplicación
4. ? Login como: `admin` / `admin`
5. ? Verificar que los menús se muestren correctamente

---

## ?? **Soporte**

Si tienes problemas:

1. Verificar que la base de datos `iam_db` exista
2. Verificar connection string en `App.config`
3. Verificar que el usuario SQL tenga permisos
4. Revisar mensajes de error en SQL Server
5. Ejecutar queries de verificación mostrados arriba

---

## ?? **Archivos Relacionados**

- `DatabaseSetup_Complete.sql` - Crear estructura de tablas
- `Setup_Permissions.sql` - Cargar permisos (RECOMENDADO)
- `Quick_Setup_Permissions.sql` - Recrear rápido
- `TestData_Setup.sql` - Datos de prueba (opcional)
- `DATABASE_SCHEMA_README.md` - Documentación del esquema

---

**¡Permisos listos para StockHelper!** ??
