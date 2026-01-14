# frmMain.cs - Guía de Refactorización con Mejoras de Permisos

## ?? **CÓDIGO ACTUAL vs CÓDIGO MEJORADO**

### **PROBLEMA #1: No Hay Verificación de Permisos en Menús**

#### ? **CÓDIGO ACTUAL**
```csharp
private void tsmUsers_Click(object sender, EventArgs e)
{
    ctrlUsers userManagement = new ctrlUsers();
    showContent(userManagement);
}

private void tsmPerms_Click(object sender, EventArgs e)
{
    ctrlPermsissions permissionManagement = new ctrlPermsissions();
    showContent(permissionManagement);
}
```

**Problemas:**
- ? No verifica si el usuario tiene permiso antes de abrir el módulo
- ? El usuario ve todos los menús aunque no tenga permisos
- ? Puede intentar acceder a funcionalidades prohibidas

---

#### ? **CÓDIGO MEJORADO**
```csharp
private void tsmUsers_Click(object sender, EventArgs e)
{
    // Verificar permiso antes de abrir
    if (!UIPermissionHelper.CanAccessForm(
        currentUser, 
        PermissionNames.ViewUserManagement,
        "User Management"))
    {
        return; // Muestra mensaje de error automáticamente
    }

    ctrlUsers userManagement = new ctrlUsers();
    showContent(userManagement);
}

private void tsmPerms_Click(object sender, EventArgs e)
{
    // Verificar permiso antes de abrir
    if (!UIPermissionHelper.CanAccessForm(
        currentUser, 
        PermissionNames.ViewPermissionManagement,
        "Permission Management"))
    {
        return;
    }

    ctrlPermsissions permissionManagement = new ctrlPermsissions();
    showContent(permissionManagement);
}
```

**Beneficios:**
- ? Verifica permisos antes de abrir formulario
- ? Muestra mensaje de error claro al usuario
- ? Loguea intentos de acceso no autorizado
- ? Usa constantes type-safe (no typos)

---

### **PROBLEMA #2: No Oculta Menús sin Permisos**

#### ? **CÓDIGO ACTUAL**
```csharp
private frmMain(User logedUser)
{
    InitializeComponent();
    ResetMainPanelSize();
    this.CenterToScreen();
    currentUser = logedUser;
    getPatents();
    this.Text = $"StockHelper - Logged in as: {currentUser.Name}";
}
```

**Problemas:**
- ? Todos los menús son visibles para todos los usuarios
- ? El usuario ve opciones que no puede usar
- ? Mala experiencia de usuario (confusión)

---

#### ? **CÓDIGO MEJORADO**
```csharp
private frmMain(User logedUser)
{
    InitializeComponent();
    ResetMainPanelSize();
    this.CenterToScreen();
    currentUser = logedUser;
    
    this.Text = $"StockHelper - Logged in as: {currentUser.Name}";
    
    // ? NUEVO: Configurar UI según permisos
    ConfigurePermissions();
}

/// <summary>
/// Configures menu visibility based on user permissions.
/// </summary>
private void ConfigurePermissions()
{
    // Ocultar menús sin permisos
    UIPermissionHelper.ShowMenuItemIfHasPermission(
        tsmUsers, 
        currentUser, 
        PermissionNames.ViewUserManagement);

    UIPermissionHelper.ShowMenuItemIfHasPermission(
        tsmPerms, 
        currentUser, 
        PermissionNames.ViewPermissionManagement);

    // Log para auditoría
    Logger.Current.Info($"Menu configured for user '{currentUser.Name}'");
}
```

**Beneficios:**
- ? Oculta menús automáticamente si no hay permisos
- ? UI limpia y clara (solo ve lo que puede usar)
- ? Logging de configuración de permisos
- ? Fácil agregar más menús en el futuro

---

### **PROBLEMA #3: Método `frmMain_Load` Ineficiente**

#### ? **CÓDIGO ACTUAL**
```csharp
private void frmMain_Load(object sender, EventArgs e)
{
    List<Services.Domain.Component> family = currentUser.Permissions;
    List<Services.Domain.Component> patents = new List<Services.Domain.Component>();

    foreach (Services.Domain.Component c in family)
    {
        if (c is Family)
        {
            foreach (Services.Domain.Component child in c.Children)
            {
                patents.Add(child);
                Console.WriteLine(child.Name); // ? Debug code
            }
        }
        else
        {
            patents.Add(c);
            Console.WriteLine(child.Name); // ? Debug code
        }
    }
}
```

