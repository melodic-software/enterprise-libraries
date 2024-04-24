using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using static System.StringComparison;

namespace Enterprise.Api.Endpoints;

public static class EndpointExtensions
{
    private const string ControllerKey = "controller";

    public static bool HaveBothMinimalApiAndControllerEndpoint(this IReadOnlyList<Endpoint> endpoints)
    {
        return endpoints.Any(x => x.IsMinimalApiEndpoint()) && endpoints.Any(x => x.IsControllerEndpoint());
    }

    public static bool IsMinimalApiEndpoint(this Endpoint endpoint)
    {
        // Confirm it's a RouteEndpoint (common for all routed endpoints including minimal APIs).
        if (endpoint is not RouteEndpoint routeEndpoint)
            return false;

        // Check for the absence of ApiControllerAttribute (common in MVC controllers).
        bool doesNotHaveControllerAttribute = routeEndpoint.Metadata.GetMetadata<ApiControllerAttribute>() == null;

        // MVC controllers generally have 'controller' in their route pattern or defaults.
        // TODO: Add constant for this route pattern value.
        bool doesNotHaveControllerInRoutePattern = !routeEndpoint.RoutePattern.RequiredValues.ContainsKey(ControllerKey);

        // If it lacks controller attributes and does not have 'controller' in the route, it's likely a minimal API
        bool isMinimalApiEndpoint = doesNotHaveControllerAttribute && doesNotHaveControllerInRoutePattern;

        return isMinimalApiEndpoint;
    }

    public static bool IsControllerEndpoint(this Endpoint endpoint)
    {
        ApiControllerAttribute? apiControllerAttribute = endpoint.Metadata.GetMetadata<ApiControllerAttribute>();
        bool displayNameContainsController = endpoint.DisplayName != null && endpoint.DisplayName.Contains("Controller", OrdinalIgnoreCase);
        bool hasApiControllerAttribute = apiControllerAttribute != null;

        bool isControllerRoutePattern = false;

        if (endpoint is RouteEndpoint routeEndpoint)
        {
            bool containsControllerKey = routeEndpoint.RoutePattern.Defaults.ContainsKey(ControllerKey);
            bool containsRequiredControllerValue = routeEndpoint.RoutePattern.RequiredValues.ContainsKey(ControllerKey);
            isControllerRoutePattern = containsControllerKey && containsRequiredControllerValue;
        }

        bool isControllerEndpoint = hasApiControllerAttribute && isControllerRoutePattern;

        return isControllerEndpoint;
    }
}