using System.Reflection;
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
        Type eventType = @event.GetType();
        Func<IEvent, Task<IEnumerable<IHandleEvent>>> resolver = CreateResolver(eventType);
        Task<IEnumerable<IHandleEvent>> task = resolver(@event);
        return task;
    }

    private Func<IEvent, Task<IEnumerable<IHandleEvent>>> CreateResolver(Type eventType)
    {
        Type baseType = typeof(EventHandlerResolverBase);

        // Retrieve all generic methods named 'ResolveAsync'.
        MethodInfo[] methods = baseType.GetMethods()
            .Where(m => m is { Name: nameof(ResolveAsync), IsGenericMethod: true })
            .ToArray();

        // Since there should only be one generic method named 'ResolveAsync', we can directly select it.
        MethodInfo genericMethodDefinition = methods.FirstOrDefault()
                                             ?? throw new InvalidOperationException($"Generic {nameof(base.ResolveAsync)} method not found.");

        // Create a closed generic method for the specific event type.
        MethodInfo closedGenericMethod = genericMethodDefinition.MakeGenericMethod(eventType);

        return async e =>
        {
            // Invoke the closed generic method.
            object result = closedGenericMethod.Invoke(this, [e])
                            ?? throw new InvalidOperationException("Method invocation returned null.");

            // Define the task type.
            Type taskType = typeof(Task<>)
                .MakeGenericType(typeof(IEnumerable<>)
                    .MakeGenericType(typeof(IHandleEvent<>)
                        .MakeGenericType(eventType)));

            // Convert/change the type to the one we defined.
            Task? task = Convert.ChangeType(result, taskType) as Task;

            // Await the task and return its result.
            await (task ?? throw new InvalidOperationException("Task conversion failed.")).ConfigureAwait(false);

            // Extract the result from the Task and return it.
            PropertyInfo resultProperty = task.GetType().GetProperty(nameof(Task<object>.Result)) ??
                                          throw new InvalidOperationException("");

            IEnumerable<IHandleEvent> eventHandlers = (IEnumerable<IHandleEvent>)(resultProperty.GetValue(task) ??
                                                                                  new InvalidOperationException("Task result is invalid."));

            return eventHandlers;
        };
    }
}
