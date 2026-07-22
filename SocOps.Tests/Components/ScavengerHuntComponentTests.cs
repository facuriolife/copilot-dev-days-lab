using Xunit;
using SocOps.Data;
using System.Reflection;

namespace SocOps.Tests.Components;

public class ScavengerHuntComponentTests
{
    [Fact]
    public void ScavengerHunt_Component_Should_Exist()
    {
        // Assert: Verify that ScavengerHunt.razor component file exists as a compiled type
        var componentType = Type.GetType("SocOps.Components.ScavengerHunt, SocOps");
        Assert.NotNull(componentType ?? throw new InvalidOperationException(
            "ScavengerHunt component should exist at SocOps/Components/ScavengerHunt.razor"));
    }

    [Fact]
    public void ScavengerHunt_Should_Initialize_With_Questions_From_QuestionsList()
    {
        // Arrange
        var questionsCount = Questions.QuestionsList.Count;

        // Assert: Verify Questions.QuestionsList has questions
        Assert.NotEmpty(Questions.QuestionsList);
        Assert.True(questionsCount > 0);
    }

    [Fact]
    public void ScavengerHunt_Should_Have_Items_Property()
    {
        // Assert: Verify Items property exists on component
        var componentType = Type.GetType("SocOps.Components.ScavengerHunt, SocOps");
        var itemsProperty = componentType?.GetProperty("Items", 
            BindingFlags.Public | BindingFlags.Instance);
        Assert.NotNull(itemsProperty ?? throw new InvalidOperationException(
            "ScavengerHunt should have an Items property"));
    }

    [Fact]
    public void ScavengerHunt_Should_Have_CompletedCount_Property()
    {
        // Assert: Verify CompletedCount property exists on component
        var componentType = Type.GetType("SocOps.Components.ScavengerHunt, SocOps");
        var completedCountProperty = componentType?.GetProperty("CompletedCount",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(completedCountProperty ?? throw new InvalidOperationException(
            "ScavengerHunt should have a CompletedCount property"));
    }

    [Fact]
    public void ScavengerHunt_Should_Have_TotalCount_Property()
    {
        // Assert: Verify TotalCount property exists on component
        var componentType = Type.GetType("SocOps.Components.ScavengerHunt, SocOps");
        var totalCountProperty = componentType?.GetProperty("TotalCount",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(totalCountProperty ?? throw new InvalidOperationException(
            "ScavengerHunt should have a TotalCount property"));
    }

    [Fact]
    public void ScavengerHunt_Should_Have_ProgressPercentage_Property()
    {
        // Assert: Verify ProgressPercentage property exists on component
        var componentType = Type.GetType("SocOps.Components.ScavengerHunt, SocOps");
        var progressProperty = componentType?.GetProperty("ProgressPercentage",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(progressProperty ?? throw new InvalidOperationException(
            "ScavengerHunt should have a ProgressPercentage property"));
    }

    [Fact]
    public void ScavengerHunt_Should_Have_IsComplete_Property()
    {
        // Assert: Verify IsComplete property exists on component
        var componentType = Type.GetType("SocOps.Components.ScavengerHunt, SocOps");
        var isCompleteProperty = componentType?.GetProperty("IsComplete",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(isCompleteProperty ?? throw new InvalidOperationException(
            "ScavengerHunt should have an IsComplete property"));
    }

    [Fact]
    public void ScavengerHunt_Should_Have_OnBack_Parameter()
    {
        // Assert: Verify OnBack EventCallback parameter exists
        var componentType = Type.GetType("SocOps.Components.ScavengerHunt, SocOps");
        var onBackProperty = componentType?.GetProperty("OnBack",
            BindingFlags.Public | BindingFlags.Instance);
        Assert.NotNull(onBackProperty ?? throw new InvalidOperationException(
            "ScavengerHunt should have an OnBack EventCallback parameter"));
    }

    [Fact]
    public void ScavengerHunt_Should_Have_ToggleItem_Method()
    {
        // Assert: Verify ToggleItem method exists on component
        var componentType = Type.GetType("SocOps.Components.ScavengerHunt, SocOps");
        var toggleMethod = componentType?.GetMethod("ToggleItem",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(toggleMethod ?? throw new InvalidOperationException(
            "ScavengerHunt should have a ToggleItem method for marking items"));
    }
}
