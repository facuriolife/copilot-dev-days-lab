using SocOps.Models;
using Xunit;

namespace SocOps.Tests.Models;

public class GameStateTests
{
    [Fact]
    public void GameState_Should_Have_ScavengerHunt_Value()
    {
        // Assert: Verify ScavengerHunt enum value exists
        Assert.True(Enum.GetNames(typeof(GameState)).Contains("ScavengerHunt"),
            "GameState enum should contain ScavengerHunt value");
    }

    [Fact]
    public void GameState_Should_Have_Required_Values()
    {
        // Arrange
        var gameStateValues = Enum.GetNames(typeof(GameState)).ToList();

        // Assert
        Assert.Contains("Start", gameStateValues);
        Assert.Contains("Playing", gameStateValues);
        Assert.Contains("Bingo", gameStateValues);
        Assert.Contains("ScavengerHunt", gameStateValues);
    }
}
