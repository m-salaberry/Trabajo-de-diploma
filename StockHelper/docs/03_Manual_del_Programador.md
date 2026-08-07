# StockHelper — Manual del Programador

> Versión del documento: 1.0
> Plataforma: .NET 8 · C# · Windows Forms
> Base de datos: Microsoft SQL Server (dos bases: `iam_db` y `core_db`)

---

## 1. Introducción

Este manual describe la **arquitectura interna** de StockHelper, sus capas,
patrones de diseño, componentes transversales y las guías para **mantener y
extender** el sistema. Está dirigido a desarrolladores.

Al final (§12) se incluye la guía para generar la **referencia de API** (estilo
MSDN) con **Sandcastle Help File Builder (SHFB)** a partir de los comentarios XML
del código.

---

## 2. Visión general de la arquitectura

StockHelper es una aplicación de escritorio con **arquitectura en capas**
(5 proyectos):

```
┌─────────────────────────────────────────────┐
│                     UI                        │  WinForms (net8.0-windows)
│      (Forms, UserControls, navegación)        │
└───────────────┬───────────────────────────────┘
                │ referencia
┌───────────────▼───────────────┐
│              BLL               │  Lógica de negocio + validaciones + auditoría
└───────────────┬───────────────┘
                │
┌───────────────▼───────────────┐
│              DAL               │  Acceso a datos (ADO.NET) → core_db
└───────────────┬───────────────┘
                │
┌───────────────▼───────────────┐
│            Domain              │  Entidades de negocio (POCOs)
└────────────────────────────────┘

        ┌──────────────────────────────────┐
        │            Services              │  Capa transversal:
        │  crypto · logging · i18n ·       │  - servicios comunes
        │  excepciones · IDENTIDAD Y       │  - stack completo de identidad/
        │  PERMISOS (con su propio DAL      │    permisos con su propio acceso
        │  contra iam_db)                   │    a datos (iam_db)
        └──────────────────────────────────┘
         (todas las capas referencian Services)
```

**Flujo de referencias:** `UI → BLL → DAL → Domain`, y **todas** las capas
referencian **Services**.

### 2.1 Proyectos

| Proyecto | Framework | Tipo | Dependencias NuGet | Referencias |
|---|---|---|---|---|
| **Domain** | `net8.0` | Librería | — | — |
| **Services** | `net8.0` | Librería | Microsoft.Data.SqlClient, System.Configuration.ConfigurationManager | — (incluye su propio DAL contra `iam_db`) |
| **DAL** | `net8.0` | Librería | Microsoft.Data.SqlClient, System.Configuration.ConfigurationManager, System.Data.SqlClient | Domain, Services |
| **BLL** | `net8.0` | Librería | — | DAL, Domain, Services |
| **UI** | `net8.0-windows` | WinExe (WinForms) | — | BLL, Domain, Services |

> **Nota:** `Services` es más que "utilidades": aloja **todo el subsistema de
> identidad y permisos** (LoginService, UserService, PermissionService y sus
> repositorios contra `iam_db`). Por eso todas las capas lo referencian.

### 2.2 Dos bases de datos, dos SqlHelper

Existen **dos** clases `SqlHelper` (ambas `internal static`, ambas en el namespace
`DAL.Helpers`):

- `DAL\Helpers\SqlHelper.cs` → usa la cadena de conexión **`coreDb`** (datos de
  negocio).
- `Services\DAL\Helpers\SqlHelper.cs` → usa la cadena **`iamDb`** (identidad).

Ambas resuelven los placeholders `{sqlUser}`/`{sqlPassword}` con los valores de
`appSettings` en su constructor estático.

---

## 3. Capa Domain

Entidades de negocio como POCOs (namespace `Domain`). La mayoría tiene el `set` de
`Id` **privado**, obligando a los repositorios a asignarlo por reflexión.

