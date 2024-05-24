using Enterprise.DesignPatterns.Decorator.Services.Abstract;
using Enterprise.Events.Model;
using Enterprise.Events.Raising.Abstract;
using Enterprise.Events.Raising.Decorators.Abstract;
using Microsoft.Extensions.Logging;

namespace Enterprise.Events.Raising.Decorators;

public class DuplicatePreventionDecorator : DuplicatePreventionDecoratorBase<IRaiseEvents>, IRaiseEvents
{
    public DuplicatePreventionDecorator(IRaiseEvents decorated,
        IGetDecoratedInstance decoratorService,
        IRecordRaisedEvents raisedEventRecorder,
        ILogger<DuplicatePreventionDecorator> logger) : base(decorated, decoratorService, raisedEventRecorder, logger)
    {
    }

    public async Task RaiseAsync(IReadOnlyCollection<IEvent> events)
    {
        await RaiseAsync(events, e => Decorated.RaiseAsync(events));
    }

    public async Task RaiseAsync(IEvent @event)
    {
        await RaiseAsync(@event, e => Decorated.RaiseAsync(@event));
    }
}
