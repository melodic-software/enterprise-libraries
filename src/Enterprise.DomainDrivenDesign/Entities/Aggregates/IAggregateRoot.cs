using Enterprise.Domain.Entities;
using Enterprise.Domain.Events;

namespace Enterprise.DomainDrivenDesign.Entities.Aggregates;

/// <summary>
/// Designates an entity as the root of an aggregate.
/// Aggregate roots encapsulate the aggregate's boundaries and ensure all changes within the aggregate are atomic and consistent.
/// They are responsible for enforcing business rules and invariants across the entire aggregate.
/// They also allow access to domain events that have occurred within the aggregate boundary.
/// Aggregate root identifiers should be globally unique.
/// Identifiers of entities inside the aggregate only need to be unique within the aggregate.
/// Aggregates can contain other aggregate references, but they can only be identifier references, and not aggregate objects.
/// Aggregate entities should not be referenced from outside the aggregate root. Those external references should point to the aggregate root and not the entity type.
/// </summary>
public interface IAggregateRoot : IEntity, IGetDomainEvents, IClearDomainEvents;
