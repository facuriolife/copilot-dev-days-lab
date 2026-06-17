using System.Collections;
using System.Reflection;
using Microsoft.JSInterop;
using SocOps.Data;
using SocOps.Models;
using SocOps.Services;

namespace SocOps.Tests;

public class ScavengerHuntPlanTests
{
    [Fact]
    public void GameMode_enum_exists_with_bingo_and_scavenger_hunt_values()
    {
        var gameModeType = typeof(BingoGameService).Assembly.GetType("SocOps.Models.GameMode");

        Assert.NotNull(gameModeType);
        Assert.True(gameModeType!.IsEnum);
        Assert.Contains("Bingo", Enum.GetNames(gameModeType));
        Assert.Contains("ScavengerHunt", Enum.GetNames(gameModeType));
    }

    [Fact]
    public void GameState_enum_includes_scavenger_hunt_state()
    {
        Assert.Contains("ScavengerHunt", Enum.GetNames(typeof(GameState)));
    }

    [Fact]
    public void BingoGameService_can_start_scavenger_hunt_mode()
    {
        var jsRuntime = new RecordingJsRuntime();
        var service = new BingoGameService(jsRuntime);
        var gameModeType = typeof(BingoGameService).Assembly.GetType("SocOps.Models.GameMode");
        var currentGameModeProperty = typeof(BingoGameService).GetProperty("CurrentGameMode");

        Assert.NotNull(gameModeType);
        Assert.NotNull(currentGameModeProperty);

        var startGame = typeof(BingoGameService).GetMethod("StartGame", [gameModeType!]);

        Assert.NotNull(startGame);

        var scavengerHuntMode = Enum.Parse(gameModeType!, "ScavengerHunt");
        startGame!.Invoke(service, [scavengerHuntMode]);

        Assert.Equal("ScavengerHunt", service.CurrentGameState.ToString());
        Assert.Equal("ScavengerHunt", currentGameModeProperty!.GetValue(service)?.ToString());
        Assert.False(service.ShowBingoModal);
    }

    [Fact]
    public void BingoGameService_persists_game_mode_and_uses_a_new_storage_version()
    {
        var jsRuntime = new RecordingJsRuntime();
        var service = new BingoGameService(jsRuntime);
        var serviceType = typeof(BingoGameService);
        var gameModeType = serviceType.Assembly.GetType("SocOps.Models.GameMode");
        var startGame = gameModeType is null ? null : serviceType.GetMethod("StartGame", [gameModeType]);
        var storageVersion = serviceType.GetField("STORAGE_VERSION", BindingFlags.NonPublic | BindingFlags.Static);

        Assert.NotNull(gameModeType);
        Assert.NotNull(startGame);
        Assert.NotNull(storageVersion);
        Assert.True((int)storageVersion!.GetRawConstantValue()! >= 2);

        var scavengerHuntMode = Enum.Parse(gameModeType!, "ScavengerHunt");
        startGame!.Invoke(service, [scavengerHuntMode]);

        var saveInvocation = Assert.Single(jsRuntime.Invocations.Where(invocation => invocation.Identifier == "localStorage.setItem"));
        var payload = Assert.IsType<string>(saveInvocation.Arguments[1]);

        Assert.Contains("\"GameMode\"", payload, StringComparison.Ordinal);
        Assert.Contains("ScavengerHunt", payload, StringComparison.Ordinal);
    }

    [Fact]
    public void BingoLogicService_generates_a_24_item_scavenger_hunt_checklist_from_existing_questions()
    {
        var method = typeof(BingoLogicService).GetMethod("GenerateScavengerHuntItems", BindingFlags.Public | BindingFlags.Static);

        Assert.NotNull(method);

        var result = method!.Invoke(null, null);

        Assert.NotNull(result);

        var items = Assert.IsAssignableFrom<IEnumerable>(result).Cast<object>().ToList();
        Assert.Equal(24, items.Count);

        var itemTexts = items.Select(GetText).ToList();

        Assert.Equal(24, itemTexts.Distinct(StringComparer.Ordinal).Count());
        Assert.DoesNotContain(Questions.FREE_SPACE, itemTexts);
        Assert.All(itemTexts, text => Assert.Contains(text, Questions.QuestionsList));
        Assert.All(items, item => Assert.False(GetCompletionState(item)));
    }

    [Fact]
    public void StartScreen_offers_bingo_and_scavenger_hunt_modes()
    {
        var content = File.ReadAllText(TestPaths.Component("StartScreen.razor"));

        Assert.Contains("OnModeSelected", content, StringComparison.Ordinal);
        Assert.Contains("Bingo", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Scavenger Hunt", content, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Home_routes_the_new_scavenger_hunt_mode()
    {
        var content = File.ReadAllText(TestPaths.Page("Home.razor"));

        Assert.Contains("OnModeSelected", content, StringComparison.Ordinal);
        Assert.Contains("ScavengerHunt", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ScavengerHuntScreen_component_exists_with_checkboxes_and_a_progress_meter()
    {
        var componentPath = TestPaths.Component("ScavengerHuntScreen.razor");

        Assert.True(File.Exists(componentPath), $"Expected component at '{componentPath}'.");

        var content = File.ReadAllText(componentPath);

        Assert.Contains("checkbox", content, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("progress", content, StringComparison.OrdinalIgnoreCase);
    }

    private static string GetText(object item)
    {
        var property = item.GetType().GetProperty("Text");

        Assert.NotNull(property);

        return Assert.IsType<string>(property!.GetValue(item));
    }

    private static bool GetCompletionState(object item)
    {
        var itemType = item.GetType();
        var property = itemType.GetProperty("IsCompleted") ?? itemType.GetProperty("IsMarked");

        Assert.NotNull(property);

        return Assert.IsType<bool>(property!.GetValue(item));
    }
}

internal sealed class RecordingJsRuntime : IJSRuntime
{
    public List<JsInvocation> Invocations { get; } = [];

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        => InvokeAsync<TValue>(identifier, CancellationToken.None, args);

    public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
    {
        var invocationArgs = args ?? [];
        Invocations.Add(new JsInvocation(identifier, invocationArgs));

        object? value = identifier switch
        {
            "localStorage.getItem" => null,
            _ => default(TValue)
        };

        return ValueTask.FromResult((TValue)value!);
    }
}

internal sealed record JsInvocation(string Identifier, IReadOnlyList<object?> Arguments);

internal static class TestPaths
{
    public static string Component(string fileName) => Path.Combine(SourceRoot, "Components", fileName);

    public static string Page(string fileName) => Path.Combine(SourceRoot, "Pages", fileName);

    private static string SourceRoot => Path.Combine(RepositoryRoot, "SocOps");

    private static string RepositoryRoot => FindRepositoryRoot();

    private static string FindRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);

        while (current is not null)
        {
            if (File.Exists(Path.Combine(current.FullName, "vscode-agent-lab-soc-ops-csharp.sln")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate the repository root from the test output directory.");
    }
}
