using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Raising.Abstract;
using Enterprise.Events.Callbacks.Raising.Abstract;

namespace Enterprise.Domain.Events.Queuing.Decoration;

public class DomainEventCallbackRaisingDecorator : DecoratorBase<IRaiseDomainEvents>, IRaiseDomainEvents
{
    private readonly IRaiseEventCallbacks _eventCallbackService;

    public DomainEventCallbackRaisingDecorator(IRaiseDomainEvents decorated,
        IGetDecoratedInstance decoratorService,
        IRaiseEventCallbacks eventCallbackService) : base(decorated, decoratorService)
    {
        _eventCallbackService = eventCallbackService;
    }

    public async Task RaiseAsync(IEnumerable<IGetDomainEvents> entities)
    {
        IReadOnlyCollection<IDomainEvent> domainEvents = entities
            .SelectMany(x => x.GetDomainEvents())
            .ToList()
            .AsReadOnly();

        await Decorated.RaiseAsync(domainEvents);

        _eventCallbackService.RaiseCallbacks(domainEvents);
    }

    public async Task RaiseAsync(IGetDomainEvents entity)
    {
        IReadOnlyCollection<IDomainEvent> domainEvents = entity
            .GetDomainEvents()
            .ToList()
            .AsReadOnly();

        await Decorated.RaiseAsync(domainEvents);

        _eventCallbackService.RaiseCallbacks(domainEvents);
    }

    public async Task RaiseAsync(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        await Decorated.RaiseAsync(domainEvents);
        _eventCallbackService.RaiseCallbacks(domainEvents);
    }

    public async Task RaiseAsync(IDomainEvent domainEvent)
    {
        await Decorated.RaiseAsync(domainEvent);
        _eventCallbackService.RaiseCallbacks(domainEvent);
    }
}
