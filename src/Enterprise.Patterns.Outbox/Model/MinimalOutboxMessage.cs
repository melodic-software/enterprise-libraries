namespace Enterprise.Patterns.Outbox.Model;

/// <summary>
/// A condensed representation of the outbox message used for processing the outbox message and persisting the status metadata.
/// </summary>
public class MinimalOutboxMessage
{
    public Guid Id { get; }
    public string JsonContent { get; }

    public MinimalOutboxMessage(Guid id, string jsonContent)
    {
        Id = id;
        JsonContent = jsonContent;
    }
}