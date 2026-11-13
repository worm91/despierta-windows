# Modo Despierta (Windows Keep-Awake) ☕

Microaplicación nativa y ultra ligera en C# para Windows que evita que la computadora se suspenda o apague la pantalla cuando estás ocupado o ejecutando tareas largas, utilizando la API oficial del kernel de Windows `SetThreadExecutionState`.

---

## Métodos de Instalación

### Opción 1: Instalador Gráfico Autónomo (`.exe`)
Si prefieres un instalador clásico ejecutable con ventana visual:
1. Descarga el ejecutable desde la carpeta `bin/` o GitHub Releases:
   👉 **`bin/Instalador-Despierta.exe`** (54 KB)
2. Haz doble clic y presiona **Instalar**.
3. *Soporte silencioso para scripts:* `Instalador-Despierta.exe /silent` (o `/uninstall` para desinstalar).

### Opción 2: Instalación rápida por consola (PowerShell)
En cualquier PC con Windows, abre PowerShell y ejecuta:
```powershell
irm https://raw.githubusercontent.com/worm91/despierta-windows/main/instalar_despierta.ps1 | iex
```

---

## Cómo usarlo

1. Presiona `Win + R`.
2. Escribe `despierta` y presiona `Enter`.
3. Aparecerá el icono del café en la bandeja del sistema (Systray) y una notificación indicando que el sistema no se suspenderá.
4. Para desactivarlo, haz un solo clic en el icono del Systray o clic derecho -> *Desactivar y salir*.

---

## Desinstalación limpia

- **Desde el instalador:** Abre `Instalador-Despierta.exe` y haz clic en *Desinstalar*.
- **Desde Windows:** Aparece en **Configuración > Aplicaciones > Aplicaciones instaladas** como `Modo Despierta`.
- **Por PowerShell:**
  ```powershell
  irm https://raw.githubusercontent.com/worm91/despierta-windows/main/desinstalar_despierta.ps1 | iex
  ```

---

## Estructura del repositorio

- `src/Program.cs`: Código fuente de la microapp residente en Systray (7 KB).
- `src/Installer.cs`: Código fuente del instalador gráfico autónomo en C# WinForms.
- `bin/despierta.exe`: Binario compilado de la aplicación.
- `bin/Instalador-Despierta.exe`: Instalador ejecutable listo para descargar y distribuir.
- `build.ps1`: Script que recompila ambos binarios y sincroniza los instaladores automáticamente.
- `instalar_despierta.ps1`: Script instalador autónomo para PowerShell.
- `desinstalar_despierta.ps1`: Script desinstalador para PowerShell.