**Problemas:**
- ? Código complejo para obtener permisos atómicos
- ? `Console.WriteLine` en código de producción
- ? No usa las nuevas extension methods
- ? Variable `patents` no se usa después

---

#### ? **CÓDIGO MEJORADO**
```csharp
private void frmMain_Load(object sender, EventArgs e)
{
    // ? NUEVO: Usar extension method
    List<Patent> userPermissions = currentUser.GetAllAtomicPermissions();
    
    // Log solo en ambiente de test
    if (NativeMethods.testEnvironment)
    {
        Logger.Current.Debug($"User '{currentUser.Name}' has {userPermissions.Count} permissions");
        
        Console.WriteLine($"Permissions for '{currentUser.Name}':");
        foreach (var permission in userPermissions)
        {
            Console.WriteLine($"  - {permission.Name}");
        }
    }
    else
    {
        // Solo log en producción
        Logger.Current.Info($"User '{currentUser.Name}' loaded main form with {userPermissions.Count} permissions");
    }
}
```

**Beneficios:**
- ? Código mucho más simple (1 línea vs 15)
- ? Usa extension method optimizado
- ? Console.WriteLine solo en test
- ? Logging apropiado por ambiente

---

### **PROBLEMA #4: Método `getPatents()` No Claro**

#### ? **CÓDIGO ACTUAL**
```csharp
private Dictionary<Guid, string> patentsDict = new Dictionary<Guid, string> { };

private void getPatents()
{
    List<Services.Domain.Component> patents = _permissionService.GetAll();
    foreach (Services.Domain.Component c in patents)
    {
        if (!(c is Family))
        {
            patentsDict.Add(c.Id, c.Name);
        }
    }
}
```

**Problemas:**
- ? Nombre confuso (`getPatents` pero carga diccionario)
- ? `patentsDict` no se usa en ninguna parte del código
- ? No está claro para qué sirve
- ? Se llama en constructor pero no tiene propósito visible

---

#### ? **CÓDIGO MEJORADO**

**Opción A: Si NO se usa en ningún lado ? ELIMINAR**
```csharp
// ? SIMPLEMENTE ELIMINAR
// private Dictionary<Guid, string> patentsDict = ...
// private void getPatents() { ... }
```

**Opción B: Si se usará en el futuro ? REFACTORIZAR**
```csharp
private Dictionary<Guid, string> _availablePermissionsCache;

/// <summary>
/// Loads and caches all available permissions from the system.
/// Used for permission management forms.
/// </summary>
private void LoadAvailablePermissions()
{
    _availablePermissionsCache = new Dictionary<Guid, string>();
    
    List<Component> allPermissions = _permissionService.GetAll();
    
    foreach (Component permission in allPermissions)
    {
        // Only cache atomic permissions (Patents)
        if (permission is Patent patent)
        {
            _availablePermissionsCache.Add(patent.Id, patent.Name);
        }
    }
    
    Logger.Current.Debug($"Loaded {_availablePermissionsCache.Count} available permissions");
}

/// <summary>
/// Gets the cached dictionary of available permissions.
/// </summary>
public Dictionary<Guid, string> GetAvailablePermissions()
{
    return _availablePermissionsCache ?? new Dictionary<Guid, string>();
}
```

**Recomendación:** Si no se usa, **eliminarlo**. Si se necesita después, es fácil agregarlo.

---

### **PROBLEMA #5: Falta Logging de Acciones**

#### ? **CÓDIGO ACTUAL**
No hay logging de:
- Cuando el usuario abre un módulo
- Qué permisos tiene configurados
- Intentos de acceso no autorizado

---

#### ? **CÓDIGO MEJORADO**
El logging está automático con `UIPermissionHelper`, pero podemos agregar más:

```csharp
private void tsmUsers_Click(object sender, EventArgs e)
{
    Logger.Current.Info($"User '{currentUser.Name}' attempting to access User Management");
    
    if (!UIPermissionHelper.CanAccessForm(
        currentUser, 
        PermissionNames.ViewUserManagement,
        "User Management"))
    {
        // UIPermissionHelper ya loguea el intento fallido
        return;
    }

    Logger.Current.Info($"User '{currentUser.Name}' opened User Management module");
    
    ctrlUsers userManagement = new ctrlUsers();
    showContent(userManagement);
}
```

---

## ?? **CÓDIGO COMPLETO REFACTORIZADO**

