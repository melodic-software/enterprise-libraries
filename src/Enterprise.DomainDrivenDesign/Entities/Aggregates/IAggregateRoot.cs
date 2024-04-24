using Enterprise.Domain.Entities;
using Enterprise.Domain.Events;

namespace Enterprise.DomainDrivenDesign.Entities.Aggregates;

/// <summary>
/// Designates an entity as the root of an aggregate.
/// Aggregate roots encapsulate the aggregate's boundaries and ensure all changes within the aggregate are consistent.
/// They are responsible for enforcing business rules and invariants across the entire aggregate.
/// They also allow access to domain events that have occurred within the aggregate boundary.
/// </summary>
public interface IAggregateRoot : IEntity, IGetDomainEvents, IClearDomainEvents;