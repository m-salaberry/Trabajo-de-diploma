# StockHelper — Scripts de instalación de base de datos

Scripts para crear e inicializar las bases de datos de StockHelper en una
**instalación nueva** (por ejemplo, desde el instalador de Advanced Installer).

> **Origen:** extraídos del estado **actual y en vivo** de `MATI\SQLEXPRESS`
> el **2026-07-10** con SMO (esquema) + consultas directas (datos semilla).
> Reemplazan a los scripts obsoletos de `StockHelper/Database/`, que **no**
> reflejaban el esquema actual — no usar aquellos.
>
> Todos los scripts fueron **probados end-to-end** contra bases desechables:
> el esquema crea 5 tablas en `iam_db` y 8 en `core_db`, el seed carga 9
> permisos + rol Administrator + usuario admin, y es **idempotente**.

## Orden de ejecución (obligatorio)

| # | Archivo | Qué hace | Base destino |
|---|---------|----------|--------------|
| 1 | `01_login_and_databases.sql` | Crea el login SQL `stock_helper_user`, las bases `iam_db` y `core_db`, y el usuario de base con rol `db_owner` en ambas. | `master` |
| 2 | `02_iam_db_schema.sql` | Esquema de `iam_db` (tablas, PK/UNIQUE, índices, checks, FKs). | `iam_db` |
| 3 | `03_iam_db_seed.sql` | Permisos (PATENTS), rol **Administrator** con todos los permisos, y usuario **admin** inicial. | `iam_db` |
| 4 | `04_core_db_schema.sql` | Esquema de `core_db` (inventario, productos, órdenes, proveedores). Se crea **vacío** (sin datos de negocio). | `core_db` |

El paso 3 depende del 2. El paso 4 es independiente (puede ir antes o después
del 2/3, pero después del 1).

## Con qué usuario ejecutarlos (en la instalación)

Los scripts deben ejecutarse conectados como **administrador del servidor**
(el login `sa`, o un login de Windows con rol `sysadmin`), **no** con
`stock_helper_user` (que aún no existe en la primera instalación).

- Si instalas SQL Server Express desde el instalador en **modo desatendido**,
  define `sa` con `/SECURITYMODE=SQL /SAPWD=<clave_sa>` y usa ese `sa` para
  ejecutar estos scripts.

## Requisito del motor: MODO MIXTO

La aplicación se conecta con el **login SQL** `stock_helper_user` + contraseña
(no con autenticación de Windows). Por eso SQL Server **debe** quedar instalado
en **modo de autenticación mixto** (SQL + Windows). En la instalación
desatendida de SQL Express: `/SECURITYMODE=SQL`.

## Credenciales

- **Login de base de datos** (debe coincidir con `UI.dll.config`):
  - Usuario: `stock_helper_user`
  - Contraseña: la definida en `App.config` (`{sqlPassword}`).
- **Usuario admin inicial de la aplicación** (para el primer inicio de sesión):
  - Usuario: `admin`
  - Contraseña: `admin`
  - ⚠️ **Cambiar la contraseña tras el primer login.** El hash almacenado es
    MD5 sobre UTF-16LE, tal como lo genera `CryptographyService.HashMd5`.

## ⚠️ Nombre del servidor en `App.config`

El `App.config`/`UI.dll.config` trae fijo `Data Source=MATI\SQLEXPRESS`
(el nombre de la máquina de desarrollo). En la PC del cliente eso no existe.
Antes de empaquetar, cambiarlo a **`.\SQLEXPRESS`** (`.` = máquina local),
que funciona en cualquier equipo cuya instancia se llame `SQLEXPRESS`.

## Cómo cargarlos en Advanced Installer

En la página **SQL Databases** (requiere edición Professional o superior):

1. **New Connection** → servidor `.\SQLEXPRESS`, autenticación como `sa`
   (o Windows), y marca que la conexión se resuelva **en la máquina destino
   durante la instalación** (runtime), no en build.
2. **Import Script** para cada `.sql`, respetando el orden 01 → 02 → 03 → 04.
3. Programa su ejecución en la fase de instalación, **después** del
   prerequisito que instala SQL Server Express.

## Regenerar estos scripts

Si la base vuelve a cambiar, regenera con los scripts incluidos en esta
misma carpeta: `_regenerar_extract_schema.ps1` (esquema) y
`_regenerar_generate_seed.ps1` (semilla), apuntando a `MATI\SQLEXPRESS`.
Ejecutar con: `powershell -ExecutionPolicy Bypass -File _regenerar_extract_schema.ps1`
