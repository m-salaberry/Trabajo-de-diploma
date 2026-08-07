# StockHelper — Manual de Instalación

> Versión del documento: 2.1
> Aplicación: StockHelper (aplicación de escritorio Windows, .NET 8 / WinForms)
> Motor de base de datos: Microsoft SQL Server Express — **instancia dedicada `STOCKHELPER`**

> **⚠️ Cambio importante en la v2.1 — dos pasos:** el instalador **solo instala el
> motor de SQL Server** (la instancia dedicada `STOCKHELPER`), la aplicación y la
> variable de entorno de cifrado. **No crea las bases de datos.** Cuando el
> instalador termina, hay que ejecutar **una sola vez** el script
> `Crear-Bases-StockHelper.bat` (carpeta **`StockHelper-Crear-Bases\`**, que se
> entrega junto al instalador) para dar de alta las bases `iam_db` y `core_db`,
> el login de la app y el usuario `admin`. Este diseño de dos pasos es
> deliberado: separa la instalación del motor (compleja y silenciosa) de la
> creación de las bases (rápida, verificable y con mensajes claros de éxito/error).

---

## 1. Introducción

Este manual describe cómo instalar y poner en funcionamiento **StockHelper** en un
equipo con Windows, incluyendo el motor de base de datos, la creación e
inicialización de las bases de datos, y la primera puesta en marcha.

Se documentan **dos vías** de instalación:

- **A) Instalación con el instalador** (Advanced Installer) — recomendada para el
  usuario final. Es un paquete **offline** que incluye el runtime, SQL Express y la
  app. **Tras instalar**, se corre el script de alta de bases (§5.6).
- **B) Instalación manual** (línea de comandos con `sqlcmd`) — útil para pruebas,
  para entornos de desarrollo o si no se dispone de la edición de Advanced
  Installer con soporte de prerequisitos/SQL.

> **Instancia dedicada `STOCKHELPER`:** el instalador crea una **instancia privada y
> dedicada de SQL Server llamada `STOCKHELPER`** (en lugar de la instancia genérica
> `SQLEXPRESS`). Como el instalador es dueño de esa instancia, siempre la crea en
> modo mixto, con su clave `sa`, **sin chocar con otras instalaciones de SQL Server
> que ya existan** en el equipo. Sobre esa instancia recién creada, el script de
> alta de bases (§5.6) da de alta `iam_db` y `core_db`.

---

## 2. Requisitos del sistema

### 2.1 Hardware mínimo

| Recurso | Mínimo recomendado |
|---|---|
| Procesador | x64, 1.4 GHz o superior |
| Memoria RAM | 4 GB (2 GB para SQL Server Express + la app) |
| Disco | 6 GB libres (SQL Server Express ~2 GB + app + datos + respaldos) |
| Pantalla | 1366×768 o superior |

### 2.2 Software

| Componente | Detalle |
|---|---|
| Sistema operativo | Windows 10/11 x64 (o Windows Server equivalente) |
| **.NET Desktop Runtime 8** | Requerido para ejecutar la app. La publicación es *framework-dependent*; el instalador lo incluye e instala si falta. |
| **SQL Server Express** | Motor de base de datos. El instalador crea una **instancia dedicada `STOCKHELPER`**, en **modo mixto**. |
| **Variable `STOCK_HELPER_SECRET_KEY`** | Variable de entorno **de máquina** obligatoria (clave maestra de cifrado). La aplicación **no arranca** sin ella; la define el instalador. |
| Permisos | Cuenta con privilegios de **administrador** para la instalación. |

> **Importante:** la aplicación usa WinForms sobre `net8.0-windows`, por lo tanto
> se necesita el **.NET Desktop Runtime** (no basta el runtime base de consola).

---

## 3. Componentes que se instalan

**El instalador (`StockHelper.exe`) instala:**

1. **.NET 8 Desktop Runtime** (si no está presente).
2. **SQL Server Express**, **instancia dedicada `STOCKHELPER`**, autenticación **mixta** (solo el motor, sin bases).
3. **Archivos de la aplicación** (ejecutable `UI.exe` y dependencias).
4. **Variable de entorno de máquina `STOCK_HELPER_SECRET_KEY`** (clave maestra para el cifrado de campos sensibles de proveedores).
5. Acceso directo a la aplicación.

**El script de alta de bases (`Crear-Bases-StockHelper.bat`, se ejecuta después — §5.6) crea:**

6. **Dos bases de datos** sobre la instancia `STOCKHELPER`:
   - `iam_db` — autenticación, usuarios, roles y permisos.
   - `core_db` — datos de negocio (inventario, productos, órdenes, proveedores).
7. **Login de base de datos** `stock_helper_user` con el que se conecta la app.
8. **Usuario administrador inicial** de la aplicación (`admin` / `admin`).

---

## 4. Conceptos clave

### 4.1 El "modo mixto" de SQL Server

SQL Server puede autenticar de dos formas:

| Modo | Quién puede conectarse |
|---|---|
| Windows Authentication (por defecto) | Solo cuentas de Windows |
| **Mixed Mode** (SQL + Windows) | Cuentas de Windows **y** logins SQL propios (usuario + contraseña internos de SQL Server) |

StockHelper se conecta con un **login SQL** (`stock_helper_user` + contraseña), que
**no** es una cuenta de Windows. Por eso el motor **debe** quedar instalado en
**modo mixto**; de lo contrario la aplicación fallará con
*"Login failed for user 'stock_helper_user'"*.

El modo mixto se activa al instalar el motor con el parámetro
`/SECURITYMODE=SQL` (y `/SAPWD` para la contraseña del usuario `sa`).

### 4.2 La instancia dedicada `STOCKHELPER`

Una **instancia** de SQL Server es una instalación independiente del motor, con su
propio nombre, sus servicios, sus logins y sus bases. En un mismo equipo pueden
convivir varias (por ejemplo `SQLEXPRESS`, `MSSQLSERVER`, etc.).

StockHelper usa una instancia **propia y exclusiva** llamada `STOCKHELPER`
(se accede como `.\STOCKHELPER`). Ventajas:

- El instalador **la crea siempre de cero** en modo mixto, con la clave `sa` y las
  bases que necesita, **sin depender** de otra instancia que ya exista.
- Evita conflictos con un SQL Server preinstalado (que podría estar en modo Windows,
  tener otra clave, u otras bases).

---

## 5. Vía A — Instalación con el instalador (Advanced Installer)

> El paso 5.3 (prerequisito de SQL Server) requiere **Advanced Installer
> Professional o superior** (los prerequisitos embebidos). La creación de las bases
> ya **no** se hace dentro del instalador: es un paso posterior con un script
> (§5.6), por lo que **no** se necesita la página *SQL Databases* ni la edición
> Architect.

### 5.1 Preparar la salida de la aplicación

La aplicación se publica con el SDK de .NET (no con el MSBuild de otras
herramientas). Desde la raíz del proyecto:

```powershell
powershell -ExecutionPolicy Bypass -File .\build-installer-input.ps1
```

Esto genera la carpeta `installer-input\` con `UI.exe` y todas sus dependencias.
El `UI.dll.config` resultante queda apuntando a `Data Source=.\STOCKHELPER`.

### 5.2 Prerequisito: .NET 8 Desktop Runtime

En Advanced Installer → página **Prerequisites** → agregar el prerequisito
predefinido **".NET Desktop Runtime 8.x (x64)"**, **embebido** en el paquete
(instalación offline). Marcarlo para que se instale antes de la aplicación.

### 5.3 Prerequisito: SQL Server Express (instancia `STOCKHELPER`, modo mixto)

1. Descargar el instalador **completo/offline** de SQL Server Express (p. ej.
   `SQLEXPR_x64_ENU.exe`, versión en inglés) y guardarlo junto al proyecto.
2. En **Prerequisites** → *New Prerequisite → Package* → seleccionar ese `.exe`,
   **embebido** en el paquete.
3. En **la línea de comandos** usar exactamente lo siguiente. **Importante:** hay
   que ponerla en los **tres** campos de línea de comandos del prerequisito
   (*Full UI*, *Basic UI* y *Silent / No UI*), idénticas — si solo se completa el
   campo *Silent*, al ejecutar el setup con interfaz se abriría el "SQL Server
   Installation Center" de forma interactiva.

```
/QS /ENU /ACTION=Install /FEATURES=SQLENGINE /INSTANCENAME=STOCKHELPER /SECURITYMODE=SQL /SAPWD="<CLAVE_SA>" /SQLSVCACCOUNT="NT AUTHORITY\SYSTEM" /SQLSYSADMINACCOUNTS="BUILTIN\Administrators" "NT AUTHORITY\SYSTEM" /TCPENABLED=1 /IACCEPTSQLSERVERLICENSETERMS
```

| Parámetro | Función |
|---|---|
| `/INSTANCENAME=STOCKHELPER` | Crea la **instancia dedicada** `STOCKHELPER` (coincide con `.\STOCKHELPER` del config) |
| `/SECURITYMODE=SQL` | **Activa el modo mixto** |
| `/SAPWD="..."` | Contraseña del usuario `sa` (ver nota sobre caracteres) |
| `/SQLSYSADMINACCOUNTS="BUILTIN\Administrators"` | Da rol **sysadmin** al grupo Administradores locales |
| `/ENU` | Fuerza la instalación en inglés (coherente con el paquete `SQLEXPR_x64_ENU.exe`) |
| `/IACCEPTSQLSERVERLICENSETERMS` | Acepta la licencia (obligatorio en modo silencioso) |

> ℹ️ **Por qué `BUILTIN\Administrators` en los sysadmin:** el script de alta de
> bases (§5.6) se ejecuta **como administrador** y conecta a SQL con **autenticación
> de Windows**. Para que pueda hacer `CREATE DATABASE` / `CREATE LOGIN`, el grupo
> Administradores locales debe ser **sysadmin** de la instancia — eso lo garantiza
> `/SQLSYSADMINACCOUNTS="BUILTIN\Administrators"`. (Como la instancia `STOCKHELPER`
> la crea el propio instalador de cero, esto siempre queda bien configurado.)

> ⚠️ **Contraseña de `sa` — usar solo alfanuméricos:** en Advanced Installer 18.8.1
> los caracteres especiales en la contraseña SQL **no se escapan** correctamente
> (se corrige recién en AI 19). Usá una contraseña **alfanumérica** (letras y
> números), fuerte, sin comillas ni símbolos. Debe cumplir la política de complejidad
> de Windows. Reemplazá `<CLAVE_SA>` por la contraseña que elijas: el valor real está
> configurado en el `.aip` (campo `/SAPWD` del prerequisito de SQL) y no se publica acá.

#### 5.3.1 Detección del prerequisito (para no reinstalar SQL indebidamente)

Para que el instalador **detecte específicamente la instancia `STOCKHELPER`** (y no
cualquier otro SQL Server que ya haya en el equipo), configurar la condición de
detección del prerequisito como **"Registry value exists"**:

- **Key:** `HKLM\SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL`
- **Value:** `STOCKHELPER`
- Búsqueda **64-bit**.

> Si se dejara la detección como "Registry key exists" (que la key exista), el
> instalador consideraría "SQL ya instalado" ante **cualquier** instancia
> preexistente y **saltearía** la instalación de la instancia dedicada — que es la
> causa raíz del problema que este diseño resuelve.

### 5.4 Archivos de la aplicación

En **Files and Folders** → *Add Folder* → seleccionar `installer-input\`.
Luego crear un acceso directo a `UI.exe` (escritorio / menú inicio).

### 5.5 Variable de entorno de cifrado (obligatoria)

La aplicación cifra los campos sensibles de proveedores (CUIT, teléfono, email)
usando una clave maestra que lee de la variable de entorno
`STOCK_HELPER_SECRET_KEY`. **Si la variable no existe, la app se cierra al
iniciar** (lo verifica `Program.cs`).

**Método recomendado — página *Environment* de Advanced Installer.** Es el enfoque
usado por el instalador actual: no depende de scripts, políticas de ejecución de
PowerShell, timing ni elevación de un Custom Action; lo maneja el propio MSI.

En Advanced Installer → página **Environment** → *New Environment Variable*:

| Campo | Valor |
|---|---|
| **Name** | `STOCK_HELPER_SECRET_KEY` |
| **Value** | la clave maestra fija (ver nota) |
| **Behavior** | *Set* (crear/actualizar) |
| **Install / Uninstall** | crear al instalar; opcionalmente quitar al desinstalar |
| **Scope** | **System / Machine** (no de usuario) |

> ⚠️ **Clave fija, estable y única en todas las instalaciones.** Los datos ya
> cifrados con una clave **no** se pueden descifrar con otra. Usá siempre la misma
> clave y guardala en un lugar seguro. El valor usado en este proyecto es una clave
> de 256 bits en Base64 (por ejemplo, la constante que también trae el script
> `set-secret-key.ps1`).

> **Nota:** en versiones anteriores del instalador esta variable se fijaba con un
> *Custom Action* que ejecutaba el script `set-secret-key.ps1`. Ese enfoque quedó
> **obsoleto** y fue reemplazado por la página *Environment* nativa. El script
> `set-secret-key.ps1` se conserva **solo** como herramienta manual para la Vía B
> (ver §6.4); **ya no se empaqueta** en el instalador.

### 5.6 Paso final (obligatorio): crear las bases de datos

> **Este paso se hace UNA SOLA VEZ, cuando el instalador ya terminó por completo.**
> El instalador dejó el motor `STOCKHELPER` y la app, pero **las bases de datos
> todavía no existen**. Si abrís StockHelper antes de este paso, no podrá conectarse.

Junto al instalador se entrega la carpeta **`StockHelper-Crear-Bases\`**, con:

| Archivo | Para qué |
|---|---|
| **`Crear-Bases-StockHelper.bat`** | **Lo que ejecuta el usuario** (doble clic). Se eleva a administrador solo. |
| `Crear-Bases-StockHelper.ps1` | Motor real: ejecuta los 4 scripts vía PowerShell (no necesita `sqlcmd`). |
| `01_login_and_databases.sql` | Login `stock_helper_user` + bases `iam_db`/`core_db` + `db_owner`. |
| `02_iam_db_schema.sql` | Esquema de `iam_db`. |
| `03_iam_db_seed.sql` | Permisos + rol Administrator + usuario `admin`. |
| `04_core_db_schema.sql` | Esquema de `core_db` (vacío). |
| `LEEME.txt` | Instrucciones para el usuario final. |

**Pasos:**

1. Confirmar que el instalador **ya terminó** (incluida la instalación de SQL Express).
2. **Doble clic** en `Crear-Bases-StockHelper.bat`. Aceptar el aviso de permisos de
   administrador (es necesario: conecta a SQL con autenticación de Windows y crea
   las bases).
3. Esperar el mensaje **"LISTO. Las bases de datos se crearon correctamente."**

> **Cómo funciona:** el `.ps1` abre **una sola conexión** de Windows a
> `.\STOCKHELPER` (usando `System.Data.SqlClient`, incluido en Windows — **no
> depende de que `sqlcmd` esté instalado**) y corre los 4 scripts en orden,
> partiéndolos por los separadores `GO`. Cada script es **autocontenido** (empieza
> con su propio `USE`). El login SQL de la app se crea con `CHECK_POLICY=OFF`. Es
> **idempotente**: se puede volver a ejecutar sin romper nada.

> **Instancia con otro nombre.** Si por algún motivo la instancia no se llama
> `STOCKHELPER`, ejecutar desde una consola **como administrador**:
> ```powershell
> powershell -ExecutionPolicy Bypass -File Crear-Bases-StockHelper.ps1 -Server ".\NOMBRE_INSTANCIA"
> ```

### 5.7 Orden de ejecución resultante

```
Instalador (StockHelper.exe):
  1) .NET 8 Desktop Runtime            (prerequisito, embebido)
  2) SQL Server Express modo mixto     (prerequisito, embebido — instancia STOCKHELPER, SOLO el motor)
  3) Copia de archivos                 (installer-input\)
  4) Variable STOCK_HELPER_SECRET_KEY  (página Environment, a nivel de máquina)
  5) Acceso directo a UI.exe

