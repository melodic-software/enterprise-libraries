using Enterprise.MediatR.Behaviors.Caching;
using Enterprise.MediatR.Behaviors.ErrorHandling;
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
        // These apply to all requests.
        new(typeof(RequestLoggingBehavior<,>), ServiceLifetime.Scoped),
        new(typeof(RequestErrorHandlingBehavior<,>)),
        new (typeof(NullRequestValidationBehavior<,>)),
        new (typeof(RequestFluentValidationBehavior<,>)),

        // These apply to specific requests.
        new (typeof(QueryCachingBehavior<,>))
    ];
}
