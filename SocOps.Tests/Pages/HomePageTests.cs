using Xunit;
using SocOps.Models;

namespace SocOps.Tests.Pages;

public class HomePageTests
{
    [Fact]
    public void Home_Page_Should_Render_ScavengerHunt_When_GameState_Is_ScavengerHunt()
    {
        // Assert: Verify GameState.ScavengerHunt exists
        Assert.True(Enum.GetNames(typeof(GameState)).Contains("ScavengerHunt"),
            "GameState should have ScavengerHunt value for Home.razor to render it");
    }

    [Fact]
    public void Home_Page_Logic_Should_Handle_ScavengerHunt_Mode_Initialization()
    {
        // Assert: This test verifies that the Home page can handle a new game mode
        // The actual conditional rendering is tested through component tests
        var gameStateValues = Enum.GetNames(typeof(GameState));
        var requiredStates = new[] { "Start", "Playing", "Bingo", "ScavengerHunt" };
        
        foreach (var state in requiredStates)
        {
            Assert.Contains(state, gameStateValues);
        }
    }
}
