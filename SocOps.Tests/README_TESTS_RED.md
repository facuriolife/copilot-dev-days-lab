# Tests Rojos (TDD Red) - Modo Búsqueda del Tesoro

## Resumen
Conjunto completo de tests que verifican el comportamiento esperado del nuevo modo "Búsqueda del Tesoro" (ScavengerHunt). Todos estos tests **deben fallar** contra el código actual porque las features aún no están implementadas.

## Estructura de Tests

### 1. GameStateTests (`Models/GameStateTests.cs`)
**Propósito:** Verificar que el estado del juego pueda soportar el nuevo modo.

Tests:
- `GameState_Should_Have_ScavengerHunt_Value()`: Valida que exista `GameState.ScavengerHunt`
- `GameState_Should_Have_Required_Values()`: Verifica todos los estados necesarios

**Estado Esperado:** FALLA (GameState.ScavengerHunt no existe)

---

### 2. BingoGameServiceTests (`Services/BingoGameServiceTests.cs`)
**Propósito:** Verificar que el servicio de juego pueda iniciar diferentes modos.

Tests:
- `StartGame_Should_Accept_Mode_Parameter()`: Valida que StartGame acepte un parámetro de modo
- `StartGame_With_ScavengerHunt_Mode_Should_Set_GameState_To_ScavengerHunt()`: Inicia modo ScavengerHunt
- `StartGame_With_Bingo_Mode_Should_Set_GameState_To_Playing()`: Inicia modo Bingo
- `StartGame_With_ScavengerHunt_Should_Not_Generate_Board()`: El modo checklist no genera tablero
- `StartGame_With_Bingo_Should_Generate_Board_With_25_Squares()`: El modo bingo genera tablero 5x5
- `ResetGame_Should_Return_To_Start_State()`: Reset regresa a estado inicial

**Estado Esperado:** FALLA (StartGame no acepta parámetro de modo)

---

### 3. ScavengerHuntComponentTests (`Components/ScavengerHuntComponentTests.cs`)
**Propósito:** Verificar que el componente de checklist exista con toda su funcionalidad.

Tests:
- `ScavengerHunt_Component_Should_Exist()`: Componente compilado existe
- `ScavengerHunt_Should_Initialize_With_Questions_From_QuestionsList()`: Usa preguntas existentes
- `ScavengerHunt_Should_Have_Items_Property()`: Propiedad Items existe
- `ScavengerHunt_Should_Have_CompletedCount_Property()`: Propiedad CompletedCount existe
- `ScavengerHunt_Should_Have_TotalCount_Property()`: Propiedad TotalCount existe
- `ScavengerHunt_Should_Have_ProgressPercentage_Property()`: Propiedad ProgressPercentage existe
- `ScavengerHunt_Should_Have_IsComplete_Property()`: Propiedad IsComplete existe
- `ScavengerHunt_Should_Have_OnBack_Parameter()`: Parámetro OnBack para volver atrás
- `ScavengerHunt_Should_Have_ToggleItem_Method()`: Método ToggleItem para marcar items

**Estado Esperado:** FALLA (Componente no existe)

---

### 4. StartScreenComponentTests (`Components/StartScreenComponentTests.cs`)
**Propósito:** Verificar que la pantalla inicial permita seleccionar entre modos.

Tests:
- `StartScreen_Should_Have_ModeSelected_Callback()`: Callback para selección de modo
- `StartScreen_Should_Allow_Mode_Selection_Between_Bingo_And_ScavengerHunt()`: Soporta ambos modos
- `StartScreen_Markup_Should_Render_Multiple_Game_Modes()`: Componente se puede compilar

**Estado Esperado:** FALLA (OnModeSelected no existe, solo OnStart)

---

### 5. HomePageTests (`Pages/HomePageTests.cs`)
**Propósito:** Verificar que la página principal maneja el nuevo estado.

Tests:
- `Home_Page_Should_Render_ScavengerHunt_When_GameState_Is_ScavengerHunt()`: GameState.ScavengerHunt existe
- `Home_Page_Logic_Should_Handle_ScavengerHunt_Mode_Initialization()`: Todos los estados necesarios existen

**Estado Esperado:** FALLA (GameState.ScavengerHunt no existe)

---

### 6. ScavengerHuntIntegrationTests (`Integration/ScavengerHuntIntegrationTests.cs`)
**Propósito:** Verificar el flujo completo de juego en modo búsqueda.

Tests:
- `ScavengerHunt_Mode_Should_Initialize_Without_Board()`: Modo checklist no crea tablero
- `Bingo_Mode_Should_Generate_Board_With_Questions()`: Modo bingo sigue generando tablero
- `Switching_From_Bingo_To_ScavengerHunt_Should_Clear_Board_State()`: Cambio limpio entre modos
- `Questions_List_Should_Have_Enough_Questions_For_Both_Modes()`: Suficientes preguntas
- `Multiple_Game_Starts_Should_Be_Independent()`: Múltiples juegos funcionen correctamente

**Estado Esperado:** FALLA (StartGame no maneja modo parámetro)

---

## Convenciones Seguidas

✅ **Framework:** xUnit (estándar moderno .NET)  
✅ **Mocking:** Moq para IJSRuntime  
✅ **Reflection:** Para verificar propiedades/métodos sin implementación  
✅ **Nomenclatura:** `Should_ExpectedBehavior_When_Scenario`  
✅ **Estilo C#:** Implicit usings, nullable enabled, net10.0  

---

## Próximos Pasos (Implementación Verde/Refactor)

1. Implementar `GameState.ScavengerHunt` en Models
2. Actualizar `BingoGameService.StartGame()` para aceptar modo
3. Crear componente `ScavengerHunt.razor`
4. Actualizar `StartScreen.razor` con selección de modo
5. Modificar `Home.razor` para renderizar ScavengerHunt
6. Agregar estilos CSS en `app.css`

Todos los tests entonces pasarán (GREEN) y se podrá refactorizar si es necesario.
