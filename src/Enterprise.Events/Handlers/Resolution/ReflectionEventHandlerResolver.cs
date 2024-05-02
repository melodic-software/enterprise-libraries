using Enterprise.Events.Handlers.Abstract;
using Enterprise.Events.Handlers.Resolution.Abstract;
using Enterprise.Events.Model;

namespace Enterprise.Events.Handlers.Resolution;

public class ReflectionEventHandlerResolver : EventHandlerResolverBase
{
    public ReflectionEventHandlerResolver(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    /// <inheritdoc />
    public override Task<IEnumerable<IHandleEvent>> ResolveAsync(IEvent @event)
    {
        var eventType = @event.GetType();
        var resolver = CreateResolver(eventType);
        var task = resolver(@event);
        return task;
    }

    private Func<IEvent, Task<IEnumerable<IHandleEvent>>> CreateResolver(Type eventType)
    {
        var baseType = typeof(EventHandlerResolverBase);

        // Retrieve all generic methods named 'ResolveAsync'
        var methods = baseType.GetMethods()
            .Where(m => m is { Name: nameof(ResolveAsync), IsGenericMethod: true })
            .ToArray();

        // Since there should only be one generic method named 'ResolveAsync', we can directly select it.
        var genericMethodDefinition = methods.FirstOrDefault()
                                             ?? throw new InvalidOperationException($"Generic {nameof(base.ResolveAsync)} method not found.");

        // Create a closed generic method for the specific event type.
        var closedGenericMethod = genericMethodDefinition.MakeGenericMethod(eventType);

        return async e =>
        {
            // Invoke the closed generic method
            var result = closedGenericMethod.Invoke(this, [e])
                            ?? throw new InvalidOperationException("Method invocation returned null.");

            // Define the task type.
            var taskType = typeof(Task<>)
                .MakeGenericType(typeof(IEnumerable<>)
                    .MakeGenericType(typeof(IHandleEvent<>)
                        .MakeGenericType(eventType)));

            // Convert/change the type to the one we defined.
            var task = Convert.ChangeType(result, taskType) as Task;

            // Await the task and return its result.
            await (task ?? throw new InvalidOperationException("Task conversion failed.")).ConfigureAwait(false);

            // Extract the result from the Task and return it
            var resultProperty = task.GetType().GetProperty(nameof(Task<object>.Result)) ??
                                          throw new InvalidOperationException("");

            var eventHandlers = (IEnumerable<IHandleEvent>)(resultProperty.GetValue(task) ??
                                                                                  new InvalidOperationException("Task result is invalid."));

            return eventHandlers;
        };
    }
}
