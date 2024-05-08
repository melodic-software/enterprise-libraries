using Enterprise.Api.Constants;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.OperationFilters;

/// <summary>
/// A filter for removing non-applicable parameters, such as the Accept header, from Swagger documentation.
/// </summary>
public class NonApplicableParamFilter : IOperationFilter
{
    /// <summary>
    /// Applies the filter to a specific operation within the Swagger documentation.
    /// </summary>
    /// <param name="operation">The operation being documented.</param>
    /// <param name="context">The context providing metadata for the operation.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Identify and list the parameters to remove. Here, it specifically targets the Accept header.
        List<OpenApiParameter> parametersToRemove = operation.Parameters
            .Where(p =>
            {
                bool isAcceptHeader = p.In == ParameterLocation.Header &&
                                      p.Name.Equals(HttpHeaderConstants.Accept, StringComparison.OrdinalIgnoreCase);

                return isAcceptHeader;
            })
            .ToList();

        // Remove the identified parameters from the operation's documentation.
        foreach (OpenApiParameter parameter in parametersToRemove)
            operation.Parameters.Remove(parameter);
    }
}
