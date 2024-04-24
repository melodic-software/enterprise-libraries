using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using static Enterprise.Api.Swagger.Constants.SwaggerConstants;

namespace Enterprise.Api.Swagger.Endpoints;

public static class SwaggerEndpointService
{
    public static void ConfigureSwaggerEndpoints(SwaggerUIOptions options, IApiVersionDescriptionProvider? descriptionProvider)
    {
        ArgumentNullException.ThrowIfNull(descriptionProvider);

        foreach (ApiVersionDescription description in descriptionProvider.ApiVersionDescriptions)
        {
            string url = GetEndpointUrl(description);
            string name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    }

    public static string GetEndpointUrl(ApiVersionDescription description)
    {
        string url = "/";

        if (!string.IsNullOrWhiteSpace(RoutePrefix))
            url += $"{RoutePrefix}/";

        url += $"{description.GroupName}/swagger.json";

        return url;
    }
}