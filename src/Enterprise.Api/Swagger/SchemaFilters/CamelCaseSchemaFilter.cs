using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace Enterprise.Api.Swagger.SchemaFilters;

public class CamelCaseSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        Dictionary<string, OpenApiSchema> camelCaseProperties = new Dictionary<string, OpenApiSchema>();

        foreach (KeyValuePair<string, OpenApiSchema> entry in schema.Properties)
        {
            string camelCaseName = JsonNamingPolicy.CamelCase.ConvertName(entry.Key);
            camelCaseProperties[camelCaseName] = entry.Value;
        }

        schema.Properties = camelCaseProperties;
    }
}
