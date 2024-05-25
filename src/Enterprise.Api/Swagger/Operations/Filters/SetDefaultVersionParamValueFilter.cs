using System.Text.RegularExpressions;
using Asp.Versioning;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.Operations.Filters;

/// <summary>
/// A Swagger operation filter that sets default values for version parameters in the Swagger documentation.
/// This filter is designed to work with both minimal API endpoints and traditional MVC API controllers.
/// </summary>
public class SetDefaultVersionParamValueFilter : IOperationFilter
{
    private readonly List<string> _allVersionNames;

    /// <summary>
    /// Initializes a new instance of the <see cref="SetDefaultVersionParamValueFilter"/> class.
    /// </summary>
    /// <param name="allVersionNames">A list of all parameter names used for versioning.</param>
    public SetDefaultVersionParamValueFilter(List<string> allVersionNames)
    {
        _allVersionNames = allVersionNames;
    }

    /// <summary>
    /// Regular expression pattern to match document version identifiers (e.g., "v1", "v2").
    /// </summary>
    private const string DocumentVersionRegexPattern = @"^v\d+$";

    /// <summary>
    /// Applies the filter to a specific operation within the Swagger documentation.
    /// Sets the default API version based on the version metadata associated with the operation.
    /// </summary>
    /// <param name="operation">The operation being documented.</param>
    /// <param name="context">The context providing metadata for the operation.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Attempt to extract the API version from the operation's metadata.
        ApiVersion? apiVersion = context.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<ApiVersionAttribute>()
            .SelectMany(attr => attr.Versions)
            .FirstOrDefault();

        apiVersion ??= context.ApiDescription.Properties
            .FirstOrDefault(x => x.Value is ApiVersion).Value as ApiVersion;

        // NOTE: For minimal APIs, this requires a route group with an explicit version attribute added to the metadata collection.
        // EXAMPLE: routeGroupBuilder.WithMetadata(new ApiVersionAttribute(versionString))

        if (apiVersion == null)
        {
            return;
        }

        // Identify version parameters by matching with known versioning parameter names.
        var versionParameters = operation.Parameters
            .Where(p => _allVersionNames.Any(x => p.Name.Equals(x, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        foreach (OpenApiParameter versionParameter in versionParameters)
        {
            // Set the example value for the version parameter to the discovered API version.
            versionParameter.Example = new OpenApiString(apiVersion.ToString());
            versionParameter.AllowEmptyValue = false;

            // If the operation's document name matches the version pattern, mark the parameter as read-only.
            // This indicates that the API version is fixed for this document and should not be changed by the user.
            if (Regex.IsMatch(context.DocumentName, DocumentVersionRegexPattern))
            {
                // This prevents the parameter from appearing as editable in the UI.
                versionParameter.Schema.ReadOnly = true;
            }
        }
    }
}
