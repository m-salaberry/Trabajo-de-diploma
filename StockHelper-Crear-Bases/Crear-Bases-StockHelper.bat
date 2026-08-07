@echo off
REM ====================================================================
REM  StockHelper - Alta de bases de datos (post-instalacion)
REM  Doble clic para ejecutar. Se eleva a administrador automaticamente.
REM ====================================================================

REM --- Comprobar permisos de administrador ---
net session >nul 2>&1
if %errorlevel% neq 0 (
    echo Solicitando permisos de administrador...
    powershell -NoProfile -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)

REM --- Ya elevado: ejecutar el script de PowerShell ---
cd /d "%~dp0"
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0Crear-Bases-StockHelper.ps1" %*

echo.
pause
