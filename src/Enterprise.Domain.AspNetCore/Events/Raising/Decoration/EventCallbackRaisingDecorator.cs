using Enterprise.DesignPatterns.Decorator.Model;
using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Domain.Events;
using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Raising;
using Enterprise.Events.Raising.Callbacks.Abstractions;
using Microsoft.Extensions.Logging;

namespace Enterprise.Domain.AspNetCore.Events.Raising.Decoration;

public class EventCallbackRaisingDecorator : DecoratorBase<IRaiseDomainEvents>, IRaiseDomainEvents
{
    private readonly IRaiseEventCallbacks _eventCallbackService;
    private readonly ILogger<EventCallbackRaisingDecorator> _logger;

    public EventCallbackRaisingDecorator(IRaiseDomainEvents decorated,
        IGetDecoratedInstance decoratorService,
        IRaiseEventCallbacks eventCallbackService,
        ILogger<EventCallbackRaisingDecorator> logger) : base(decorated, decoratorService)
    {
        _eventCallbackService = eventCallbackService;
        _logger = logger;
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
