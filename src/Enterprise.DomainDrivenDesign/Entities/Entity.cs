using Enterprise.Domain.Entities;

namespace Enterprise.DomainDrivenDesign.Entities;

// TODO: What about natural keys?
// Would we just provide another property and have the ID just return another named property of the same type?

// It is recommended to keep entity properties entirely private or internal so external layers cannot directly couple to the entity state.
// With this approach, it is common to implement the memento pattern for state hydration and external access to entity state without exposing its direct internal structure.

// With properties and constructors, consider refactoring primitive types to value objects (particularly if behavior can be moved to this class).
// This can be done with the ID as well instead of using a GUID or integer directly.

public abstract class Entity<TId> : IEntity, IEquatable<Entity<TId>> where TId : IEquatable<TId>
{
    /// <summary>
    /// Gets the identifier of the entity.
    /// Aggregate root entity IDs must be unique within the system.
    /// The IDs of child entities under aggregate roots must be unique within the parent aggregate.
    /// </summary>
    public TId Id { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class with the specified identifier.
    /// Throws an exception if the given ID is the default value for its type,
    /// enforcing the requirement for entities to have a meaningful identity.
    /// </summary>
    /// <param name="id">The unique identifier for the entity.</param>
    /// <exception cref="ArgumentException">Thrown if the given ID is the default value for its type.</exception>
    protected Entity(TId id)
    {
        if (Equals(id, default(TId)))
        {
            throw new ArgumentException("The ID cannot be the default value.", nameof(id));
        }

        Id = id;
    }

    /// <summary>
    /// Compares this entity with another entity for equality based on their identifiers.
    /// Equality is determined by the identity (ID), not by the attributes of the entity.
    /// </summary>
    /// <param name="other">The other entity to compare with.</param>
    /// <returns>True if the entities are equal; otherwise false.</returns>
    public bool Equals(Entity<TId>? other)
    {
        if (other == null)
        {
            return false;
        }

        bool identifiersAreEqual = Id.Equals(other.Id);

        return identifiersAreEqual;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// Implements object equality, ensuring that the type and ID match.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        bool isTypeMismatch = obj.GetType() != GetType();

        if (isTypeMismatch)
        {
            return false;
        }

        bool areEqual = Equals(obj as Entity<TId>);

        return areEqual;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// Hash code is based on the entity's ID, supporting usage in collections.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => Id.GetHashCode();

    /// <summary>
    /// Returns a string that represents the current object.
    /// Useful for debugging and logging, showing the type of the entity.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return GetType().Name;
    }
}

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
