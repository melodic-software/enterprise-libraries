namespace Enterprise.DomainDrivenDesign.Entities.EventRecording.NonGeneric;

public abstract class Entity : Entity<Guid>
{
    protected Entity(Guid id) : base(id)
    {
    }
}
