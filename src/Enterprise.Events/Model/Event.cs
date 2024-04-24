namespace Enterprise.Events.Model;

public abstract class Event : IEvent
{
    /// <inheritdoc />
    public Guid Id { get; protected set; }

    /// <inheritdoc />
    public DateTimeOffset DateOccurred { get; protected set; }

    protected Event(Guid id, DateTimeOffset dateOccurred)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Event ID cannot be an empty GUID!", nameof(id));

        Id = id;
        DateOccurred = dateOccurred;
    }

    protected Event(DateTimeOffset dateOccurred) : this(Guid.NewGuid(), dateOccurred)
    {
    }

    protected Event() : this(Guid.NewGuid(), DateTimeOffset.UtcNow)
    {
    }
}