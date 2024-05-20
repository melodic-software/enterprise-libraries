using Enterprise.Api.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.OperationFilters;

/// <summary>
/// A Swagger operation filter that removes version parameters from the Swagger documentation.
/// This filter supports both minimal API endpoints and traditional MVC API controllers by checking
/// the presence of versioning information in the route template and operation parameters.
/// </summary>
public class RemoveVersionParamsFilter : IOperationFilter
{
    private readonly bool _mediaTypeVersioningEnabled;
    private readonly List<string> _allVersionNames;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveVersionParamsFilter"/> class.
    /// </summary>
    /// <param name="mediaTypeVersioningEnabled">Indicates whether media type versioning is enabled.</param>
    /// <param name="allVersionNames">A list of all parameter names used for versioning.</param>
    public RemoveVersionParamsFilter(bool mediaTypeVersioningEnabled, List<string> allVersionNames)
    {
        _mediaTypeVersioningEnabled = mediaTypeVersioningEnabled;
        _allVersionNames = allVersionNames;
    }

    /// <summary>
    /// Applies the filter to a specific operation within the Swagger documentation.
    /// This method checks if the operation is associated with a route that includes versioning
    /// and removes version parameters accordingly.
    /// </summary>
    /// <param name="operation">The operation being documented.</param>
    /// <param name="context">The context providing metadata for the operation.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Check if the API uses URL versioning by examining route templates for version segments.
        bool isUrlVersioned = context.ApiDescription.ActionDescriptor.EndpointMetadata.Any(x =>
        {
            if (x is not RouteAttribute routeAttribute)
            {
                return false;
            }

            string routeTemplate = routeAttribute.Template;
            bool routeTemplateContainsVersion = routeTemplate.Contains(RoutePartials.VersionSegment, StringComparison.Ordinal);
            return routeTemplateContainsVersion;
        });

        // Identify version parameters by matching with known versioning parameter names.
        var versionParameters = operation.Parameters
            .Where(p => _allVersionNames.Any(x => p.Name.Equals(x, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        // Remove version parameters if either URL or media type versioning is enabled,
        // as these versioning strategies do not require explicit request parameters for versioning.
        if (_mediaTypeVersioningEnabled || isUrlVersioned)
        {
            foreach (OpenApiParameter versionParameter in versionParameters)
            {
                operation.Parameters.Remove(versionParameter);
            }
        }
        else if (versionParameters.Count > 1)
        {
            // When multiple versioning parameters exist but URL or media type versioning is not enabled,
            // choose to represent only one versioning mechanism in Swagger UI, preferring headers over query parameters.
            OpenApiParameter? queryStringParam = versionParameters.Find(x => x.In == ParameterLocation.Query);
            OpenApiParameter? headerParam = versionParameters.Find(x => x.In == ParameterLocation.Header);

            if (queryStringParam != null)
            {
                operation.Parameters.Remove(queryStringParam);
            }
        }
    }
}
