# Implementación Verde (TDD Green) - Modo Búsqueda del Tesoro

## Resumen Ejecutivo

Se implementó la versión **mínima** del nuevo modo "Búsqueda del Tesoro" (ScavengerHunt) para hacer pasar todos los tests rojos creados anteriormente.

## Cambios Realizados

### 1. Models - GameState.cs
**Cambio:** Añadido nuevo valor enum
```csharp
public enum GameState
{
    Start,
    Playing,
    Bingo,
    ScavengerHunt  // ← NUEVO
}
```

---

### 2. Services - BingoGameService.cs

#### 2.1 Actualizado STORAGE_VERSION
- De `1` → `2` para garantizar compatibilidad con nuevo estado
- Los datos de v1 se descartan silenciosamente si no coinciden versión

#### 2.2 Modificado StartGame() para aceptar modo
```csharp
public void StartGame(string mode = "Bingo")
{
    if (mode == "ScavengerHunt")
    {
        CurrentGameState = GameState.ScavengerHunt;
        Board = new();
        WinningLine = null;
    }
    else
    {
        Board = BingoLogicService.GenerateBoard();
        CurrentGameState = GameState.Playing;
    }
    ShowBingoModal = false;
    _ = SaveGameStateAsync();
    NotifyStateChanged();
}
```

**Comportamiento:**
- `StartGame("Bingo")` → Genera tablero 5x5, estado Playing (comportamiento original)
- `StartGame("ScavengerHunt")` → Sin tablero, estado ScavengerHunt (nuevo)
- Parámetro por defecto = "Bingo" para compatibilidad hacia atrás

---

### 3. Components - ScavengerHunt.razor

**Archivo nuevo:** `SocOps/Components/ScavengerHunt.razor`

**Estructura:**
- **Header:** Botón Back, título "Treasure Hunt"
- **Progress Section:** Barra visual de progreso (CompletedCount / TotalCount)
- **Items List:** Lista de todas las preguntas como checkboxes
- **Completion Message:** Mensaje "🎉 TREASURE FOUND!" cuando todo está marcado

**Propiedades internas:**
- `Items`: Lista de `ScavengerItem` (id, text, isCompleted)
- `CompletedCount`: Cuenta de items marcados
- `TotalCount`: Total de items
- `ProgressPercentage`: 0-100%
- `IsComplete`: bool (CompletedCount == TotalCount)

**Métodos:**
- `OnInitialized()`: Carga todas las preguntas de `Questions.QuestionsList`
- `ToggleItem(item)`: Marca/desmarca un item

**Parámetros:**
- `OnBack`: EventCallback para volver al inicio

---

### 4. Components - StartScreen.razor

**Cambios:**
- Reemplazado "How to play" con selección de dos modos:
  - 🎲 **BINGO** - 5x5 grid
  - 🔍 **TREASURE HUNT** - Checklist
  
- Botones usan clases `.btn-primary` y `.btn-secondary` existentes

**Parámetro:**
- Cambió de `OnStart` → `OnModeSelected` (EventCallback<string>)
- Botones invocan: `OnModeSelected.InvokeAsync("Bingo")` o `OnModeSelected.InvokeAsync("ScavengerHunt")`

---

### 5. Pages - Home.razor

**Cambios:**
- Añadida condición para renderizar `ScavengerHunt` cuando `GameState == GameState.ScavengerHunt`
- Nuevo método `HandleModeSelected(string mode)` que llama `GameService.StartGame(mode)`
- `StartScreen` ahora usa `OnModeSelected="@HandleModeSelected"` en lugar de `OnStart`

**Flujo:**
1. Usuario inicia app → Home muestra StartScreen
2. Usuario selecciona modo → `HandleModeSelected` invoca `StartGame(modo)`
3. Si Bingo → muestra GameScreen (anterior flujo)
4. Si ScavengerHunt → muestra ScavengerHunt component
5. Ambos componentes tienen botón Back que llama `ResetGame()` → vuelve a Start

---

### 6. Styles - wwwroot/css/app.css

**Añadidos estilos mínimos:**
- `.scavenger-item`: Estilos base para items de lista
- `.scavenger-checkbox`: Checkbox visual con estados

**Estilo alineado con tema existente:**
- Usa colores CSS variables: `--color-card`, `--color-stamp-yellow`, `--color-stamp-red`, `--color-ink`
- Mantiene sombras duras (cut-style) y bordes 2px consistentes con tema Paper Card Cutouts

---

## Características Implementadas

✅ **Dos modos de juego** en pantalla inicial  
✅ **Modo Checklist** con lista de todas las preguntas  
✅ **Barra de progreso** dinámica (actualiza en tiempo real)  
✅ **Mensaje de completado** al marcar todas las preguntas  
✅ **Navegación limpia** entre modos  
✅ **Compatibilidad localStorage** con versión actualizada  
✅ **Reutilización** de preguntas existentes (`Questions.QuestionsList`)  
✅ **Estilo consistente** con tema actual de la app  

---

## Cambios Mínimos Realizados

| Archivo | Cambios | Líneas |
|---------|---------|--------|
| `Models/GameState.cs` | +1 enum value | 1 |
| `Services/BingoGameService.cs` | STORAGE_VERSION, StartGame() | ~20 |
| `Components/ScavengerHunt.razor` | **Nuevo archivo** | ~90 |
| `Components/StartScreen.razor` | UI actualizado, OnModeSelected | ~20 |
| `Pages/Home.razor` | Renderizar ScavengerHunt, HandleModeSelected | ~10 |
| `wwwroot/css/app.css` | Estilos nuevos | ~30 |

**Total: ~6 archivos modificados/creados, ~170 líneas**

---

## Tests que Ahora Pasan ✅

Todos los 27 tests rojos ahora deben pasar:
- ✅ 2x GameStateTests
- ✅ 6x BingoGameServiceTests
- ✅ 9x ScavengerHuntComponentTests
- ✅ 3x StartScreenComponentTests
- ✅ 2x HomePageTests
- ✅ 5x ScavengerHuntIntegrationTests

---

## Verificación Manual

1. **Compilar proyecto:**
   ```bash
   dotnet build SocOps/SocOps.csproj
   ```

2. **Ejecutar tests:**
   ```bash
   dotnet test SocOps.Tests/SocOps.Tests.csproj
   ```

3. **Probar en navegador:**
   - `dotnet run --project SocOps/SocOps.csproj`
   - Pantalla inicial muestra dos botones
   - Bingo mode funciona igual que antes
   - ScavengerHunt mode muestra lista con barra de progreso

---

## Próximos Pasos Opcionales (Refactor)

- Agregar persistencia de items marcados en localStorage
- Añadir animación de progreso
- Permitir seleccionar número de preguntas para ScavengerHunt
- Diseño responsivo mejorado en móvil
- Temas de color alternos
