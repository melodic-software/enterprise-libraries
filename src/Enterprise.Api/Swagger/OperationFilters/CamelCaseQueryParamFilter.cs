using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace Enterprise.Api.Swagger.OperationFilters;

public class CamelCaseQueryParamFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Parameters == null)
        {
            return;
        }

        foreach (OpenApiParameter? parameter in operation.Parameters)
        {
            if (parameter.In != ParameterLocation.Query)
            {
                continue;
            }

            string camelCaseName = JsonNamingPolicy.CamelCase.ConvertName(parameter.Name);

            parameter.Name = camelCaseName;
        }
    }
}
