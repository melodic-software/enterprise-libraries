using Enterprise.Api.Middleware.Custom;
using Enterprise.Domain.AspNetCore.Events.Queuing.Middleware;

namespace Example.WebApi.Middleware;

public class MiddlewareRegistrar : IRegisterAppMiddleware
{
    public static void RegisterAppMiddleware(WebApplication app)
    {
        // TODO: We might want to conditionally add this OR the entity framework specific middleware (until they are consolidated)
        // We should be able to auto register this middleware based on a config options.
        // For example, the DomainEventQueuingConfigOptions

        // TODO: Is there a way to clear / replace middleware registrations or is this a one time permanent add?

        app.UseMiddleware<DeferredDomainEventRaisingMiddleware>();
    }
}
