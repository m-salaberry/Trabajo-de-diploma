# Genera la Referencia de API (Manual del Programador) de StockHelper con
# Sandcastle Help File Builder (SHFB).
#
# Requisitos (una sola vez):
#   - SHFB instalado (define la variable de entorno SHFBROOT).
#     Descarga: https://github.com/EWSoftware/SHFB/releases  (instalar el MSI elevado)
#   - .NET SDK (se usa `dotnet build` para compilar el .shfbproj).
#   - Haber compilado la solucion en Release (para tener los .dll y .xml de doc).
#
# Uso:  powershell -ExecutionPolicy Bypass -File .\build-api-docs.ps1
# Salida: .\Help\index.html (sitio web navegable)

$ErrorActionPreference = "Stop"

$sc   = $PSScriptRoot
$root = Resolve-Path (Join-Path $sc "..\..")   # raiz del repo StockHelper
$asm  = Join-Path $sc "asm"

# 1) Verificar SHFB
$shfbRoot = [Environment]::GetEnvironmentVariable('SHFBROOT','Machine')
if (-not $shfbRoot) { $shfbRoot = $env:SHFBROOT }
if (-not $shfbRoot -or -not (Test-Path $shfbRoot)) {
    throw "No se encontro SHFBROOT. Instala Sandcastle Help File Builder (MSI elevado) desde https://github.com/EWSoftware/SHFB/releases"
}
$env:SHFBROOT = $shfbRoot
Write-Host "SHFBROOT = $env:SHFBROOT" -ForegroundColor Cyan

# 2) Rearmar la carpeta de ensamblados (5 propios + dependencias) con sus XML de doc
Write-Host "Preparando ensamblados en $asm ..." -ForegroundColor Cyan
if (Test-Path $asm) { Remove-Item $asm -Recurse -Force }
New-Item -ItemType Directory -Force $asm | Out-Null

$inputDir = Join-Path $root "installer-input"
if (-not (Test-Path $inputDir)) {
    throw "No existe installer-input\. Ejecuta primero .\build-installer-input.ps1 en la raiz del repo."
}
Copy-Item (Join-Path $inputDir "*.dll") $asm -Force

# XML de documentacion (estan en bin\Release, no en installer-input)
$xmlSources = @(
    (Join-Path $root "Domain\bin\Release\net8.0\Domain.xml"),
    (Join-Path $root "Services\bin\Release\net8.0\Services.xml"),
    (Join-Path $root "DAL\bin\Release\net8.0\DAL.xml"),
    (Join-Path $root "BLL\bin\Release\net8.0\BLL.xml"),
    (Join-Path $root "UI\bin\Release\net8.0-windows\UI.xml")
)
foreach ($x in $xmlSources) {
    if (-not (Test-Path $x)) { throw "Falta el XML de documentacion: $x  (compila la solucion en Release)" }
    Copy-Item $x $asm -Force
}

# 3) Compilar la documentacion
Write-Host "Compilando la documentacion con SHFB..." -ForegroundColor Cyan
Push-Location $sc
try {
    & "dotnet" build "StockHelper.shfbproj" -c Release
    if ($LASTEXITCODE -ne 0) { throw "El build de SHFB fallo (codigo $LASTEXITCODE)" }
}
finally { Pop-Location }

$index = Join-Path $sc "Help\index.html"
Write-Host ""
Write-Host "OK. Documentacion generada:" -ForegroundColor Green
Write-Host "  $index" -ForegroundColor Green
