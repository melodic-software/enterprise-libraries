namespace Enterprise.DomainDrivenDesign.Entities.Standard.NonGeneric;

/// <summary>
/// Represents the base class for entities in a domain-driven design context.
/// Entities are objects with a distinct identity that runs through time and different states.
/// This abstract class provides a standard way to define entities with a unique identifier (GUID).
/// </summary>
public abstract class Entity : Entity<Guid>
{
    protected Entity(Guid id) : base(id)
    {
    }
}

