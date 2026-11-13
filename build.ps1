<#
  Script de compilacion automatica para Despierta
  Compila src/Program.cs a bin/despierta.exe usando el compilador nativo csc.exe de Windows
  y actualiza el payload Base64 dentro de instalar_despierta.ps1
#>
$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$srcFile   = Join-Path $scriptDir "src\Program.cs"
$binDir    = Join-Path $scriptDir "bin"
$binFile   = Join-Path $binDir "despierta.exe"
$installer = Join-Path $scriptDir "instalar_despierta.ps1"

if (-not (Test-Path $binDir)) {
    New-Item -ItemType Directory -Path $binDir -Force | Out-Null
}

$csc = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if (-not (Test-Path $csc)) {
    $csc = (Get-ChildItem -Path "C:\Windows\Microsoft.NET\Framework64" -Filter "csc.exe" -Recurse | Select-Object -First 1).FullName
}

Write-Host "Compilando $srcFile ..." -ForegroundColor Cyan
& $csc /target:winexe /out:"$binFile" /platform:anycpu /optimize+ "$srcFile"

if ($LASTEXITCODE -ne 0) {
    Write-Error "Fallo la compilacion de C#"
    exit 1
}

$fileSize = (Get-Item $binFile).Length
Write-Host "Compilacion exitosa: $binFile ($fileSize bytes)" -ForegroundColor Green

# Actualizar Base64 en el instalador si existe
if (Test-Path $installer) {
    Write-Host "Actualizando payload Base64 en $installer ..." -ForegroundColor Cyan
    $b64 = [Convert]::ToBase64String([System.IO.File]::ReadAllBytes($binFile))
    $content = Get-Content -Path $installer -Raw -Encoding UTF8
    $pattern = '(?s)\$b64\s*=\s*"[^"]*"'
    $replacement = "`$b64 = `"$b64`""
    $newContent = [regex]::Replace($content, $pattern, $replacement)
    [System.IO.File]::WriteAllText($installer, $newContent, [System.Text.Encoding]::UTF8)
    Write-Host "Instalador sincronizado con el nuevo binario." -ForegroundColor Green
}