Aquí está el código completo de `frmMain.cs` con todas las mejoras:

```csharp
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Services.Domain;
using Services.Implementations;
using Services.Contracts.Logs;
using UI.secondaryForms;
using UI.Helpers;

namespace UI
{
    public partial class frmMain : Form
    {
        private PermissionService _permissionService;
        private User currentUser;
        private static frmMain _instance;

        private frmMain(User logedUser)
        {
            InitializeComponent();
            
            _permissionService = new PermissionService();
            currentUser = logedUser;
            
            ResetMainPanelSize();
            this.CenterToScreen();
            this.Text = $"StockHelper - Logged in as: {currentUser.Name}";
            
            // ? NUEVO: Configurar permisos
            ConfigurePermissions();
            
            Logger.Current.Info($"Main form initialized for user '{currentUser.Name}'");
        }

        public static frmMain GetInstance(User logedUser = null!)
        {
            if (_instance == null || _instance.IsDisposed)
            {
                _instance = new frmMain(logedUser);
            }
            return _instance;
        }

        /// <summary>
        /// Configures menu visibility and state based on user permissions.
        /// </summary>
        private void ConfigurePermissions()
        {
            // Configurar visibilidad de menús según permisos
            UIPermissionHelper.ShowMenuItemIfHasPermission(
                tsmUsers, 
                currentUser, 
                PermissionNames.ViewUserManagement);

            UIPermissionHelper.ShowMenuItemIfHasPermission(
                tsmPerms, 
                currentUser, 
                PermissionNames.ViewPermissionManagement);

            // Agregar más menús aquí cuando se creen:
            // UIPermissionHelper.ShowMenuItemIfHasPermission(
            //     tsmInventory, currentUser, PermissionNames.ViewInventory);
            // UIPermissionHelper.ShowMenuItemIfHasPermission(
            //     tsmReports, currentUser, PermissionNames.ViewReports);
            
            Logger.Current.Info($"Menu configured for user '{currentUser.Name}' - " +
                $"Visible menus: Users={tsmUsers.Visible}, Permissions={tsmPerms.Visible}");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // ? NUEVO: Obtener permisos usando extension method
            List<Patent> userPermissions = currentUser.GetAllAtomicPermissions();
            
            // Log detallado solo en ambiente de test
            if (NativeMethods.testEnvironment)
            {
                Console.WriteLine($"\n=== User Permissions for '{currentUser.Name}' ===");
                Console.WriteLine($"Total permissions: {userPermissions.Count}");
                
                List<Family> roles = currentUser.GetRoles();
                Console.WriteLine($"Roles: {string.Join(", ", roles.Select(r => r.Name))}");
                
                Console.WriteLine("\nPermissions:");
                foreach (var permission in userPermissions)
                {
                    Console.WriteLine($"  - {permission.Name}");
                }
                Console.WriteLine("=====================================\n");
            }
            else
            {
                // Log simple en producción
                Logger.Current.Info(
                    $"User '{currentUser.Name}' loaded main form with {userPermissions.Count} permissions");
            }
        }

        private void tsmUsers_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access User Management");
            
            // ? NUEVO: Verificar permiso antes de abrir
            if (!UIPermissionHelper.CanAccessForm(
                currentUser, 
                PermissionNames.ViewUserManagement,
                "User Management"))
            {
                return; // Mensaje de error ya mostrado por helper
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened User Management module");
            
            ctrlUsers userManagement = new ctrlUsers();
            showContent(userManagement);
        }

        private void tsmPerms_Click(object sender, EventArgs e)
        {
            Logger.Current.Info($"User '{currentUser.Name}' attempting to access Permission Management");
            
            // ? NUEVO: Verificar permiso antes de abrir
            if (!UIPermissionHelper.CanAccessForm(
                currentUser, 
                PermissionNames.ViewPermissionManagement,
                "Permission Management"))
            {
                return;
            }

            Logger.Current.Info($"User '{currentUser.Name}' opened Permission Management module");
            
            ctrlPermsissions permissionManagement = new ctrlPermsissions();
            showContent(permissionManagement);
        }

        private void showContent(UserControl newContent)
        {
            this.MinimumSize = new Size(0, 0);
            panelContainerMain.Controls.Clear();
            newContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelContainerMain.Controls.Add(newContent);
            newContent.BringToFront();
        }

        public void ResetMainPanelSize()
        {
            this.MinimumSize = new Size(800, 600);
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        public User CurrentUser => currentUser;
    }
}
```

