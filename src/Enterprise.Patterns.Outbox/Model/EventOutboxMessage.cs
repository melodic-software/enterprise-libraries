namespace Enterprise.Patterns.Outbox.Model;

public class EventOutboxMessage : OutboxMessage
{
    /// <summary>
    /// The type name of the event.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// The serialized JSON representation of the event data.
    /// </summary>
    public string JsonContent { get; set; }
    
    public EventOutboxMessage(Guid id, string type, string jsonContent, DateTime? dateCreated) : base(id, dateCreated)
    {
        Type = type;
        JsonContent = jsonContent;
    }
}