| Clase | Descripción |
|---|---|
| `Product` | Producto terminado: `Id`, `Name`, `Code`, `List<DetailProduct>` (lista de materiales). |
| `DetailProduct` | Línea de receta: `Item` + `QuantityToConsume`. |
| `Item` | Insumo de inventario: `Id (Guid)`, `Name`, `Unit`, `Category`, `Stock`, `LastUpdate`; `IsUnitInteger()`. |
| `ItemsCategory` | Categoría de ítems: `Id`, `Name`. |
| `Provider` | Proveedor: `Id (Guid)`, `CUIT`, `Name`, `Category`, `CompanyName`, `ContactTel`, `Email`. |
| `ReplacementOrder` | Orden de reposición: `Id`, `ReplacementOrderNumber`, `Provider`, `List<OrderRow>`; genera número `REP-yyyyMM-<cuit>-NNN`. |
| `OrderRow` | Renglón de orden: `ReplacementOrder`, `Item`, `Quantity`. |
| `PurchaseOrder` | Orden de compra: `Id`, `ReplacementOrder`, `Status`, `BillFilePath`, `TotalAmount`, `IssuedDate`. |
| `Enums\PurchaseOrderStatus` | Constantes de estado: `SentToProvider`, `BillReceived`, `Cancelled`. |

> Las entidades del **modelo de seguridad** (User, Family, Patent, Component) viven
> en **`Services\Domain\`** (ver §6.2), no en Domain.

---

## 4. Capa DAL (acceso a datos de `core_db`)

- **`Contracts\IRepository.cs`** — contrato genérico `IRepository<T, TId>` con
  `Create / Update / Delete / GetById / GetAll`.
- **`Helpers\SqlHelper.cs`** — helper ADO.NET (`ExecuteNonQuery`, `ExecuteScalar`,
  `ExecuteReader` con `CommandBehavior.CloseConnection`, `CheckNullables`).
  Los errores se enrutan por `DALExceptionHandler` y se relanzan como
  `MySystemException(.., "DAL", ex)`.
- **Repositorios** (ADO.NET parametrizado): `ItemRepository`,
  `ItemsCategoryRepository`, `ProductRepository`, `DetailProductRepository`,
  `ProviderRepository`, `OrderRowRepository`, `ReplacementOrderRepository`,
  `PurchaseOrderRepository`.
  - **`ProviderRepository`** es el punto de cifrado: en `Create`/`Update` llama a
    `CryptographyService.ProtectField(...)` para CUIT/ContactTel/Email, y en
    `MapToEntity` llama a `UnprotectField(...)`.
- **`DatabaseBackupService`** — singleton, respaldo diario (REQ.006). Ver §8.4.

---

## 5. Capa BLL (lógica de negocio)

- **`IGenericBllService<T,TId>`** + **`GenericBllService<T,TId>`** — CRUD base que
  envuelve un `IRepository<T,TId>` con validaciones de null / id vacío; `Delete`
  verifica existencia primero.
- Servicios de dominio (**singletons** que heredan de `GenericBllService`, agregan
  validaciones y logging `[AUDIT]`):

| Servicio | Responsabilidades destacadas |
|---|---|
| `ProviderService` | Valida nombre, razón social, teléfono, **CUIT = 11 dígitos** (`IsValidCUIT`), categoría; bloquea borrado con órdenes activas; `GetProvidersByCategory`. |
| `ItemService` | `AddStock`, `ReduceStock`; `GetItemsByCategory`, `GetLowStockItems(10)`, `GetOutOfStockItems`. |
| `ItemsCategoryService` | CRUD de categorías. |
| `ProductService` | `GetProductsUsingItem(Guid)`. |
| `ReplacementOrderService` | `GetReplacementOrdersByProvider`. |
| `PurchaseOrderService` | `ReceiveOrder(...)` (valida estado, adjunta factura, **suma stock** vía `ItemService.AddStock`), `CancelOrder`, `GetActiveOrdersByProvider`. |
| `OrderRowService` | `GetRowsByReplacementOrderId`. |
| `AnalyticsService` | `GetStatsByCategory`, `GetStatsByProvider` (agrega compras no canceladas). |
| `closeShiftProcessorService` | Parsea el reporte de cierre de turno (ancho fijo) y calcula consumo por ítem según la receta de cada producto. |
| `BackupService` | Wrapper de `DAL.DatabaseBackupService` (REQ.006). |
| `EmailMessengerService` / `WhatsAppMessengerService` | Abren `mailto:` / `https://wa.me/...` con `Process.Start(UseShellExecute=true)`. |

