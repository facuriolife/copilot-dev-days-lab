namespace SocOps.Models;

/// <summary>
/// Represents a single item in a scavenger hunt checklist
/// </summary>
public class ScavengerItem
{
    /// <summary>
    /// Unique identifier for the item
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The question/task text to be completed
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Whether this item has been marked as completed
    /// </summary>
    public bool IsCompleted { get; set; }
}
