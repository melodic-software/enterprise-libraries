using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.Domain.Events.Extensions;

public static class DomainEventRecordingExtensions
{
    public static IDomainEvent? GetFirstDomainEvent<T>(this IGetDomainEvents entity) where T : IDomainEvent
    {
        ICollection<T> typedEvents = entity.GetDomainEvents<T>();
        T? first = typedEvents.FirstOrDefault();
        return first;
    }

    public static IDomainEvent? GetSingleDomainEvent<T>(this IGetDomainEvents entity) where T : IDomainEvent
    {
        ICollection<T> typedEvents = entity.GetDomainEvents<T>();
        T? first = typedEvents.SingleOrDefault();
        return first;
    }

    public static ICollection<T> GetDomainEvents<T>(this IGetDomainEvents entity) where T : IDomainEvent
    {
        IReadOnlyList<IDomainEvent> domainEvents = entity.GetDomainEvents();
        var typedEvents = domainEvents.OfType<T>().ToList();
        return typedEvents;
    }
}
