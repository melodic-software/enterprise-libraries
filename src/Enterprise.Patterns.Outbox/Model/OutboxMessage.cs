namespace Enterprise.Patterns.Outbox.Model;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string JsonContent { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateProcessed { get; set; }
    public string? Error { get; set; }

    public OutboxMessage(Guid id, string type, string jsonContent, DateTime? dateCreated)
    {
        Id = id;
        DateCreated = dateCreated;
        Type = type;
        JsonContent = jsonContent;
    }
}