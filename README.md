# Modo Despierta (Windows Keep-Awake) ☕

Microaplicación nativa y ultra ligera en C# (7 KB) para Windows que evita que la computadora se suspenda o apague la pantalla cuando estás trabajando, utilizando la API oficial del kernel de Windows `SetThreadExecutionState`.

## Características
- **Eficiente y nativa**: No recurre a "mouse jigglers" ni simulaciones de pulsaciones de teclas; utiliza la instrucción oficial de energía del sistema operativo.
- **Acceso instantáneo**: Presiona `Win + R`, escribe `despierta` y presiona Enter.
- **Icono en la bandeja del sistema (Systray)**: Indicador visual en forma de taza de café. Con un solo clic sobre el icono se desactiva y restaura el estado original del sistema.
- **Instalación sin privilegios de administrador**: Funciona completamente en el entorno de usuario.

---

## Instalación rápida (1 línea en PowerShell)

Para instalar en esta o en cualquier otra computadora con Windows:

```powershell
irm https://raw.githubusercontent.com/worm91/despierta-windows/main/instalar_despierta.ps1 | iex
```

---

## Cómo usarlo

1. Presiona `Win + R`.
2. Escribe `despierta` y presiona `Enter`.
3. Verás una notificación y el icono de café en el Systray indicando que el equipo se mantendrá despierto.
4. Para desactivarlo, simplemente haz clic izquierdo o derecho en el icono y selecciona salir.

---

## Desinstalación limpia

Si alguna vez deseas desinstalarlo por completo:

```powershell
irm https://raw.githubusercontent.com/worm91/despierta-windows/main/desinstalar_despierta.ps1 | iex
```

---

## Estructura del repositorio

- `src/Program.cs`: Código fuente en C# WinForms con control de instancia única (Mutex) y diseño vectorial del icono.
- `bin/despierta.exe`: Ejecutable compilado listo para usar.
- `build.ps1`: Script de compilación usando el compilador .NET incluido por defecto en Windows (`csc.exe`).
- `instalar_despierta.ps1`: Script instalador autónomo (contiene el payload para despliegue sin dependencias).
- `desinstalar_despierta.ps1`: Script para remover limpiamente binarios y entradas de registro.
