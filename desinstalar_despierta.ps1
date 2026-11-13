<#
  Desinstalador de Modo Despierta (Windows Keep-Awake)
  Uso: irm <URL>/desinstalar_despierta.ps1 | iex
#>
$ErrorActionPreference = "SilentlyContinue"

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "   Desinstalando Modo Despierta ☕                        " -ForegroundColor Yellow
Write-Host "==========================================================" -ForegroundColor Cyan

# 1. Terminar proceso si esta corriendo
$proc = Get-Process -Name despierta -ErrorAction SilentlyContinue
if ($proc) {
    Write-Host "[1/4] Cerrando proceso activo..." -ForegroundColor Gray
    $proc | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep -Milliseconds 300
}

# 2. Eliminar accesos directos de Win + R (App Paths)
Write-Host "[2/4] Eliminando registro de comando Win + R..." -ForegroundColor Gray
Remove-Item -Path "HKCU:\Software\Microsoft\Windows\CurrentVersion\App Paths\despierta.exe" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "HKCU:\Software\Microsoft\Windows\CurrentVersion\App Paths\despierta" -Recurse -Force -ErrorAction SilentlyContinue

# 3. Limpiar variable PATH de usuario
Write-Host "[3/4] Limpiando variable PATH de usuario..." -ForegroundColor Gray
$targetDir = Join-Path $env:LOCALAPPDATA "Despierta"
$userPath = [Environment]::GetEnvironmentVariable("Path", "User")
if ($userPath -match [regex]::Escape($targetDir)) {
    $cleanPath = ($userPath -split ';' | Where-Object { $_ -and ($_ -ne $targetDir) }) -join ';'
    [Environment]::SetEnvironmentVariable("Path", $cleanPath, "User")
}

# 4. Eliminar carpeta en AppData
Write-Host "[4/4] Eliminando archivos locales..." -ForegroundColor Gray
if (Test-Path $targetDir) {
    Remove-Item -Path $targetDir -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Host ""
Write-Host " [OK] Desinstalacion completada con exito." -ForegroundColor Green
Write-Host "==========================================================" -ForegroundColor Cyan

