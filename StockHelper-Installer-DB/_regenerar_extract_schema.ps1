param(
    [string]$Instance = 'MATI\SQLEXPRESS',
    [string]$OutDir   = 'D:\Facultad\Trabajo-de-diploma\StockHelper-Installer-DB'
)
$ErrorActionPreference = 'Stop'
Push-Location $PWD
try { Import-Module SqlServer -DisableNameChecking -ErrorAction Stop }
catch { Import-Module SQLPS -DisableNameChecking -ErrorAction Stop }
Pop-Location
New-Item -ItemType Directory -Force -Path $OutDir | Out-Null

$conn = New-Object Microsoft.SqlServer.Management.Common.ServerConnection
$conn.ServerInstance        = $Instance
$conn.LoginSecure           = $true
$conn.TrustServerCertificate = $true
$conn.Connect()
$srv = New-Object Microsoft.SqlServer.Management.Smo.Server($conn)

function New-Opt {
    $o = New-Object Microsoft.SqlServer.Management.Smo.ScriptingOptions
    $o.IncludeHeaders = $false
    $o.ScriptBatchTerminator = $true
    $o.NoCollation = $true
    $o.AnsiPadding = $false
    $o.ExtendedProperties = $false
    return $o
}

function Export-DbSchema([string]$dbName, [string]$file) {
    $db = $srv.Databases[$dbName]
    $sb = New-Object System.Text.StringBuilder
    [void]$sb.AppendLine("-- ==============================================================")
    [void]$sb.AppendLine("-- ESQUEMA de $dbName  (extraido del estado ACTUAL en vivo)")
    [void]$sb.AppendLine("-- Generado con SMO. NO editar a mano salvo necesidad.")
    [void]$sb.AppendLine("-- ==============================================================")
    [void]$sb.AppendLine("USE [$dbName];")
    [void]$sb.AppendLine("GO")

    $tables = $db.Tables | Where-Object { -not $_.IsSystemObject -and $_.Name -ne 'sysdiagrams' } | Sort-Object Name

    # Pass 1: tablas con PK, uniques, defaults, indices (SIN foreign keys)
    $o1 = New-Opt
    $o1.DriPrimaryKey = $true; $o1.DriUniqueKeys = $true; $o1.DriDefaults = $true
    $o1.DriChecks = $true; $o1.ClusteredIndexes = $true; $o1.NonClusteredIndexes = $true
    $o1.Indexes = $true; $o1.DriForeignKeys = $false
    [void]$sb.AppendLine(""); [void]$sb.AppendLine("-- ---------- TABLAS ----------")
    foreach ($t in $tables) {
        foreach ($line in $t.Script($o1)) { [void]$sb.AppendLine($line); [void]$sb.AppendLine("GO") }
    }

    # Pass 2: solo foreign keys (ALTER TABLE) para respetar el orden de creacion
    $o2 = New-Opt
    $o2.DriForeignKeys = $true; $o2.PrimaryObject = $false; $o2.DriPrimaryKey = $false
    $o2.ClusteredIndexes = $false; $o2.NonClusteredIndexes = $false; $o2.Indexes = $false
    [void]$sb.AppendLine(""); [void]$sb.AppendLine("-- ---------- FOREIGN KEYS ----------")
    foreach ($t in $tables) {
        foreach ($fk in $t.ForeignKeys) {
            foreach ($line in $fk.Script($o2)) { [void]$sb.AppendLine($line); [void]$sb.AppendLine("GO") }
        }
    }

    $target = Join-Path $OutDir $file
    [System.IO.File]::WriteAllText($target, $sb.ToString(), (New-Object System.Text.UTF8Encoding($false)))
    Write-Host "OK -> $target ($($tables.Count) tablas)"
}

Export-DbSchema 'iam_db'  '02_iam_db_schema.sql'
Export-DbSchema 'core_db' '04_core_db_schema.sql'
Write-Host "LISTO"
