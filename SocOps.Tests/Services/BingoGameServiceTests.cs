using SocOps.Models;
using SocOps.Services;
using Xunit;
using Moq;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;

namespace SocOps.Tests.Services;

public class BingoGameServiceTests
{
    private Mock<IJSRuntime> CreateMockJSRuntime()
    {
        var mock = new Mock<IJSRuntime>();
        mock.Setup(m => m.InvokeAsync<string?>(It.IsAny<string>(), It.IsAny<object[]>()))
            .ReturnsAsync((string?)null);
        mock.Setup(m => m.InvokeAsync<IJSVoidResult>(It.IsAny<string>(), It.IsAny<object[]>()))
            .ReturnsAsync((IJSVoidResult)null!);
        return mock;
    }

    [Fact]
    public void StartGame_Should_Accept_Mode_Parameter()
    {
        // Arrange
        var mockJSRuntime = CreateMockJSRuntime();
        var service = new BingoGameService(mockJSRuntime.Object);

        // Act & Assert: Method should accept a string parameter for mode
        var method = typeof(BingoGameService).GetMethod("StartGame");
        Assert.NotNull(method);
        var parameters = method.GetParameters();
        Assert.True(parameters.Length >= 1, "StartGame should accept at least one parameter");
    }

    [Fact]
    public void StartGame_With_ScavengerHunt_Mode_Should_Set_GameState_To_ScavengerHunt()
    {
        // Arrange
        var mockJSRuntime = CreateMockJSRuntime();
        var service = new BingoGameService(mockJSRuntime.Object);

        // Act
        var startGameMethod = typeof(BingoGameService).GetMethod("StartGame", new[] { typeof(string) });
        Assert.NotNull(startGameMethod);
        startGameMethod.Invoke(service, new object[] { "ScavengerHunt" });

        // Assert
        var currentGameStateProperty = typeof(BingoGameService).GetProperty("CurrentGameState");
        Assert.NotNull(currentGameStateProperty);
        var gameState = (GameState?)currentGameStateProperty.GetValue(service);
        Assert.Equal(GameState.ScavengerHunt, gameState);
    }

    [Fact]
    public void StartGame_With_Bingo_Mode_Should_Set_GameState_To_Playing()
    {
        // Arrange
        var mockJSRuntime = CreateMockJSRuntime();
        var service = new BingoGameService(mockJSRuntime.Object);

        // Act
        var startGameMethod = typeof(BingoGameService).GetMethod("StartGame", new[] { typeof(string) });
        Assert.NotNull(startGameMethod);
        startGameMethod.Invoke(service, new object[] { "Bingo" });

        // Assert
        var currentGameStateProperty = typeof(BingoGameService).GetProperty("CurrentGameState");
        Assert.NotNull(currentGameStateProperty);
        var gameState = (GameState?)currentGameStateProperty.GetValue(service);
        Assert.Equal(GameState.Playing, gameState);
    }

    [Fact]
    public void StartGame_With_ScavengerHunt_Should_Not_Generate_Board()
    {
        // Arrange
        var mockJSRuntime = CreateMockJSRuntime();
        var service = new BingoGameService(mockJSRuntime.Object);

        // Act
        var startGameMethod = typeof(BingoGameService).GetMethod("StartGame", new[] { typeof(string) });
        Assert.NotNull(startGameMethod);
        startGameMethod.Invoke(service, new object[] { "ScavengerHunt" });

        // Assert
        var boardProperty = typeof(BingoGameService).GetProperty("Board");
        Assert.NotNull(boardProperty);
        var board = (List<BingoSquareData>?)boardProperty.GetValue(service);
        Assert.Empty(board);
    }

    [Fact]
    public void StartGame_With_Bingo_Should_Generate_Board_With_25_Squares()
    {
        // Arrange
        var mockJSRuntime = CreateMockJSRuntime();
        var service = new BingoGameService(mockJSRuntime.Object);

        // Act
        var startGameMethod = typeof(BingoGameService).GetMethod("StartGame", new[] { typeof(string) });
        Assert.NotNull(startGameMethod);
        startGameMethod.Invoke(service, new object[] { "Bingo" });

        // Assert
        var boardProperty = typeof(BingoGameService).GetProperty("Board");
        Assert.NotNull(boardProperty);
        var board = (List<BingoSquareData>?)boardProperty.GetValue(service);
        Assert.NotEmpty(board);
        Assert.Equal(25, board.Count);
    }

    [Fact]
    public void ResetGame_Should_Return_To_Start_State()
    {
        // Arrange
        var mockJSRuntime = CreateMockJSRuntime();
        var service = new BingoGameService(mockJSRuntime.Object);
        var startGameMethod = typeof(BingoGameService).GetMethod("StartGame", new[] { typeof(string) });
        startGameMethod.Invoke(service, new object[] { "ScavengerHunt" });

        // Act
        var resetMethod = typeof(BingoGameService).GetMethod("ResetGame");
        resetMethod.Invoke(service, null);

        // Assert
        var currentGameStateProperty = typeof(BingoGameService).GetProperty("CurrentGameState");
        var gameState = (GameState?)currentGameStateProperty.GetValue(service);
        Assert.Equal(GameState.Start, gameState);
    }
}
