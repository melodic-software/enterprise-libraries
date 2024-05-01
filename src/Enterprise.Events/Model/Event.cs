namespace Enterprise.Events.Model;

public abstract class Event : IEvent
{
    /// <inheritdoc />
    public Guid Id { get; init; }

    /// <inheritdoc />
    public DateTimeOffset DateOccurred { get; init; }

    protected Event() : this(Guid.NewGuid(), DateTimeOffset.UtcNow)
    {

    }

    protected Event(DateTimeOffset dateOccurred) : this(Guid.NewGuid(), dateOccurred)
    {

    }

    protected Event(Guid id, DateTimeOffset dateOccurred)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Event ID cannot be an empty GUID!", nameof(id));

        Id = id;
        DateOccurred = dateOccurred;
    }
}
