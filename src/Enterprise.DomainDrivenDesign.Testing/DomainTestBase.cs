using Enterprise.Domain.Events;
using Enterprise.Domain.Events.Extensions;
using Enterprise.Domain.Events.Model.Abstract;

namespace Enterprise.DomainDrivenDesign.Testing;

public abstract class DomainTestBase
{
    public static T AssertDomainEventWasRecorded<T>(IGetDomainEvents entity) where T : IDomainEvent
    {
        IDomainEvent? domainEvent = entity.GetSingleDomainEvent<T>();

        if (domainEvent == null)
            throw new Exception($"{typeof(T).Name} was not recorded.");

        return (T)domainEvent;
    }
}