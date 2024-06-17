using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Enterprise.Api.Minimal.Mapping;

public static class EndpointMappingExtensions
{
    /// <summary>
    /// Adds transient registrations of all <see cref="IMapEndpoint"/> instances found in the provided assemblies with the DI container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, List<Assembly> assemblies)
    {
        return services.AddEndpoints(assemblies.ToArray());
    }

    /// <summary>
    /// Adds transient registrations of all <see cref="IMapEndpoint"/> instances found in the provided assemblies with the DI container.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (!assemblies.Any())
        {
            return services;
        }

        ServiceDescriptor[] serviceDescriptors = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IMapEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IMapEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);

        return services;
    }

    /// <summary>
    /// Retrieves all registered instances of <see cref="IMapEndpoint"/> and invokes the endpoint mapping behavior for each one that is registered.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="routeGroupBuilder"></param>
    /// <returns></returns>
    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IMapEndpoint> endpointMappers = app.Services.GetRequiredService<IEnumerable<IMapEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IMapEndpoint endpointMapper in endpointMappers)
        {
            endpointMapper.MapEndpoint(builder);
        }

        return app;
    }
}