Paso posterior, a cargo del usuario (§5.6):
  6) Crear-Bases-StockHelper.bat  → scripts SQL 01 → 02 → 03 → 04 sobre .\STOCKHELPER
```

### 5.8 Generar el instalador

Recién con todo lo anterior configurado → botón **Build**. El resultado es un
`StockHelper.exe` (setup **offline** con runtime, SQL Express y la app embebidos).
**Entregar junto a él la carpeta `StockHelper-Crear-Bases\`** (§5.6), que no va
dentro del `.exe`.

---

## 6. Vía B — Instalación manual (sqlcmd)

Para entornos de desarrollo/prueba, o sin la página SQL de Advanced Installer.

### 6.1 Instalar el motor en modo mixto

Ejecutar el instalador de SQL Server Express con los mismos argumentos del
punto 5.3 (desde una consola como administrador). Podés instalar la instancia
dedicada `STOCKHELPER` para replicar producción, o usar `SQLEXPRESS` en desarrollo
(ajustando el config en consecuencia, §7.1):

```powershell
.\SQLEXPR_x64_ENU.exe /QS /ENU /ACTION=Install /FEATURES=SQLENGINE /INSTANCENAME=STOCKHELPER /SECURITYMODE=SQL /SAPWD="<CLAVE_SA>" /SQLSVCACCOUNT="NT AUTHORITY\SYSTEM" /SQLSYSADMINACCOUNTS="BUILTIN\Administrators" /TCPENABLED=1 /IACCEPTSQLSERVERLICENSETERMS
```

### 6.2 Ejecutar los scripts de base de datos

Conectado como administrador (autenticación de Windows, ya sysadmin), ejecutar
**en orden**:

```powershell
cd StockHelper-Installer-DB
sqlcmd -S .\STOCKHELPER -E -C -b < 01_login_and_databases.sql
sqlcmd -S .\STOCKHELPER -E -C -b < 02_iam_db_schema.sql
sqlcmd -S .\STOCKHELPER -E -C -b < 03_iam_db_seed.sql
sqlcmd -S .\STOCKHELPER -E -C -b < 04_core_db_schema.sql
```

| Opción | Significado |
|---|---|
| `-S .\STOCKHELPER` | Servidor e instancia local dedicada |
| `-E` | Autenticación de Windows |
| `-C` | Confía en el certificado del servidor |
| `-b` | Abortar si un script devuelve error |
| `< archivo.sql` | Se pasa por *stdin* (evita una incompatibilidad de `-i` con `-E` en algunas versiones de sqlcmd) |

> En una máquina de desarrollo que ya tiene la instancia `SQLEXPRESS`, podés usarla
> en su lugar (`-S .\SQLEXPRESS`) y ajustar `Data Source` del config a `.\SQLEXPRESS`.

### 6.3 Copiar la aplicación

Copiar el contenido de `installer-input\` a la carpeta de instalación deseada
(por ejemplo `C:\Program Files\StockHelper\`) y crear un acceso directo a `UI.exe`.

### 6.4 Definir la variable de entorno de cifrado (obligatoria)

Desde una consola **como administrador** (la app no arranca sin esta variable).
Lo más simple es correr el script incluido en la raíz del repositorio, que fija la
clave fija y guarda una copia de resguardo:

```powershell
powershell -ExecutionPolicy Bypass -File .\set-secret-key.ps1
```

Alternativamente, definirla a mano con la misma clave fija:

```powershell
setx STOCK_HELPER_SECRET_KEY "la-clave-maestra-fija-en-Base64" /M
```

`/M` la define a nivel de **máquina**. Cerrar y reabrir la sesión para que tome
efecto. **No cambiar** este valor una vez que haya datos cifrados.

---

## 7. Configuración

La configuración vive en el archivo `UI.dll.config` (junto al ejecutable),
generado a partir de `App.config`.

### 7.1 Cadenas de conexión

```xml
<connectionStrings>
  <add name="iamDb"  connectionString="Data Source=.\STOCKHELPER;Initial Catalog=iam_db;Persist Security Info=True;User ID={sqlUser};Password={sqlPassword};Trust Server Certificate=True" />
  <add name="coreDb" connectionString="Data Source=.\STOCKHELPER;Initial Catalog=core_db;Persist Security Info=True;User ID={sqlUser};Password={sqlPassword};Trust Server Certificate=True" />
