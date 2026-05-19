# Instrucciones de Copilot para este repositorio

Estas instrucciones ayudan a los agentes a ser productivos rápidamente en este proyecto Soc Ops (Blazor WebAssembly).

## Objetivo del proyecto
- Aplicación de bingo social para eventos presenciales.
- Frontend en Blazor WebAssembly con lógica de juego en servicios C#.

## Comandos recomendados
- Restaurar/build:
  - `dotnet build SocOps/SocOps.csproj`
- Ejecutar app:
  - `dotnet run --project SocOps/SocOps.csproj`

Si hay tareas de VS Code disponibles, prioriza:
- `dotnet: build`
- `dotnet: run`

## Arquitectura (resumen)
- Entrada de app: `SocOps/Program.cs`
- Ruta principal: `SocOps/Pages/Home.razor`
- Componentes UI: `SocOps/Components/`
- Servicios de dominio:
  - `SocOps/Services/BingoGameService.cs` (estado de juego + persistencia localStorage)
  - `SocOps/Services/BingoLogicService.cs` (lógica pura de bingo)
- Modelos: `SocOps/Models/`
- Datos de preguntas: `SocOps/Data/Questions.cs`
- Estilos utilitarios: `SocOps/wwwroot/css/app.css`

## Convenciones importantes
- Mantén la separación actual:
  - UI en componentes Razor
  - Estado/orquestación en `BingoGameService`
  - Reglas puras de negocio en `BingoLogicService`
- Conserva la grilla 5x5 y el centro como free space cuando cambies lógica de tablero.
- Cambios de estado deben notificar vía `OnStateChanged` para refrescar UI.
- La persistencia usa `localStorage` con versionado (`STORAGE_VERSION`); mantén compatibilidad cuando ajustes el modelo guardado.

## Estilos y frontend
- Usa utilidades CSS existentes de `SocOps/wwwroot/css/app.css`.
- Si falta una utilidad, agrégala siguiendo el patrón actual en ese mismo archivo.
- Antes de introducir estilos nuevos, revisa:
  - `.github/instructions/css-utilities.instructions.md`
  - `.github/instructions/frontend-design.instructions.md`

## Alcance de cambios
- Evita editar `.solutions/` salvo que el usuario lo pida explícitamente.
- Para cambios funcionales, prioriza archivos bajo `SocOps/`.

## Referencias (enlazar, no duplicar)
- Visión general y ejecución: [README.md](../README.md)
- Guía de laboratorio: [workshop/GUIDE.md](../workshop/GUIDE.md)
- Contribución: [CONTRIBUTING.md](../CONTRIBUTING.md)

---

## Diseño

**Tema activo:** Paper Card Cutouts — estética de recortes de papel superpuestos con sombras duras, tipografía de pantalla y paleta parchment/ink.

### Tokens CSS (en `SocOps/wwwroot/css/app.css`, bloque `:root`)

| Variable | Valor | Uso |
|---|---|---|
| `--color-parchment` | `#F0E9D8` | Fondo global de la app |
| `--color-card` | `#FFFDF7` | Superficie de tarjetas y botones secundarios |
| `--color-ink` | `#1C1B18` | Texto principal, bordes y sombras |
| `--color-ink-muted` | `#6B6760` | Texto secundario / labels |
| `--color-stamp-yellow` | `#F5C842` | Casilla marcada |
| `--color-stamp-red` | `#CC3B25` | Casilla ganadora, banner bingo, modal |
| `--font-display` | Fraunces, Georgia, serif | Títulos y decoración |
| `--font-body` | DM Sans, system-ui | Texto de interfaz |

### Reglas de composición

- **Fondos:** usa siempre `.paper-bg` en contenedores raíz; usa `.bg-card` en header y paneles.
- **Tarjetas:** combina `.card-paper` + `.shadow-cut-lg` para tarjetas principales; `.card-inset` para bloques anidados.
- **Sombras:** SIEMPRE duras (sin blur): `.shadow-cut-sm` (2px), `.shadow-cut` (4px), `.shadow-cut-lg` (6px). No uses `box-shadow` con blur en este tema.
- **Bordes:** `border: 2px solid var(--color-ink)` en todos los elementos con profundidad; usa `.border-b-ink` para divisores de header.
- **Tipografía:** `font-display` + `font-black` para headings; `tracking-widest` + `uppercase` para labels pequeños en `text-ink-muted`.
- **Botones:** `.btn-primary` (dark fill, press shadow) para CTA; `.btn-secondary` (card bg, cut shadow) para acciones secundarias. No uses clases como `bg-accent` ni `rounded-lg` en elementos nuevos.

### Estados de BingoSquare

| Clase | Estado | Visual |
|---|---|---|
| `sq-base sq-normal` | Sin marcar | Fondo card, borde ink, sombra 2px, hover parchment |
| `sq-base sq-marked` | Marcada | Fondo amarillo stamp, borde ink, negrita |
| `sq-base sq-free` | Free space (centro) | Fondo ink, texto parchment, estrella ★, sin sombra |
| `sq-base sq-winning` | Línea ganadora | Fondo stamp-red, texto blanco, sombra 3px, animación pop |

### Animaciones permitidas

- `.animate-board-in` — entrada suave del tablero al cargar.
- `.animate-modal-pop` — aparición del modal con escala y rebote.
- `sq-win-pop` — pop en casillas ganadoras (sólo via `.sq-winning`).
- Botones: press effect via `:active` (`translate + shadow`). No añadir otras animaciones sin razón visual clara.
