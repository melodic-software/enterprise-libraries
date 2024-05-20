using Enterprise.Api.Swagger.MinimalApis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enterprise.Api.Minimal.EndpointRouteBuilding;

/// <summary>
/// Provides extension methods for adding endpoints to an <see cref="IEndpointRouteBuilder"/> with environment-specific visibility.
/// </summary>
public static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Maps a minimal API endpoint that is only available in the development environment.
    /// </summary>
    /// <param name="endpoints">The route builder to add the endpoint to.</param>
    /// <param name="pattern">The URL pattern for the route.</param>
    /// <param name="handler">The route handler delegate.</param>
    /// <returns>An endpoint convention builder that can be used to further configure the endpoint, if the environment condition is met.</returns>
    public static IEndpointConventionBuilder MapDevOnlyEndpoint(this IEndpointRouteBuilder endpoints, string pattern, RequestDelegate handler)
    {
        return endpoints.MapEnvironmentSpecificEndpoint(pattern, handler, [Environments.Development])
            .WithMetadata();
    }

    /// <summary>
    /// Maps a minimal API endpoint that is available only in specified environments.
    /// This method provides flexibility to restrict endpoint availability to any set of environments defined by the allowedEnvironments parameter.
    /// Additional configuration can be applied to the route handler via the optional configure parameter.
    /// </summary>
    /// <param name="endpoints">The route builder to add the endpoint to.</param>
    /// <param name="pattern">The URL pattern for the route.</param>
    /// <param name="handler">The route handler delegate.</param>
    /// <param name="allowedEnvironments">An array of environment names where the endpoint should be available.</param>
    /// <returns>An endpoint convention builder that can be used to further configure the endpoint, if the environment condition is met.</returns>
    public static IEndpointConventionBuilder MapEnvironmentSpecificEndpoint(this IEndpointRouteBuilder endpoints,
        string pattern, RequestDelegate handler, string[]? allowedEnvironments)
    {
        allowedEnvironments ??= [Environments.Development];

        IWebHostEnvironment environment = endpoints.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        if (allowedEnvironments.Contains(environment.EnvironmentName))
        {
            return endpoints.Map(pattern, handler);
        }

        // Register a handler that simply returns a 404 status code when the environment doesn't match.
        // We add metadata, so it doesn't show up in the generated Swagger documentation unless the environment matches.
        IEndpointConventionBuilder endpointConventionBuilder = endpoints
            .Map(pattern, () => TypedResults.NotFound())
            .WithMetadata(new EnvironmentSwaggerFilterMetadata(environment.EnvironmentName));
            
        return endpointConventionBuilder;

        // Create the endpoint using the provided handler.
    }
}