</connectionStrings>
```

- `Data Source=.\STOCKHELPER` → instancia local **dedicada**. En una máquina de
  **desarrollo** que usa la instancia genérica, cambiar a `.\SQLEXPRESS`.
- `{sqlUser}` / `{sqlPassword}` → se reemplazan en tiempo de ejecución con los
  valores de `appSettings` (`sqlUser`, `sqlPassword`), que deben coincidir con el
  login creado por el script `01`.
- `Trust Server Certificate=True` → necesario porque SQL Express usa un
  certificado autofirmado.

> **Dev vs. instalador:** `App.config` en el repositorio apunta a `.\SQLEXPRESS`
> para poder correr la app en la máquina de desarrollo. **Antes de publicar para el
> instalador** hay que dejarlo en `.\STOCKHELPER` (o usar el flujo de publicación,
> que genera el `UI.dll.config` con la instancia dedicada).

### 7.2 Otros ajustes (`appSettings`)

| Clave | Función |
|---|---|
| `sqlUser` / `sqlPassword` | Credenciales del login SQL de la app |
| `backupDirectory` | Carpeta de respaldos diarios (por defecto `C:\StockHelperBackups`) |
| `backupRetentionDays` | Días de retención de respaldos (por defecto 14) |
| `logFileDirectory` | Carpeta de logs del sistema |
| `Culture` / `LanguageFolderPath` / `LanguageFileName` | Idioma y archivos de traducción |

> La cuenta del servicio de SQL Server (`NT AUTHORITY\SYSTEM`) debe tener permiso
> de escritura sobre `backupDirectory` para que funcione el respaldo diario.

> ⚠️ **Nota de seguridad:** la contraseña del login de base de datos se guarda en
> **texto plano** en `appSettings` (`sqlPassword`). Restringí el acceso de lectura
> al archivo `UI.dll.config` y a la carpeta de instalación.

### 7.3 Clave maestra de cifrado (`STOCK_HELPER_SECRET_KEY`)

Independiente del `.config`: es una **variable de entorno de máquina** con la que
la aplicación cifra/descifra los campos sensibles de proveedores.

- Es **obligatoria**: sin ella, la aplicación se cierra al iniciar.
- Debe ser **estable en el tiempo**: si cambia, los datos ya cifrados quedan
  ilegibles.
- Se define en la instalación (ver §5.5 para Advanced Installer o §6.4 para la
  vía manual).

---

## 8. Primer inicio de sesión

> **Requisito previo:** haber ejecutado el script de alta de bases (§5.6). Sin ese
> paso las bases no existen y la aplicación no podrá conectarse.

Al abrir la aplicación por primera vez, ingresar con el usuario administrador
inicial creado por el script de datos:

| Campo | Valor |
|---|---|
| Usuario | `admin` |
| Contraseña | `admin` |

> ⚠️ **Cambiar la contraseña inmediatamente** desde el módulo de gestión de
> usuarios. La contraseña se almacena hasheada (MD5 sobre UTF-16LE).

---

## 9. Verificación de la instalación

1. El servicio **SQL Server (STOCKHELPER)** está iniciado (services.msc).
2. La variable de entorno de máquina quedó definida:
   ```powershell
   [Environment]::GetEnvironmentVariable("STOCK_HELPER_SECRET_KEY","Machine")
   ```
   Debe devolver la clave fija (no vacío).
3. Existen las bases `iam_db` y `core_db`:
   ```powershell
   sqlcmd -S .\STOCKHELPER -E -C -Q "SELECT name FROM sys.databases WHERE name IN ('iam_db','core_db')"
   ```
4. La aplicación abre y permite iniciar sesión con `admin`.
5. Se puede navegar por los módulos según los permisos del rol Administrator.

---

## 10. Respaldos (backup)

La aplicación realiza un **respaldo diario** de la base de datos (requisito
REQ.006):

- Carpeta destino: la indicada en `backupDirectory` (`C:\StockHelperBackups`).
- Retención: `backupRetentionDays` días (por defecto 14).
- La cuenta del servicio SQL debe tener escritura sobre esa carpeta.

---

## 11. Desinstalación

- **Aplicación:** desde *Panel de control → Programas y características*
  (o el desinstalador de Advanced Installer).
- **Bases de datos:** no se eliminan automáticamente para evitar pérdida de datos.
  Para quitarlas manualmente:
  ```powershell
  sqlcmd -S .\STOCKHELPER -E -C -Q "DROP DATABASE iam_db; DROP DATABASE core_db;"
  sqlcmd -S .\STOCKHELPER -E -C -Q "DROP LOGIN stock_helper_user;"
  ```
- **SQL Server Express (instancia `STOCKHELPER`):** se desinstala por separado desde
  *Programas y características* si ya no se necesita. Al ser una instancia dedicada,
  quitarla no afecta a otras instalaciones de SQL Server del equipo.

---

## 12. Solución de problemas

| Síntoma | Causa probable | Solución |
|---|---|---|
| `Login failed for user 'stock_helper_user'` | El motor no está en modo mixto, o el login no fue creado | Reinstalar SQL con `/SECURITYMODE=SQL`, o volver a correr el script `01`. Verificar `sqlUser`/`sqlPassword` en el config. |
| `Database 'core_db' does not exist` / la app no conecta tras instalar | Todavía no se ejecutó el script de alta de bases | Ejecutar `Crear-Bases-StockHelper.bat` **como administrador** (§5.6) y esperar el mensaje de éxito. |
| El script de bases dice *"no se pudo conectar a '.\STOCKHELPER'"* | El servicio SQL no está iniciado, el instalador no terminó, o la instancia tiene otro nombre | Verificar que el servicio **SQL Server (STOCKHELPER)** esté iniciado; reintentar; si la instancia tiene otro nombre usar `-Server` (§5.6). |
| Se abre el "SQL Server Installation Center" (SQL interactivo) | La línea de comandos del prereq solo estaba en el campo *Silent* | Poner la misma línea en los **tres** campos: *Full UI*, *Basic UI* y *Silent* (§5.3). |
| `A network-related... error... server was not found` | La instancia no se llama `STOCKHELPER`, o el servicio está detenido | Verificar el nombre de instancia (`.\STOCKHELPER`) y que el servicio SQL esté iniciado. |
| Login/ODBC **timeout** al conectar tras instalar | Se forzó un puerto fijo (1433) sobre una instancia con puerto dinámico | Quitar el puerto de la conexión (resolver por nombre) o fijar un puerto estático en la instancia (§5.6). |
| Error de certificado SSL al conectar | Certificado autofirmado no confiado | Asegurar `Trust Server Certificate=True` en el config (ya incluido). |
| La app no abre / falta runtime | Falta el .NET 8 Desktop Runtime | Instalar el **.NET Desktop Runtime 8 (x64)**. |
| La app se cierra apenas inicia (sin login) | Falta la variable `STOCK_HELPER_SECRET_KEY` | Definirla a nivel de máquina (§5.5 / §6.4) y reiniciar la sesión de Windows. |
| Al compilar el instalador: `MSB4247 ... NuGetSdkResolver` | Advanced Installer usó el MSBuild de otra herramienta (p. ej. SSMS) | Usar la publicación previa con `build-installer-input.ps1` y empaquetar `installer-input\` (no dejar que Advanced Installer compile la solución). |
| El respaldo diario no se genera | La cuenta del servicio SQL no puede escribir en la carpeta | Dar permisos de escritura sobre `backupDirectory` a la cuenta del servicio. |

---

## 13. Apéndice A — Armar el instalador desde cero (paso a paso)

Esta guía recorre, de principio a fin, la creación del proyecto de **Advanced
Installer** para StockHelper. La sección 5 documenta el *qué* de cada página;
acá va el *orden* completo y los pasos de arranque (crear el proyecto, datos de
producto, carpeta de instalación) que la sección 5 daba por hechos.

> **Edición necesaria:** la página *Prerequisites* (para embeber .NET y SQL Server)
> requiere **Advanced Installer Professional o superior**. La creación de bases ya
> **no** se hace dentro del instalador (es el script post-instalación de §5.6), así
> que **no** se usa la página *SQL Databases* ni hace falta la edición Architect.
> Con la edición gratuita podés armar la copia de archivos y el acceso directo, pero
> no los prerequisitos embebidos (en ese caso, seguí la **Vía B — instalación manual**).

### A.0 Antes de abrir Advanced Installer

Tené listo esto (una sola vez):

1. **Salida de la app publicada.** Desde la raíz del proyecto:
   ```powershell
   powershell -ExecutionPolicy Bypass -File .\build-installer-input.ps1
   ```
   Debe quedar la carpeta `installer-input\` con `UI.exe` y el `UI.dll.config`
   apuntando a `.\STOCKHELPER`.
2. **Instalador completo de SQL Server Express** descargado (p. ej.
   `SQLEXPR_x64_ENU.exe`) — el paquete *offline*, no el "web installer".
3. **La carpeta `StockHelper-Crear-Bases\`** (los 4 scripts SQL + el `.bat`/`.ps1` +
   `LEEME.txt`) lista para **entregar junto al instalador** (no va dentro del `.exe`).
4. **La clave maestra fija** (Base64) para la variable de entorno.
5. **Advanced Installer** instalado con licencia Professional o superior.

### A.1 Crear el proyecto

1. Abrir Advanced Installer → **New Project**.
2. En *Installer* elegir el tipo **Professional** (o Enterprise/Architect si se
   dispone). **No** elegir "Simple": no tiene Prerequisites ni SQL.
3. **Create Project** y guardarlo, por ejemplo, como `StockHelper.aip`.

### A.2 Datos del producto (Product Details)

En la página **Product Details**:

| Campo | Valor sugerido |
|---|---|
| Product Name | `StockHelper` |
| Product Version | `1.0.0` |
| Company / Publisher | tu nombre o el de la facultad |
| Upgrade Code | dejar el GUID que genera AI (no cambiarlo entre versiones) |

> Al subir de versión, incrementá **Product Version** y mantené el **Upgrade
> Code**: así el instalador reconoce y actualiza la versión anterior.

### A.3 Carpeta de instalación (Install Parameters)

En **Install Parameters** (o *Product Details → Install location*):

- **Application Folder / INSTALLDIR:** `[ProgramFiles64Folder][Manufacturer]\[ProductName]`
  (queda en `C:\Program Files\StockHelper\`).
- Dejar la instalación **per-machine** (`ALLUSERS=1`, para todos los usuarios),
  coherente con que la clave y la base son de máquina.

### A.4 Agregar los archivos de la aplicación

En **Files and Folders**:

1. Seleccionar la carpeta destino **Application Folder**.
2. *Add Folder* → elegir `installer-input\`.
3. Verificar que quede **`UI.exe`** y sus dependencias (incluido `UI.dll.config`).

### A.5 Acceso directo

En **Files and Folders** (o *Shortcuts*): clic derecho sobre `UI.exe` →
*New Shortcut* → crear accesos en **Desktop** y **Start Menu**.

### A.6 Prerequisito — .NET 8 Desktop Runtime

Página **Prerequisites** → agregar el predefinido **".NET Desktop Runtime 8.x
(x64)"**, **embebido**. (Detalle en §5.2.) Marcarlo para instalarse **antes** que
la aplicación.

### A.7 Prerequisito — SQL Server Express (instancia `STOCKHELPER`, modo mixto)

Página **Prerequisites** → *New Prerequisite → Package* → seleccionar el
`SQLEXPR_x64_ENU.exe`, **embebido**, con:

- La línea de comandos **silenciosa y en modo mixto** de **§5.3**, en los **tres**
  campos (*Full UI*, *Basic UI*, *Silent*), con `/INSTANCENAME=STOCKHELPER` y
  `/SQLSYSADMINACCOUNTS="BUILTIN\Administrators"`.
- La **detección** como *Registry value exists* con valor `STOCKHELPER` (**§5.3.1**).

Ordenarlo para instalarse **después** del runtime y **antes** de la app.

### A.8 Variable de entorno — clave de cifrado

Página **Environment** → *New Environment Variable* → `STOCK_HELPER_SECRET_KEY`
a nivel **System/Machine**, con la clave fija, tal como se describe en **§5.5**.

> (En versiones previas esto se hacía con un *Custom Action* + `set-secret-key.ps1`;
> ese enfoque quedó obsoleto — ver la nota en §5.5.)

### A.9 Bases de datos — NO van en el instalador

En este diseño el instalador **no** crea las bases: se hace después con el script
`Crear-Bases-StockHelper.bat` (§5.6). Por lo tanto **no** hay que configurar la
página *SQL Databases* de Advanced Installer.

> Solo asegurate de que la carpeta **`StockHelper-Crear-Bases\`** se entregue junto
> al `StockHelper.exe` (en el mismo pendrive/carpeta de entrega). Si en una versión
> previa del `.aip` habías cargado una conexión + scripts en *SQL Databases*,
> **borrá esa conexión** (al borrarla, AI elimina también los scripts y las acciones
> asociadas) y volvé a **Build**.

### A.10 Revisar el orden de ejecución

Confirmar que la secuencia efectiva sea la de **§5.7**:

```
Instalador:
  1) .NET 8 Desktop Runtime
  2) SQL Server Express modo mixto (instancia STOCKHELPER, solo el motor)
  3) Copia de archivos
  4) Variable STOCK_HELPER_SECRET_KEY (página Environment, máquina)
  5) Acceso directo a UI.exe
