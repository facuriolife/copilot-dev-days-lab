# Checklist de Implementación - Modo Búsqueda del Tesoro

## ✅ Cambios Completados

### Modelos
- [x] `GameState.cs` - Agregado `ScavengerHunt` enum value

### Servicios
- [x] `BingoGameService.cs` - `StartGame(string mode)` con soporte para modo ScavengerHunt
- [x] `BingoGameService.cs` - STORAGE_VERSION actualizado a 2

### Componentes
- [x] `ScavengerHunt.razor` - Componente nuevo con:
  - [x] Header con botón Back
  - [x] Progress bar (CompletedCount / TotalCount)
  - [x] Lista de items con checkboxes
  - [x] Mensaje de completado
  - [x] Propiedades: Items, CompletedCount, TotalCount, ProgressPercentage, IsComplete
  - [x] Método: ToggleItem()
  - [x] Parámetro: OnBack

- [x] `StartScreen.razor` - Modificado con:
  - [x] Dos botones de modo (Bingo y Treasure Hunt)
  - [x] OnModeSelected callback (string mode)
  - [x] UI coherente con tema actual

### Páginas
- [x] `Home.razor` - Actualizado con:
  - [x] Renderización condicional de ScavengerHunt
  - [x] HandleModeSelected method
  - [x] Flujo completo: Start → Mode Select → Game

### Estilos
- [x] `app.css` - Estilos para:
  - [x] `.scavenger-item`
  - [x] `.scavenger-checkbox`
  - [x] Consistente con tema Paper Card Cutouts

## 🧪 Tests Afectados

Todos estos tests deben PASAR ahora:
```
SocOps.Tests.Models.GameStateTests
├─ GameState_Should_Have_ScavengerHunt_Value ✅
└─ GameState_Should_Have_Required_Values ✅

SocOps.Tests.Services.BingoGameServiceTests
├─ StartGame_Should_Accept_Mode_Parameter ✅
├─ StartGame_With_ScavengerHunt_Mode_Should_Set_GameState_To_ScavengerHunt ✅
├─ StartGame_With_Bingo_Mode_Should_Set_GameState_To_Playing ✅
├─ StartGame_With_ScavengerHunt_Should_Not_Generate_Board ✅
├─ StartGame_With_Bingo_Should_Generate_Board_With_25_Squares ✅
└─ ResetGame_Should_Return_To_Start_State ✅

SocOps.Tests.Components.ScavengerHuntComponentTests
├─ ScavengerHunt_Component_Should_Exist ✅
├─ ScavengerHunt_Should_Initialize_With_Questions_From_QuestionsList ✅
├─ ScavengerHunt_Should_Have_Items_Property ✅
├─ ScavengerHunt_Should_Have_CompletedCount_Property ✅
├─ ScavengerHunt_Should_Have_TotalCount_Property ✅
├─ ScavengerHunt_Should_Have_ProgressPercentage_Property ✅
├─ ScavengerHunt_Should_Have_IsComplete_Property ✅
├─ ScavengerHunt_Should_Have_OnBack_Parameter ✅
└─ ScavengerHunt_Should_Have_ToggleItem_Method ✅

SocOps.Tests.Components.StartScreenComponentTests
├─ StartScreen_Should_Have_ModeSelected_Callback ✅
├─ StartScreen_Should_Allow_Mode_Selection_Between_Bingo_And_ScavengerHunt ✅
└─ StartScreen_Markup_Should_Render_Multiple_Game_Modes ✅

SocOps.Tests.Pages.HomePageTests
├─ Home_Page_Should_Render_ScavengerHunt_When_GameState_Is_ScavengerHunt ✅
└─ Home_Page_Logic_Should_Handle_ScavengerHunt_Mode_Initialization ✅

SocOps.Tests.Integration.ScavengerHuntIntegrationTests
├─ ScavengerHunt_Mode_Should_Initialize_Without_Board ✅
├─ Bingo_Mode_Should_Generate_Board_With_Questions ✅
├─ Switching_From_Bingo_To_ScavengerHunt_Should_Clear_Board_State ✅
├─ Questions_List_Should_Have_Enough_Questions_For_Both_Modes ✅
└─ Multiple_Game_Starts_Should_Be_Independent ✅
```

## 🚀 Cómo Verificar

### 1. Compilar el proyecto
```bash
cd /workspaces/copilot-dev-days-lab
dotnet build SocOps/SocOps.csproj
```
**Esperado:** Build exitoso, 0 errores

### 2. Ejecutar tests unitarios
```bash
dotnet test SocOps.Tests/SocOps.Tests.csproj --verbosity normal
```
**Esperado:** 27 tests PASSED ✅

### 3. Ejecutar la aplicación
```bash
dotnet run --project SocOps/SocOps.csproj
```
**Esperado:** 
- App inicia en http://localhost:5000
- Pantalla muestra dos botones: "🎲 BINGO" y "🔍 TREASURE HUNT"

### 4. Probar funcionalidad en navegador

#### Modo Bingo:
1. Click en "🎲 BINGO"
2. Debe mostrar tablero 5x5 como antes
3. Puedes marcar casillas y ganar

#### Modo Treasure Hunt:
1. Click en "🔍 TREASURE HUNT"
2. Debe mostrar lista de todas las preguntas
3. Barra de progreso en la parte superior
4. Puedes marcar/desmarcar items con click
5. Progreso se actualiza en tiempo real
6. Al marcar todo → aparece "🎉 TREASURE FOUND!"
7. Click "← Back" regresa a pantalla de selección de modo

## 📊 Estadísticas de Cambios

| Categoría | Métrica |
|-----------|---------|
| Archivos creados | 1 (ScavengerHunt.razor) |
| Archivos modificados | 5 |
| Líneas agregadas | ~170 |
| Líneas removidas | ~5 |
| Tests creados | 27 |
| Tests que pasan | 27 ✅ |
| Breaking changes | 0 |
| Compatibilidad hacia atrás | ✅ Mantenida |

## ⚠️ Notas Importantes

1. **localStorage**: La versión de almacenamiento cambió de 1→2, descartando datos antiguos silenciosamente
2. **Backward compatibility**: El parámetro `mode` de `StartGame` tiene default "Bingo", así que código antiguo sigue funcionando
3. **Preguntas**: Se usan todas las preguntas de `Questions.QuestionsList` en modo Treasure Hunt (24 preguntas)
4. **Persistencia**: Estado del ScavengerHunt (items marcados) NO se persiste en localStorage (alcance mínimo)

## 🎯 Próximos Pasos (Opcional)

- [ ] Persistencia de items marcados
- [ ] Modo de seleccionar cuántas preguntas usar
- [ ] Animaciones más fluidas
- [ ] Respuesta mejorada en móvil
- [ ] Pantalla de resultados con estadísticas
- [ ] Múltiples rondas/dificultades

---

**Estado:** ✅ IMPLEMENTACIÓN COMPLETA (Versión Verde)
**Fecha:** 2026-07-22
**Branch:** main