Plantillas de mensajes en `BLL\Templates\` (`EmailMessageTemplates`,
`WhatsAppMessageTemplates`).

---

## 6. Capa Services (transversal + identidad)

### 6.1 Servicios transversales

**Criptografía — `CryptographyService` (static):**

| Función | Uso |
|---|---|
| `HashMd5(string)` | Hash de contraseñas (MD5 sobre UTF-16, hex). Ver nota de seguridad en §11. |
| `Encrypt` / `Decrypt` (legado) | AES con `Rfc2898DeriveBytes` desde `STOCK_HELPER_SECRET_KEY` + salt fija. |
| `ProtectField` / `UnprotectField` (actual) | Cifrado de campos: prefijo `enc:v1:`, AES con IV aleatorio por llamada, claves derivadas por PBKDF2 (SHA256, 10000 iter), verificación HMAC con `FixedTimeEquals`. Passthrough de texto plano legado. |

La clave maestra proviene de la variable de entorno **`STOCK_HELPER_SECRET_KEY`**;
`Program.cs` **aborta el arranque** si no está definida.

**Logging — `Contracts\Logs\`:** `Logger` (singleton `Logger.Current`) con lista de
appenders (`ILogAppender`): `FileAppender` (rotación a 10 MB, 5 backups, nivel
mínimo Info) y `ConsoleAppender`. Niveles en `LogLevels`. `LogReaderService`
(singleton) parsea logs actuales + rotados con regex, une entradas multilínea
(stack traces) y alimenta el módulo de Logs de la UI.

**Internacionalización — `LanguageService` (singleton)** +
**`LanguageRepository`**: `Translate`, `ChangeCulture` (persiste en config y
dispara el evento `LanguageChanged`), `GetAvailableCultures`, `RefreshTranslations`.
Traducciones en archivos `key=value` bajo `UI\I18n\` (`translations.es-ES`,
`translations.en-EN`). Caché por cultura con invalidación por fecha de
modificación de archivo. Documentado en `LANGUAGEREPOSITORY_README.md`.

### 6.2 Subsistema de identidad y permisos

**Modelo (patrón Composite) — `Services\Domain\`:**

- `Component` (abstracto) — base con `Name`, `Id`, `Children`, `AddChild`,
  `RemoveChild`, y `HasPermission(string)` (búsqueda recursiva en el árbol).
- `Patent` — **hoja** (permiso atómico); `AddChild`/`RemoveChild` lanzan
  `LeafComponentException`.
- `Family` — **compuesto** (rol) que contiene Components.
- `User` — `Id`, `Name`, `Password`, `Role`, `IsActive`, `List<Component> Permissions`.
- `PermissionNames` — constantes de los 10 permisos que la app verifica.
- `UserPermissionExtensions` — `HasPermission`, `HasAllPermissions`,
  `HasAnyPermission`, `GetAllAtomicPermissions`, `GetRoles`, `HasRole`.

**Servicios de identidad:**

- `LoginService.Authenticate(user, pass)` — compara `HashMd5(pass)`, verifica
  `IsActive`; lanza `InvalidCredentialsException`.
- `UserService` (singleton) — CRUD sobre `UsersRepository`, hashea contraseñas,
  gestiona la relación usuario↔rol vía `PermissionService`.
- `PermissionService` (singleton, doble verificación de bloqueo) — CRUD de Patents
  y Families, caché (30 min), `GetFamilyByName`, `GetFamilyPatents`, reconcilia
  relaciones patente↔familia en `TransactionScope`, impide borrar familias en uso.

**Acceso a datos de identidad (`Services\DAL\`):** `UsersRepository`,
`FamilyRepository`, `PatentRepository`, `LanguageRepository`, más su propio
`SqlHelper` (contra `iamDb`).

### 6.3 Manejo de excepciones

- Excepciones propias: `InvalidCredentialsException`, `LeafComponentException`,
  `WordNotFoundException`.
- `MySystemException` — lleva la **capa** de origen; su método `.Handler()`
  despacha al manejador de la capa (`UIExceptionHandler`, `BLLExceptionHandler`,
  `DALExceptionHandler`) y registra el error.

> ⚠️ Hay una **inconsistencia de namespace** entre `Services.Contracts.CustomException`
> y `...CustomsException` (ambos aparecen en `using`s). Ver §11.

---

## 7. Capa UI

- **`Program.cs`** — `Main`: inicializa el logger; **falla con MessageBox si falta
  `STOCK_HELPER_SECRET_KEY`**; `Application.Run(new frmLogIn())`.
- **`frmLogIn`** — login; validación de credenciales del lado cliente,
  `LoginService.Authenticate`, dispara `BackupService.RunDailyBackupIfNeeded()`
  (REQ.006) y abre `frmMain`.
- **`frmMain`** — ventana principal singleton (`GetInstance(User)`); `MenuStrip` +
  panel de contenido (`showContent(UserControl)`); `ConfigurePermissions()` oculta
  menús según permisos.

**Infraestructura de traducción:** interfaz `ITranslatable`; clases base
`TranslatableForm` (Form) y `TranslatableUserControls` (UserControl) que se
suscriben a `LanguageService.LanguageChanged` y llaman `ApplyTranslations()`. Cada
formulario/control sobrescribe `ApplyTranslations()`.

**Módulos (`controlForms\`):** `ctrlUsers`, `ctrlPermsissions`,
`ctrlItemsAndCategories`, `ctrlProviders`, `ctrlProductBuilder`, `ctrlStock`,
`ctrlOrders`, `ctrlPurchase`, `ctrlAnalytics`, `ctrlConfiguration`, `ctrlLogs`.

**Diálogos (`secondaryForms\`):** altas/bajas/modificaciones de usuarios, roles,
categorías, ítems, proveedores; `addItemToProductForm`, `chooseProviderForm`,
`cancelPurchaseOrderForm`, `uploadBillToPurchaseOrderForm`, `importShiftUsageFileForm`.

**Navegación (MenuStrip):**
`tsmSystem` (Configuración, Logs) · `tsmUserAndPerms` (Usuarios, Permisos) ·
`tsmCatalogManagment` (Ítems/Categorías, Proveedores, ProductBuilder) ·
`tsmInventoryAndPurchasing` (Stock, Órdenes, Compras, Analíticas) · `tsmLogOut`.

---

## 8. Componentes transversales (detalle)

### 8.1 Modelo de seguridad y verificación de permisos

- Relaciones en `iam_db`: `USERS`, `PATENTS`, `FAMILIES`, `PATENTS_FAMILIES`
  (N:M), `USERS_FAMILIES` (N:M), con FKs en cascada.
- **Carga:** `UsersRepository.GetByName` → `FillUserFamily` →
  `FamilyRepository.FillFamilyPatents`. El `User` logueado queda con su árbol de
  permisos completo.
- **Aplicación en runtime (defensa en profundidad):**
  1. `frmMain.ConfigurePermissions()` fija `Visible` de cada menú según
     `currentUser.HasPermission(PermissionNames.X)`.
  2. Cada handler de módulo llama a **`UIPermissionHelper.CanAccessForm(user,
     permiso, nombre)`** (`UI\Helpers\UIPermissionHelper.cs`): si falta el permiso,
     devuelve false, muestra "Acceso denegado" y registra una advertencia.
  3. `HasPermission` resuelve por el árbol Composite (`Component.HasPermission`).
- `UIPermissionHelper` ofrece además `ShowIfHasPermission`,
  `EnableIfHasPermission`, `CanPerformAction` y el enum `PermissionAction`.

### 8.2 Cifrado de campos de proveedores (REQ.002 / CU.Arq.003-005)

CUIT, ContactTel y Email se cifran en reposo con `ProtectField`/`UnprotectField`
(AES + HMAC, IV aleatorio, prefijo `enc:v1:`). Las columnas se ampliaron a
`NVARCHAR(512)` y **no hay UNIQUE en CUIT** (el cifrado con IV aleatorio es no
determinista). La clave maestra sale de `STOCK_HELPER_SECRET_KEY`.

### 8.3 Internacionalización

Traducciones basadas en archivos `key=value` en `UI\I18n\`. Cultura conmutable en
runtime desde `ctrlConfiguration` y persistida en config. Las claves faltantes se
agregan automáticamente.

### 8.4 Respaldo diario (REQ.006)

`DAL.DatabaseBackupService` (singleton) hace
`BACKUP DATABASE [db] TO DISK=@path WITH INIT, FORMAT, CHECKSUM` (timeout 300s) de
ambas bases. `RunDailyBackupIfNeeded()` omite si el `.bak` más reciente tiene < 24 h;
`ApplyRetention` borra `.bak` de más de `backupRetentionDays` días (14 por
defecto). Se dispara al iniciar sesión (`BLL.BackupService`). Todos los errores se
registran y se tragan (nunca bloquean el login).

---

## 9. Configuración y entorno

**`UI\App.config`** (se publica como `UI.dll.config`):

- `connectionStrings`: `iamDb` (Catalog `iam_db`) y `coreDb` (Catalog `core_db`),
  servidor `.\SQLEXPRESS`, placeholders `{sqlUser}`/`{sqlPassword}`,
  `Trust Server Certificate=True`.
- `appSettings`: `sqlUser`, `sqlPassword`, `logFileDirectory`, `backupDirectory`,
  `backupRetentionDays`, `LanguageFolderPath`, `LanguageFileName`, `Culture`.

**Variable de entorno (fuera del config):** `STOCK_HELPER_SECRET_KEY` —
**obligatoria** (clave maestra de cifrado). Sin ella la app no inicia.

---

## 10. Compilación, publicación y base de datos

### 10.1 Compilar / publicar

Publicar **con el SDK de .NET** (no con el MSBuild de otras herramientas como SSMS,
que provoca el error `MSB4247`). Script incluido:

```powershell
powershell -ExecutionPolicy Bypass -File .\build-installer-input.ps1
```

Genera `installer-input\` (publicación *framework-dependent*: el equipo destino
necesita el **.NET 8 Desktop Runtime**).

### 10.2 Scripts de base de datos

Los scripts que reflejan el **estado actual** de la BBDD están en
`StockHelper-Installer-DB\` (ver su `00_README.md`), con orden de ejecución
`01 → 02 → 03 → 04`. Para regenerarlos si el esquema cambia, usar los scripts
`_regenerar_*.ps1` de esa carpeta.

> La fuente autoritativa del esquema es la carpeta `StockHelper-Installer-DB\`
> (extraída del estado real de la base). Los scripts y READMEs antiguos de
> `Database\` fueron eliminados por estar desactualizados.

---

## 11. Deuda técnica y notas de seguridad

Puntos a tener en cuenta al mantener el sistema:

1. **Contraseñas con MD5** (`CryptographyService.HashMd5`) — MD5 no es apto para
   contraseñas. Recomendado migrar a **BCrypt** o **PBKDF2** (el propio
   `DATABASE_SCHEMA_README.md` lo sugiere).
2. **Contraseña de BBDD en texto plano** en `App.config` (`sqlPassword`). Restringir
   permisos de lectura del `.config` en el equipo destino.
3. **`STOCK_HELPER_SECRET_KEY` obligatoria** — sin ella la app no arranca; su valor
   debe permanecer estable (si cambia, los datos cifrados quedan ilegibles).
4. **Permisos vigentes = los 10 de `PermissionNames`.** (Existía documentación
   legada con un catálogo de 20 permisos / 9 roles que no coincidía con el código;
   fue eliminada para evitar confusiones.)
5. **Inconsistencia de namespace** `CustomException` vs `CustomsException`.
6. **Dependencia MSAL/Identity sobrante**: `frmLogIn.cs` importa
   `Microsoft.Identity.Client` y la publicación arrastra `Microsoft.Identity.Client.dll`
   / Azure.Identity, pero no hay uso real de MSAL en el código. Se puede limpiar.
7. **`ProviderService.ValidateCuitUniqueness`** existe pero no se invoca en
   Insert/Update (la unicidad de CUIT ya no se garantiza a nivel BD por el cifrado).

---

## 12. Referencia de API con Sandcastle Help File Builder (SHFB)

La **referencia de API** (documentación de clases, métodos y parámetros estilo
MSDN) se genera automáticamente a partir de los **comentarios XML `///`** del
código. Herramienta recomendada: **Sandcastle Help File Builder (SHFB)**.

