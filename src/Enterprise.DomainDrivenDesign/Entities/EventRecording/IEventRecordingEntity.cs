using Enterprise.Domain.Entities;
using Enterprise.Domain.Events;

namespace Enterprise.DomainDrivenDesign.Entities.EventRecording;

/// <summary>
/// Implementations provide access to and allow direct clearing of any recorded domain events.
/// It is expected that recording is not public behavior.
/// If public access to recording events is desired, also implement <see cref="IRecordDomainEvents"/>
/// </summary>
public interface IEventRecordingEntity : IEntity, IGetDomainEvents, IClearDomainEvents;
