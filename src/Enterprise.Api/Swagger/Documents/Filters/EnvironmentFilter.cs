using Enterprise.Api.Swagger.Attributes;
using Enterprise.Api.Swagger.MinimalApis;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Enterprise.Api.Swagger.Documents.Filters;

/// <summary>
/// Filters out Swagger endpoints that are not intended for use outside the development environment.
/// This includes endpoints decorated with <see cref="EnvironmentSwaggerFilterAttribute"/>
/// and minimal API endpoints associated with <see cref="EnvironmentSwaggerFilterMetadata"/>.
/// </summary>
public class EnvironmentFilter : IDocumentFilter
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<EnvironmentFilter> _filterLogger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnvironmentFilter"/> class.
    /// </summary>
    /// <param name="webHostEnvironment">The web hosting environment to check.</param>
    /// <param name="filterLogger">The logger to use for logging the filtering actions.</param>
    public EnvironmentFilter(IWebHostEnvironment webHostEnvironment, ILogger<EnvironmentFilter> filterLogger)
    {
        _webHostEnvironment = webHostEnvironment;
        _filterLogger = filterLogger;
    }

    /// <summary>
    /// Applies the filter to the specified Swagger document, removing endpoints that should not be visible
    /// in non-development environments based on custom attribute and metadata checks.
    /// </summary>
    /// <param name="swaggerDoc">The Swagger document to modify.</param>
    /// <param name="context">The document filter context providing access to additional metadata.</param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        if (_webHostEnvironment.IsDevelopment())
        {
            return;
        }

        var pathsToExclude = new HashSet<string>();

        foreach (ApiDescription apiDescription in context.ApiDescriptions)
        {
            bool isRestrictedByMetadata = apiDescription.ActionDescriptor.EndpointMetadata
                .OfType<EnvironmentSwaggerFilterMetadata>()
                .Any(metadata => metadata.Environment != _webHostEnvironment.EnvironmentName);

            bool isRestrictedByAttribute = false;

            if (apiDescription.TryGetMethodInfo(out MethodInfo methodInfo))
            {
                isRestrictedByAttribute = methodInfo.DeclaringType?.GetCustomAttributes<EnvironmentSwaggerFilterAttribute>(true)
                                       .Any(attr => attr.Environment != _webHostEnvironment.EnvironmentName) == true ||
                                   methodInfo.GetCustomAttributes<EnvironmentSwaggerFilterAttribute>(true)
                                       .Any(attr => attr.Environment != _webHostEnvironment.EnvironmentName);
            }

            bool isEndpointRestricted = isRestrictedByMetadata || isRestrictedByAttribute;

            if (!isEndpointRestricted)
            {
                continue;
            }

            string cleanPathKey = "/" + apiDescription.RelativePath?.Split('?')[0];

            if (pathsToExclude.Add(cleanPathKey))
            {
                _filterLogger.LogInformation("Removing '{Path}' from Swagger documentation as it is not intended for the current environment.", cleanPathKey);
            }
        }

        foreach (string path in pathsToExclude)
        {
            swaggerDoc.Paths.Remove(path);
        }
    }
}
