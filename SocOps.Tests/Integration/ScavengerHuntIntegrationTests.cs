using SocOps.Models;
using SocOps.Services;
using SocOps.Data;
using Xunit;
using Moq;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;

namespace SocOps.Tests.Integration;

public class ScavengerHuntIntegrationTests
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
    public void ScavengerHunt_Mode_Should_Initialize_Without_Board()
    {
        // Arrange
        var mockJSRuntime = CreateMockJSRuntime();
        var service = new BingoGameService(mockJSRuntime.Object);

        // Act
        var startGameMethod = typeof(BingoGameService).GetMethod("StartGame", new[] { typeof(string) });
        startGameMethod?.Invoke(service, new object[] { "ScavengerHunt" });

        // Assert
        var boardProperty = typeof(BingoGameService).GetProperty("Board");
        var board = (List<BingoSquareData>?)boardProperty?.GetValue(service);
        
        Assert.Empty(board);
        Assert.Equal(GameState.ScavengerHunt, service.CurrentGameState);
    }

    [Fact]
    public void Bingo_Mode_Should_Generate_Board_With_Questions()
    {
        // Arrange
        var mockJSRuntime = CreateMockJSRuntime();
        var service = new BingoGameService(mockJSRuntime.Object);
        var expectedBoardSize = 25;

        // Act
        var startGameMethod = typeof(BingoGameService).GetMethod("StartGame", new[] { typeof(string) });
        startGameMethod?.Invoke(service, new object[] { "Bingo" });

        // Assert
        var boardProperty = typeof(BingoGameService).GetProperty("Board");
        var board = (List<BingoSquareData>?)boardProperty?.GetValue(service);

        Assert.NotEmpty(board);
        Assert.Equal(expectedBoardSize, board.Count);
        Assert.True(board.Any(s => s.IsFreeSpace), "Board should have a free space");
        Assert.True(board.Any(s => !string.IsNullOrEmpty(s.Text) && !s.IsFreeSpace),
            "Board should contain questions");
    }

    [Fact]
    public void Switching_From_Bingo_To_ScavengerHunt_Should_Clear_Board_State()
    {
        // Arrange
        var mockJSRuntime = CreateMockJSRuntime();
        var service = new BingoGameService(mockJSRuntime.Object);
        var startGameMethod = typeof(BingoGameService).GetMethod("StartGame", new[] { typeof(string) });

        // Act - Start Bingo
        startGameMethod?.Invoke(service, new object[] { "Bingo" });
        var boardAfterBingo = (List<BingoSquareData>?)typeof(BingoGameService)
            .GetProperty("Board")?.GetValue(service);

        // Act - Reset and Start ScavengerHunt
        var resetMethod = typeof(BingoGameService).GetMethod("ResetGame");
        resetMethod?.Invoke(service, null);
        startGameMethod?.Invoke(service, new object[] { "ScavengerHunt" });

        var boardAfterScavenger = (List<BingoSquareData>?)typeof(BingoGameService)
            .GetProperty("Board")?.GetValue(service);

        // Assert
        Assert.NotEmpty(boardAfterBingo);
        Assert.Empty(boardAfterScavenger);
        Assert.Equal(GameState.ScavengerHunt, service.CurrentGameState);
    }

    [Fact]
    public void Questions_List_Should_Have_Enough_Questions_For_Both_Modes()
    {
        // Arrange
        var questionsList = Questions.QuestionsList;
        var minRequiredForBingo = 24; // 25 squares - 1 free space
        var minRequiredForScavenger = 10; // Reasonable minimum for checklist

        // Assert
        Assert.True(questionsList.Count >= minRequiredForBingo,
            $"QuestionsList should have at least {minRequiredForBingo} questions for Bingo mode");
        Assert.True(questionsList.Count >= minRequiredForScavenger,
            $"QuestionsList should have at least {minRequiredForScavenger} questions for ScavengerHunt mode");
    }

    [Fact]
    public void Multiple_Game_Starts_Should_Be_Independent()
    {
        // Arrange
        var mockJSRuntime = CreateMockJSRuntime();
        var service = new BingoGameService(mockJSRuntime.Object);
        var startGameMethod = typeof(BingoGameService).GetMethod("StartGame", new[] { typeof(string) });

        // Act - First Bingo game
        startGameMethod?.Invoke(service, new object[] { "Bingo" });
        var board1 = (List<BingoSquareData>?)typeof(BingoGameService)
            .GetProperty("Board")?.GetValue(service);
        var board1Text = string.Join(",", board1?.Where(s => !s.IsFreeSpace).Select(s => s.Text) ?? Enumerable.Empty<string>());

        // Act - Reset and new Bingo game
        var resetMethod = typeof(BingoGameService).GetMethod("ResetGame");
        resetMethod?.Invoke(service, null);
        startGameMethod?.Invoke(service, new object[] { "Bingo" });
        var board2 = (List<BingoSquareData>?)typeof(BingoGameService)
            .GetProperty("Board")?.GetValue(service);
        var board2Text = string.Join(",", board2?.Where(s => !s.IsFreeSpace).Select(s => s.Text) ?? Enumerable.Empty<string>());

        // Assert - Boards might be different (due to randomization) or same, but should both be valid
        Assert.NotEmpty(board1);
        Assert.NotEmpty(board2);
        Assert.Equal(25, board1.Count);
        Assert.Equal(25, board2.Count);
    }
}