### 12.1 Requisito previo: activar la generación de XML docs

Cada proyecto de librería debe emitir su archivo `.xml` de documentación. Esto ya
quedó activado en los `.csproj` con:

```xml
<PropertyGroup>
  <GenerateDocumentationFile>true</GenerateDocumentationFile>
  <!-- 1591: no exigir comentario XML en cada miembro público -->
  <NoWarn>$(NoWarn);1591</NoWarn>
</PropertyGroup>
```

Al compilar en Release, se genera `Domain.xml`, `DAL.xml`, `BLL.xml`,
`Services.xml`, `UI.xml` junto a cada `.dll`.

### 12.2 Instalar SHFB

1. Descargar **Sandcastle Help File Builder** desde su repositorio oficial
   (proyecto *EWSoftware/SHFB* en GitHub) e instalarlo.
2. (Opcional) Instalar la extensión de Visual Studio de SHFB para editar el
   proyecto de ayuda desde el IDE.

### 12.3 Crear el proyecto de documentación

1. Abrir **SHFB** → *New Project* (se crea un archivo `.shfbproj`).
   Guardarlo, por ejemplo, en `docs\api\StockHelper.shfbproj`.
2. En **Project Properties → Documentation Sources**, agregar las **DLL** y sus
   **.xml** compilados en Release:
   ```
   BLL\bin\Release\net8.0\BLL.dll        (+ BLL.xml)
   DAL\bin\Release\net8.0\DAL.dll        (+ DAL.xml)
   Services\bin\Release\net8.0\Services.dll (+ Services.xml)
   Domain\bin\Release\net8.0\Domain.dll  (+ Domain.xml)
   UI\bin\Release\net8.0-windows\UI.dll  (+ UI.xml)
   ```
   (Alternativamente, se puede agregar directamente `StockHelper.sln` como
   *Documentation Source* y SHFB toma todos los proyectos.)
