using Enterprise.Api.Swagger.Constants;

namespace Enterprise.Api.Middleware.RootRedirect;

public class RootRedirectMiddlewareOptions
{
    public string? SwaggerRoutePrefix => SwaggerConstants.RoutePrefix;
}