Post-instalación (usuario): Crear-Bases-StockHelper.bat → scripts 01 → 04
```

### A.11 Generar el instalador (Build)

1. Botón **Build** (o `F7`).
2. Advanced Installer produce el `StockHelper.exe` (setup offline con los
   prerequisitos embebidos) en la carpeta de salida del proyecto.

> Si aparece el error `MSB4247 ... NuGetSdkResolver` durante el build, es porque
> AI intentó **compilar** la solución con el MSBuild equivocado. La solución es no
> compilar en AI: empaquetar la carpeta `installer-input\` ya publicada (ver la
> tabla de §12). Este proyecto está pensado justamente así.

### A.12 Probar en una máquina limpia

Idealmente en **Windows Sandbox** o una VM sin nada instalado (no en la máquina de
desarrollo, que tiene su propia instancia y base):

1. Ejecutar el `StockHelper.exe` **como administrador**.
2. Verificar que instale el runtime, SQL Express (instancia `STOCKHELPER`) y la app.
3. **Ejecutar `Crear-Bases-StockHelper.bat`** (§5.6) y esperar el mensaje de éxito.
4. Comprobar con la **sección 9** (servicio SQL iniciado, variable de entorno
   definida, bases `iam_db`/`core_db` creadas, login con `admin`/`admin`).

---

*Fin del Manual de Instalación.*