3. En **Build** configurar:
   - **Help File Format:** `Website` (HTML navegable) y/o `HTML Help 1` (`.chm`).
   - **Framework version:** .NET Core/.NET 5+ (`.NETCore`), acorde a net8.0.
   - **Namespace filters:** incluir `Domain`, `DAL`, `BLL`, `Services`, `UI` y sus
     subnamespaces.
4. En **Namespace Summaries**, escribir una breve descripción de cada namespace
   (aparece en la portada de la referencia).

### 12.4 Generar

- Compilar la solución en **Release** (para tener DLL + XML actualizados).
- En SHFB, **Build the help file**. El resultado (sitio HTML o `.chm`) queda en la
  carpeta de salida configurada (por defecto `Help\`).

### 12.5 Buenas prácticas de comentarios XML

Para que la referencia salga rica, documentar los miembros públicos con:

```csharp
/// <summary>Describe qué hace el método.</summary>
/// <param name="id">Qué representa el parámetro.</param>
/// <returns>Qué devuelve.</returns>
/// <exception cref="MySystemException">Cuándo se lanza.</exception>
```

El código ya tiene comentarios XML en varios componentes (p. ej.
`CryptographyService`, `SqlHelper`, interfaces genéricas); completar los faltantes
mejora la referencia generada.

### 12.6 Alcance

SHFB cubre la **referencia técnica de la API** (el "qué" de cada clase/método). La
**visión conceptual** (arquitectura, patrones, flujos) es este mismo documento.
Ambas partes componen el manual del programador completo.

---

*Fin del Manual del Programador.*
