# =====================================================================
#  StockHelper - Alta de bases de datos (post-instalacion)
# =====================================================================
#  Ejecuta, en orden, los 4 scripts .sql que crean el login de la app,
#  las bases iam_db / core_db, el esquema y los datos semilla, sobre la
#  instancia SQL Server que dejo el instalador.
#
#  NO depende de sqlcmd: usa System.Data.SqlClient (incluido en Windows).
#  Conecta por autenticacion de Windows -> hay que ejecutarlo COMO
#  ADMINISTRADOR (el .bat que lo acompania eleva solo).
#
#  Uso normal:  doble clic en  Crear-Bases-StockHelper.bat
#  Uso manual:  powershell -ExecutionPolicy Bypass -File Crear-Bases-StockHelper.ps1
#               (opcional)  -Server ".\OTRA_INSTANCIA"
# =====================================================================

param(
    [string] $Server = ".\STOCKHELPER"
)

$ErrorActionPreference = "Stop"
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Orden OBLIGATORIO de ejecucion.
$sqlFiles = @(
    "01_login_and_databases.sql",
    "02_iam_db_schema.sql",
    "03_iam_db_seed.sql",
    "04_core_db_schema.sql"
)

Write-Host ""
Write-Host "==============================================================="
Write-Host " StockHelper - Alta de bases de datos"
Write-Host "==============================================================="
Write-Host " Servidor / instancia : $Server"
Write-Host " Carpeta de scripts   : $scriptDir"
Write-Host "==============================================================="
Write-Host ""

# --- Validar que existan los 4 scripts ---
foreach ($f in $sqlFiles) {
    $full = Join-Path $scriptDir $f
    if (-not (Test-Path $full)) {
        Write-Host "ERROR: no se encontro el script '$f' en la carpeta." -ForegroundColor Red
        Write-Host "Verifica que los 4 archivos .sql esten junto a este script." -ForegroundColor Red
        exit 1
    }
}

# --- Abrir conexion (Windows auth) contra master ---
$connStr = "Server=$Server;Database=master;Integrated Security=SSPI;TrustServerCertificate=True;Connect Timeout=60"
$conn = New-Object System.Data.SqlClient.SqlConnection $connStr

# Mostrar los PRINT de SQL en pantalla.
$handler = [System.Data.SqlClient.SqlInfoMessageEventHandler] {
    param($sender, $e)
    foreach ($err in $e.Errors) { Write-Host ("    " + $err.Message) -ForegroundColor DarkGray }
}
$conn.add_InfoMessage($handler)
$conn.FireInfoMessageEventOnUserErrors = $false

try {
    Write-Host "Conectando a SQL Server..." -ForegroundColor Cyan
    $conn.Open()
    Write-Host "Conexion establecida ($($conn.ServerVersion))." -ForegroundColor Green
    Write-Host ""
}
catch {
    Write-Host "ERROR: no se pudo conectar a '$Server'." -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    Write-Host "Posibles causas:" -ForegroundColor Yellow
    Write-Host "  - La instancia '$Server' no existe (revisa el nombre)."
    Write-Host "  - El servicio 'SQL Server (STOCKHELPER)' no esta iniciado."
    Write-Host "  - No estas ejecutando como administrador."
    exit 1
}

# --- Ejecutar cada script, partiendo por lotes GO ---
$i = 0
foreach ($f in $sqlFiles) {
    $i++
    $full = Join-Path $scriptDir $f
    Write-Host "[$i/$($sqlFiles.Count)] Ejecutando $f ..." -ForegroundColor Cyan

    $sql = Get-Content -Path $full -Raw -Encoding UTF8
    # Separar en lotes: lineas que contienen solo 'GO' (ignora may/min y espacios).
    $batches = [System.Text.RegularExpressions.Regex]::Split($sql, "(?im)^[\t ]*GO[\t ]*;?[\t ]*\r?$")

    foreach ($batch in $batches) {
        if ([string]::IsNullOrWhiteSpace($batch)) { continue }
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = $batch
        $cmd.CommandTimeout = 120
        try {
            [void]$cmd.ExecuteNonQuery()
        }
        catch {
            Write-Host ""
            Write-Host "ERROR ejecutando un lote de '$f':" -ForegroundColor Red
            Write-Host $_.Exception.Message -ForegroundColor Red
            $conn.Close()
            exit 1
        }
    }
    Write-Host "    OK" -ForegroundColor Green
}

$conn.Close()

Write-Host ""
Write-Host "==============================================================="
Write-Host " LISTO. Las bases de datos se crearon correctamente." -ForegroundColor Green
Write-Host "==============================================================="
Write-Host ""
Write-Host " Ya podes abrir StockHelper e iniciar sesion con:"
Write-Host "     Usuario:      admin"
Write-Host "     Contrasena:   admin"
Write-Host ""
Write-Host " (Cambia la contrasena de admin tras el primer inicio de sesion.)"
Write-Host ""
exit 0
