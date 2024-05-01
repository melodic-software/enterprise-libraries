namespace Enterprise.Patterns.Outbox.Model;

public abstract class OutboxMessage
{
    /// <summary>
    /// The unique identifier for the outbox message.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The date and time (in UTC) the outbox message was created.
    /// </summary>
    public DateTime? DateCreated { get; set; }

    /// <summary>
    /// The date and time (in UTC) the outbox message was processed.
    /// </summary>
    public DateTime? DateProcessed { get; set; }

    /// <summary>
    /// An error message describing a failure that occured during an attempt to process the outbox message.
    /// </summary>
    public string? Error { get; set; }

    protected OutboxMessage(Guid id, DateTime? dateCreated)
    {
        Id = id;
        DateCreated = dateCreated;
    }
}
