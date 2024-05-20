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
            Uri url = GetEndpointUrl(description);
            string name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url.ToString(), name);
        }
    }

    public static Uri GetEndpointUrl(ApiVersionDescription description)
    {
        string endpointUrl = "/";

        if (!string.IsNullOrWhiteSpace(RoutePrefix))
        {
            endpointUrl += $"{RoutePrefix}/";
        }

        endpointUrl += $"{description.GroupName}/swagger.json";

        return new Uri(endpointUrl);
    }
}
