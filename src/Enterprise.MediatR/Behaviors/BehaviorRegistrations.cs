using Enterprise.MediatR.Behaviors.Caching;
using Enterprise.MediatR.Behaviors.Logging;
using Enterprise.MediatR.Behaviors.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.MediatR.Behaviors;

public static class BehaviorRegistrations
{
    /// <summary>
    /// These are the default behavior registrations.
    /// </summary>
    public static IReadOnlyCollection<BehaviorRegistration> Default() =>
    [
        new(typeof(RequestLoggingBehavior<,>), ServiceLifetime.Scoped),
        new (typeof(UseCaseLoggingBehavior<,>)),
        new (typeof(CommandFluentValidationBehavior<,>)),
        new (typeof(QueryCachingBehavior<,>))
    ];
}
