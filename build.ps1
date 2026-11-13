<#
  Script de compilacion automatica para Despierta
  1. Compila src/Program.cs a bin/despierta.exe
  2. Sincroniza el payload Base64 en instalar_despierta.ps1 y src/Installer.cs
  3. Compila src/Installer.cs a bin/Instalador-Despierta.exe
#>
$ErrorActionPreference = "Stop"

$scriptDir     = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcApp        = Join-Path $scriptDir "src\Program.cs"
$srcInstaller  = Join-Path $scriptDir "src\Installer.cs"
$binDir        = Join-Path $scriptDir "bin"
$binApp        = Join-Path $binDir "despierta.exe"
$binInstaller  = Join-Path $binDir "Instalador-Despierta.exe"
$psInstaller   = Join-Path $scriptDir "instalar_despierta.ps1"

if (-not (Test-Path $binDir)) {
    New-Item -ItemType Directory -Path $binDir -Force | Out-Null
}

$csc = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if (-not (Test-Path $csc)) {
    $csc = (Get-ChildItem -Path "C:\Windows\Microsoft.NET\Framework64" -Filter "csc.exe" -Recurse | Select-Object -First 1).FullName
}

# 1. Compilar aplicacion principal
Write-Host "[1/3] Compilando $srcApp ..." -ForegroundColor Cyan
& $csc /target:winexe /out:"$binApp" /platform:anycpu /optimize+ "$srcApp"
if ($LASTEXITCODE -ne 0) { throw "Fallo la compilacion de despierta.exe" }
$appSize = (Get-Item $binApp).Length
Write-Host "  -> Generado: $binApp ($appSize bytes)" -ForegroundColor Green

# 2. Sincronizar Base64 payload
Write-Host "[2/3] Sincronizando payload Base64 ..." -ForegroundColor Cyan
$b64 = [Convert]::ToBase64String([System.IO.File]::ReadAllBytes($binApp))

if (Test-Path $psInstaller) {
    $content = Get-Content -Path $psInstaller -Raw -Encoding UTF8
    $pattern = '(?s)\$b64\s*=\s*"[^"]*"'
    $newContent = [regex]::Replace($content, $pattern, "`$b64 = `"$b64`"")
    [System.IO.File]::WriteAllText($psInstaller, $newContent, [System.Text.Encoding]::UTF8)
    Write-Host "  -> instalar_despierta.ps1 actualizado" -ForegroundColor Green
}

if (Test-Path $srcInstaller) {
    $content = Get-Content -Path $srcInstaller -Raw -Encoding UTF8
    $pattern = '(?s)const\s+string\s+APP_PAYLOAD_B64\s*=\s*"[^"]*";'
    $newContent = [regex]::Replace($content, $pattern, "const string APP_PAYLOAD_B64 = `"$b64`";")
    [System.IO.File]::WriteAllText($srcInstaller, $newContent, [System.Text.Encoding]::UTF8)
    Write-Host "  -> src/Installer.cs actualizado" -ForegroundColor Green
}

# 3. Compilar instalador grafico autónomo
Write-Host "[3/3] Compilando instalador autonomo $srcInstaller ..." -ForegroundColor Cyan
& $csc /target:winexe /out:"$binInstaller" /platform:anycpu /optimize+ "$srcInstaller"
if ($LASTEXITCODE -ne 0) { throw "Fallo la compilacion de Instalador-Despierta.exe" }
$instSize = (Get-Item $binInstaller).Length
Write-Host "  -> Generado: $binInstaller ($instSize bytes)" -ForegroundColor Green

Write-Host ""
Write-Host "Compilacion completa y sincronizada!" -ForegroundColor Green