---

## ?? **COMPARACIÓN DE CAMBIOS**

| Aspecto | Antes | Después |
|---------|-------|---------|
| **Verificación de permisos** | ? Ninguna | ? En cada menú |
| **Visibilidad de menús** | ? Todos visibles | ? Según permisos |
| **Logging** | ? Solo Console.WriteLine | ? Logger completo |
| **Código frmMain_Load** | ? 15 líneas complejas | ? 5 líneas simples |
| **Obtención de permisos** | ? Loop manual | ? Extension method |
| **Manejo de errores** | ? Sin validación | ? Mensajes claros |
| **Seguridad** | ? Baja | ? Alta |
| **Mantenibilidad** | ? Media | ? Alta |

---

## ? **CHECKLIST DE IMPLEMENTACIÓN**

### **Paso 1: Agregar Using Statements**
```csharp
using Services.Contracts.Logs;
using UI.Helpers;
```

### **Paso 2: Modificar Constructor**
- ? Agregar llamada a `ConfigurePermissions()`
- ? Agregar logging

### **Paso 3: Crear Método ConfigurePermissions**
- ? Configurar visibilidad de `tsmUsers`
- ? Configurar visibilidad de `tsmPerms`

### **Paso 4: Actualizar frmMain_Load**
- ? Usar `currentUser.GetAllAtomicPermissions()`
- ? Agregar logging condicional

### **Paso 5: Actualizar Event Handlers**
- ? Agregar verificación en `tsmUsers_Click`
- ? Agregar verificación en `tsmPerms_Click`

### **Paso 6: Eliminar Código No Usado**
- ? Eliminar `patentsDict` si no se usa
- ? Eliminar `getPatents()` si no se usa

### **Paso 7: Compilar y Probar**
- ? Verificar compilación
- ? Probar con usuario con permisos
- ? Probar con usuario sin permisos

---

## ?? **TESTING**

### **Test 1: Usuario con Todos los Permisos (Administrator)**
```
1. Login como "admin"
2. Abrir frmMain
3. Verificar: tsmUsers VISIBLE
4. Verificar: tsmPerms VISIBLE
5. Click en tsmUsers ? Debe abrir sin error
6. Click en tsmPerms ? Debe abrir sin error
```

### **Test 2: Usuario con Permisos Parciales**
```
1. Login como "manager" (solo ViewUserManagement)
2. Abrir frmMain
3. Verificar: tsmUsers VISIBLE
4. Verificar: tsmPerms OCULTO
5. Click en tsmUsers ? Debe abrir sin error
```

### **Test 3: Usuario sin Permisos**
```
1. Login como "guest" (sin permisos)
2. Abrir frmMain
3. Verificar: tsmUsers OCULTO
4. Verificar: tsmPerms OCULTO
5. Menú debe estar vacío o con pocas opciones
```

---

## ?? **BENEFICIOS DE LOS CAMBIOS**

### **Seguridad**
- ? Verifica permisos antes de abrir módulos
- ? Oculta opciones no permitidas
- ? Loguea intentos de acceso

### **Experiencia de Usuario**
- ? UI limpia (solo ve lo que puede usar)
- ? Mensajes de error claros
- ? No confusión con opciones deshabilitadas

### **Mantenibilidad**
- ? Código más limpio y legible
- ? Fácil agregar nuevos menús
- ? Usa helpers reutilizables

### **Auditoría**
- ? Logging completo de accesos
- ? Trazabilidad de acciones
- ? Detección de intentos no autorizados

---

## ?? **NOTAS IMPORTANTES**

1. **No Rompe Funcionalidad Existente**
   - El código anterior sigue funcionando
   - Solo agregamos validaciones y mejoras

2. **Fácil de Revertir**
   - Si algo falla, es fácil volver atrás
   - Cambios son aditivos, no destructivos

3. **Preparado para el Futuro**
   - Fácil agregar más menús
   - Patrón establecido para nuevos módulos

4. **Compatible con Base de Datos**
   - No requiere cambios en BD
   - Usa estructura existente

---

## ?? **PRÓXIMO PASO**

¿Quieres que implemente estos cambios directamente en `frmMain.cs`?

Puedo:
1. ? Aplicar todos los cambios automáticamente
2. ? Verificar que compile
3. ? Crear versión de backup del archivo original

Solo dime "sí, aplica los cambios" y lo hago inmediatamente.
