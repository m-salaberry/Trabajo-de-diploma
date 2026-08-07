param(
    [string]$Instance = 'MATI\SQLEXPRESS',
    [string]$OutDir   = 'D:\Facultad\Trabajo-de-diploma\StockHelper-Installer-DB'
)
$ErrorActionPreference = 'Stop'
Push-Location $PWD
try { Import-Module SqlServer -DisableNameChecking -ErrorAction Stop }
catch { Import-Module SQLPS -DisableNameChecking -ErrorAction Stop }
Pop-Location

function Esc([string]$s) { if ($null -eq $s) { 'NULL' } else { "N'" + $s.Replace("'","''") + "'" } }

$patents = Invoke-Sqlcmd -ServerInstance $Instance -Database 'iam_db' `
    -Query "SELECT Id, Name, Description FROM PATENTS ORDER BY Name"

$sb = New-Object System.Text.StringBuilder
[void]$sb.AppendLine("-- ==============================================================")
[void]$sb.AppendLine("-- DATOS SEMILLA de iam_db  (permisos + rol Administrator + usuario admin)")
[void]$sb.AppendLine("-- Extraido del estado ACTUAL en vivo. Idempotente (IF NOT EXISTS).")
[void]$sb.AppendLine("--")
[void]$sb.AppendLine("-- Usuario admin inicial:  usuario = 'admin'   contrasena = 'admin'")
[void]$sb.AppendLine("-- (contrasena hasheada MD5/UTF-16LE por CryptographyService.HashMd5)")
[void]$sb.AppendLine("-- CAMBIAR la contrasena tras el primer inicio de sesion.")
[void]$sb.AppendLine("-- ==============================================================")
[void]$sb.AppendLine("USE [iam_db];")
[void]$sb.AppendLine("GO")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("-- ---------- PERMISOS (PATENTS) ----------")
foreach ($p in $patents) {
    $id = $p.Id.ToString().ToUpper()
    [void]$sb.AppendLine("IF NOT EXISTS (SELECT 1 FROM dbo.PATENTS WHERE Name = $(Esc $p.Name))")
    [void]$sb.AppendLine("    INSERT INTO dbo.PATENTS (Id, Name, Description, CreatedDate)")
    [void]$sb.AppendLine("    VALUES ('$id', $(Esc $p.Name), $(Esc $p.Description), GETDATE());")
}
[void]$sb.AppendLine("GO")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("-- ---------- ROL ADMINISTRATOR (FAMILY) ----------")
[void]$sb.AppendLine("IF NOT EXISTS (SELECT 1 FROM dbo.FAMILIES WHERE Name = N'Administrator')")
[void]$sb.AppendLine("    INSERT INTO dbo.FAMILIES (Id, Name, Description, CreatedDate)")
[void]$sb.AppendLine("    VALUES (NEWID(), N'Administrator', N'Total access to the system', GETDATE());")
[void]$sb.AppendLine("GO")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("-- ---------- ASIGNAR TODOS LOS PERMISOS AL ROL ADMINISTRATOR ----------")
[void]$sb.AppendLine("INSERT INTO dbo.PATENTS_FAMILIES (PatentId, FamilyId, AssignedDate)")
[void]$sb.AppendLine("SELECT p.Id, f.Id, GETDATE()")
[void]$sb.AppendLine("FROM dbo.PATENTS p")
[void]$sb.AppendLine("CROSS JOIN dbo.FAMILIES f")
[void]$sb.AppendLine("WHERE f.Name = N'Administrator'")
[void]$sb.AppendLine("  AND NOT EXISTS (SELECT 1 FROM dbo.PATENTS_FAMILIES pf WHERE pf.PatentId = p.Id AND pf.FamilyId = f.Id);")
[void]$sb.AppendLine("GO")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("-- ---------- USUARIO ADMIN INICIAL ----------")
[void]$sb.AppendLine("-- MD5(UTF-16LE) de 'admin' = 19a2854144b63a8f7617a6f225019b12")
[void]$sb.AppendLine("IF NOT EXISTS (SELECT 1 FROM dbo.USERS WHERE Name = N'admin')")
[void]$sb.AppendLine("    INSERT INTO dbo.USERS (Id, Name, Password, Role, IsActive, CreatedDate)")
[void]$sb.AppendLine("    VALUES (NEWID(), N'admin', N'19a2854144b63a8f7617a6f225019b12', N'Administrator', 1, GETDATE());")
[void]$sb.AppendLine("GO")
[void]$sb.AppendLine("")
[void]$sb.AppendLine("-- ---------- VINCULAR admin AL ROL ADMINISTRATOR ----------")
[void]$sb.AppendLine("INSERT INTO dbo.USERS_FAMILIES (UserId, FamilyId, AssignedDate)")
[void]$sb.AppendLine("SELECT u.Id, f.Id, GETDATE()")
[void]$sb.AppendLine("FROM dbo.USERS u, dbo.FAMILIES f")
[void]$sb.AppendLine("WHERE u.Name = N'admin' AND f.Name = N'Administrator'")
[void]$sb.AppendLine("  AND NOT EXISTS (SELECT 1 FROM dbo.USERS_FAMILIES uf WHERE uf.UserId = u.Id AND uf.FamilyId = f.Id);")
[void]$sb.AppendLine("GO")

$target = Join-Path $OutDir '03_iam_db_seed.sql'
[System.IO.File]::WriteAllText($target, $sb.ToString(), (New-Object System.Text.UTF8Encoding($false)))
Write-Host "OK -> $target ($($patents.Count) permisos)"
