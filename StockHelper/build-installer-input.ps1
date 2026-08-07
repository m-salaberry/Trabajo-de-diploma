# Publica StockHelper con el .NET SDK correcto para empaquetar en Advanced Installer.
# Uso:  powershell -ExecutionPolicy Bypass -File .\build-installer-input.ps1
#       powershell -ExecutionPolicy Bypass -File .\build-installer-input.ps1 -SqlInstance ".\OTRA"
param(
    # Instancia SQL que debe quedar en el paquete. El instalador crea una instancia
    # dedicada STOCKHELPER, asi que el config publicado tiene que apuntar ahi sin importar
    # a que instancia apunte UI\App.config en la maquina de desarrollo.
    [string] $SqlInstance = ".\STOCKHELPER"
)

$ErrorActionPreference = "Stop"

$dotnet = "C:\Program Files\dotnet\dotnet.exe"
$root   = $PSScriptRoot
$outDir = Join-Path $root "installer-input"

Write-Host "Limpiando salida anterior..." -ForegroundColor Cyan
if (Test-Path $outDir) { Remove-Item $outDir -Recurse -Force }

Write-Host "Publicando UI (Release)..." -ForegroundColor Cyan
& $dotnet publish "$root\UI\UI.csproj" -c Release -o $outDir --nologo

if ($LASTEXITCODE -ne 0) { throw "La publicacion fallo (codigo $LASTEXITCODE)" }

# Fijar la instancia SQL en el config publicado. UI\App.config apunta a la instancia de
# desarrollo; el paquete debe apuntar siempre a la que crea el instalador.
$publishedConfig = Join-Path $outDir "UI.dll.config"
if (Test-Path $publishedConfig) {
    $cfg = Get-Content -Raw -Encoding UTF8 $publishedConfig
    $nuevo = [regex]::Replace($cfg, 'Data Source=[^;]+;', "Data Source=$SqlInstance;")
    if ($nuevo -cne $cfg) {
        Set-Content -Path $publishedConfig -Value $nuevo -Encoding UTF8 -NoNewline
        Write-Host "Instancia SQL del paquete fijada a '$SqlInstance'." -ForegroundColor Cyan
    } else {
        Write-Host "El config publicado ya apuntaba a '$SqlInstance'." -ForegroundColor Cyan
    }
} else {
    throw "No se encontro $publishedConfig tras publicar"
}

# Verificar que las traducciones viajaron al paquete (UI.csproj las copia como Content).
$i18nDir = Join-Path $outDir "I18n"
$traducciones = @(if (Test-Path $i18nDir) { Get-ChildItem $i18nDir -Filter "translations.*" } else { @() })
if ($traducciones.Count -eq 0) {
    throw "El paquete no incluye la carpeta I18n con los archivos de traduccion"
}
Write-Host "Traducciones incluidas: $($traducciones.Name -join ', ')" -ForegroundColor Cyan

# Copiar el script de la clave maestra al paquete (lo usa el Custom Action del instalador
# y sirve para restaurar la clave a mano). dotnet publish limpia $outDir, por eso se copia aca.
$secretScript = Join-Path $root "set-secret-key.ps1"
if (Test-Path $secretScript) {
    Copy-Item $secretScript $outDir -Force
    Write-Host "Copiado set-secret-key.ps1 al paquete." -ForegroundColor Cyan
} else {
    Write-Host "ADVERTENCIA: no se encontro set-secret-key.ps1; el instalador no podra fijar la clave." -ForegroundColor Red
}

Write-Host ""
Write-Host "OK. Archivos listos en:" -ForegroundColor Green
Write-Host "  $outDir" -ForegroundColor Green
Write-Host "En Advanced Installer: Files and Folders -> Add Folder -> selecciona esa carpeta." -ForegroundColor Yellow
