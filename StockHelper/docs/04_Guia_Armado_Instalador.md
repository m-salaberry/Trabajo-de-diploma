# StockHelper — Guía detallada para armar el instalador

> Guía autocontenida y paso a paso para crear el instalador de **StockHelper** en
> **Advanced Installer** (edición Professional o superior), con cada campo y valor
> concreto. Pensada para seguir mientras se hacen los clics.
>
> Complementa el `01_Manual_de_Instalacion.md` (que documenta *qué* hace cada
> página); esta guía da el recorrido completo *desde cero*.

---

## Fase 0 — Preparación (fuera de Advanced Installer)

Hacer esto una sola vez antes de abrir AI:

1. **Publicar la app.** Desde la raíz del proyecto
   (`D:\Facultad\Trabajo-de-diploma\StockHelper`):
   ```powershell
   powershell -ExecutionPolicy Bypass -File .\build-installer-input.ps1
   ```
   Al terminar, confirmar que en `installer-input\` estén **`UI.exe`** y
   **`set-secret-key.ps1`**.

2. **Descargar SQL Server Express (paquete offline completo).** El archivo
   `SQLEXPR_x64_ENU.exe` (no el "web installer" chico). Guardarlo en una ruta
   fija, p. ej. `D:\Facultad\Trabajo-de-diploma\Installers\SQLEXPR_x64_ENU.exe`.

3. **Verificar los 4 scripts** en
   `D:\Facultad\Trabajo-de-diploma\StockHelper-Installer-DB\`:
   `01_login_and_databases.sql` … `04_core_db_schema.sql`.

4. **Advanced Installer** instalado con licencia **Professional o superior** (las
   páginas *Prerequisites* y *SQL Databases* no existen en la edición Free).

---

## Fase 1 — Crear el proyecto

1. Abrir Advanced Installer → **File → New** (o botón *New Project* en la pantalla
   de inicio).
2. Categoría **Installer** → tipo **Professional** → botón **Create Project**.
   - No elegir "Simple": no tiene Prerequisites ni SQL.
3. **File → Save As** → guardar como `StockHelper.aip` en la raíz del proyecto.

---

## Fase 2 — Datos del producto

Panel izquierdo → grupo **Product Information → Product Details**:

| Campo | Valor |
|---|---|
| **Product Name** | `StockHelper` |
| **Product Version** | `1.0.0` |
| **Company Name** | tu nombre / la facultad |
| **Product/Package GUID** | dejar el que genera AI |
| **Upgrade Code** | **no cambiarlo nunca** (es lo que permite actualizar versiones futuras) |

---

## Fase 3 — Carpeta y modo de instalación

Panel izquierdo → **Product Information → Install Parameters**:

- **Installation type / context:** **Per-machine** (para todos los usuarios). Es
  coherente con que la clave y la base son de máquina.
- **Application Folder (INSTALLDIR):** dejar el valor por defecto
  `[ProgramFiles64Folder][Manufacturer]\[ProductName]`
  → instalará en `C:\Program Files\StockHelper\`.

---

## Fase 4 — Archivos de la aplicación

Panel izquierdo → **Resources → Files and Folders**:

1. En el árbol de la izquierda seleccionar **Application Folder**.
2. Botón **Add Folder** (barra superior) → elegir la carpeta `installer-input\`.
3. AI copia todo su contenido dentro de Application Folder. Verificar que aparezcan:
   - `UI.exe`
   - `UI.dll` y demás dependencias
   - **`set-secret-key.ps1`** ← imprescindible para la Fase 7.

---

## Fase 5 — Acceso directo

Sigue en **Files and Folders**:

1. Clic derecho sobre **`UI.exe`** → **New Shortcut**.
2. En el diálogo, elegir como destino **Desktop** y repetir para
   **Start Menu / Programs**.
3. Nombre del acceso: `StockHelper`.

---

## Fase 6 — Prerequisitos

Panel izquierdo → **Prerequisites** (grupo *Requirements* / *Prerequisites*).

### 6a) .NET 8 Desktop Runtime
1. Botón **Add Predefined Prerequisite** → buscar **".NET Desktop Runtime 8.x (x64)"**.
2. Agregarlo. Dejarlo para instalarse **antes** que la aplicación (AI ya lo ordena así).

### 6b) SQL Server Express (modo mixto)
1. Botón **New Prerequisite → Package (from file)** → seleccionar `SQLEXPR_x64_ENU.exe`.
2. En la pestaña **Install Command Line**, pegar exactamente:
   ```
   /QS /ACTION=Install /FEATURES=SQLENGINE /INSTANCENAME=SQLEXPRESS /SECURITYMODE=SQL /SAPWD="ClaveSA_Fuerte!2026" /SQLSVCACCOUNT="NT AUTHORITY\SYSTEM" /SQLSYSADMINACCOUNTS="BUILTIN\Administrators" /TCPENABLED=1 /IACCEPTSQLSERVERLICENSETERMS
   ```
   - Cambiar `ClaveSA_Fuerte!2026` por una contraseña fuerte propia y **anotarla**.
   - `/SECURITYMODE=SQL` es lo que activa el **modo mixto** (obligatorio para que la
     app conecte con `stock_helper_user`).
3. En la **detección** (para no reinstalar si ya está): condición sobre el
   servicio `MSSQL$SQLEXPRESS`, o dejar la detección que AI sugiera para SQL Express.
4. Ordenarlo para instalarse **después** del runtime y **antes** de la app/scripts.

> ⚠️ La clave de `sa` debe cumplir la política de complejidad (mayúscula,
> minúscula, número/símbolo, 8+ caracteres) o el `/QS` falla en silencio.

---

## Fase 7 — Custom Action: fijar la clave de cifrado

Panel izquierdo → **Custom Behavior → Custom Actions**.

1. Botón **Add Custom Action** → de la galería elegir **"Launch file"** (o
   *Run attached/installed file*).
2. Configurarlo así:
   - **File to launch / Executable:** `powershell.exe`
     (usar el path del sistema; en AI se puede poner
     `[SystemFolder]WindowsPowerShell\v1.0\powershell.exe` o simplemente
     `powershell.exe`).
   - **Command line / Arguments:**
     ```
     -ExecutionPolicy Bypass -File "[INSTALLDIR]set-secret-key.ps1"
     ```
3. Pestaña **Execution / Sequence**:
   - **Execution Time:** **Deferred** (se ejecuta en la transacción de instalación).
   - **Sequence / When:** **After "InstallFiles"** (el `.ps1` ya tiene que estar
     copiado en disco). Un punto seguro es *When: After files are installed* dentro
     de *Install Execution Stage*.
   - **Condition:** `NOT Installed` (que corra en instalación nueva; opcional, el
     script es idempotente igual).
4. Pestaña **Options / Properties:**
   - Marcar **"Run under the LocalSystem account with full privileges"** (acción
     **elevada**). Es obligatorio porque escribe una variable de entorno **de máquina**.

> El script fija la clave fija (`STOCK_HELPER_SECRET_KEY`), es idempotente (no pisa
> una existente) y deja `STOCK_HELPER_SECRET_KEY.backup.txt` en `[INSTALLDIR]`.

---

## Fase 8 — Bases de datos (scripts SQL)

Panel izquierdo → **SQL Databases** (grupo *Server*).

1. Botón **New Connection**:
   - **Server:** `.\SQLEXPRESS`
   - **Authentication:** **Windows Authentication** (la cuenta instaladora ya quedó
     sysadmin por `/SQLSYSADMINACCOUNTS`, así no se embebe la clave de `sa`).
   - **Run scripts:** en la **máquina destino, en tiempo de instalación** (runtime),
     no en el momento de armar el proyecto.
2. Con la conexión seleccionada, botón **Import Script** y agregar los cuatro,
   **en este orden**:

   | Orden | Script | Qué hace |
   |---|---|---|
   | 1 | `01_login_and_databases.sql` | Login `stock_helper_user` + bases `iam_db`/`core_db` + `db_owner` |
   | 2 | `02_iam_db_schema.sql` | Esquema de `iam_db` |
   | 3 | `03_iam_db_seed.sql` | Permisos + rol Administrator + usuario `admin` |
   | 4 | `04_core_db_schema.sql` | Esquema de `core_db` |

3. Asegurarse de que los scripts corran **después** del prerequisito de SQL Express
   (el motor tiene que existir primero).

---

## Fase 9 — Verificar el orden de ejecución

La secuencia efectiva debe quedar:

```
1) .NET 8 Desktop Runtime           (prerequisito)
2) SQL Server Express modo mixto    (prerequisito)
3) Copia de archivos                (incluye set-secret-key.ps1)
4) Custom Action set-secret-key.ps1 (deferred + elevada, after InstallFiles)
5) Scripts SQL 01 → 02 → 03 → 04    (SQL Databases, runtime)
6) Acceso directo a UI.exe
```

---

## Fase 10 — Generar el instalador (Build)

1. Barra superior → **Build** (o `F7`).
2. AI genera el paquete en la carpeta de salida del proyecto: un `.msi` y/o un
   `setup.exe` (este último trae embebidos los prerequisitos).

> **Si aparece `MSB4247 ... NuGetSdkResolver`:** es porque AI intentó *compilar* la
> solución con el MSBuild equivocado. No dejar que AI compile — la app ya se publica
> con `build-installer-input.ps1` y solo se empaqueta `installer-input\`. Revisar que
> no haya un "Visual Studio project" / build de la solución agregado al proyecto de AI.

---

## Fase 11 — Probar en una máquina limpia

Idealmente en una VM sin nada instalado:

1. Ejecutar `setup.exe` **como administrador**.
2. Debe instalar, en orden: runtime → SQL Express → app → clave → bases.
3. Verificaciones:
   ```powershell
   # ¿Existen las bases?
   sqlcmd -S .\SQLEXPRESS -E -C -Q "SELECT name FROM sys.databases WHERE name IN ('iam_db','core_db')"

   # ¿Quedó la variable de máquina?
   [Environment]::GetEnvironmentVariable("STOCK_HELPER_SECRET_KEY","Machine")
   ```
   La variable debe devolver la clave fija. Si vuelve vacía, revisar el Custom
   Action (Fase 7).
4. Abrir StockHelper e iniciar sesión con **`admin` / `admin`** (cambiar la
   contraseña enseguida).

---

## Notas / advertencias

- **Los nombres exactos de menús y botones** cambian un poco entre versiones de
  Advanced Installer (20/21/22). Los grupos del panel izquierdo
  (*Product Information*, *Resources*, *Prerequisites*, *SQL Databases*,
  *Custom Actions*, *Builds*) son estables; si un botón se llama levemente distinto,
  la función es la misma.
- **La detección del prerequisito de SQL** (para no reinstalarlo si ya está) es la
  parte más artesanal. Si no se configura, en el peor caso reintenta y SQL Express
  aborta porque la instancia ya existe — no rompe la app, pero conviene afinarlo.
- **Recordatorio de seguridad:** la contraseña del login SQL viaja en texto plano en
  `UI.dll.config`, y la clave maestra de cifrado es fija/conocida. Es aceptable para
  un proyecto académico / de uso personal, pero no para distribución masiva.

---

*Fin de la guía.*
