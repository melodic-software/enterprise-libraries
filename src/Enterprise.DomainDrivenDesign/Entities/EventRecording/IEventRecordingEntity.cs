using Enterprise.Domain.Events;

namespace Enterprise.DomainDrivenDesign.Entities.EventRecording;

/// <summary>
/// Implementations provide access to and allow direct clearing of any recorded domain events.
/// </summary>
public interface IEventRecordingEntity : IGetDomainEvents, IClearDomainEvents;
