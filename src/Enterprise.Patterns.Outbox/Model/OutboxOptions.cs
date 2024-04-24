namespace Enterprise.Patterns.Outbox.Model;

public class OutboxOptions
{
    /// <summary>
    /// Determines how often the background job should run.
    /// </summary>
    public int IntervalInSeconds { get; init; } = 10;

    /// <summary>
    /// Determines many items will be loaded and processed in a single run.
    /// </summary>
    public int BatchSize { get; init; } = 10;
}