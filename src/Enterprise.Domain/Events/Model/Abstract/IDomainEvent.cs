using Enterprise.Events.Model;

namespace Enterprise.Domain.Events.Model.Abstract;

/// <summary>
/// Represents an event that happened within the system that is of importance to the business.
/// </summary>
public interface IDomainEvent : IEvent;