using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.Documents.Filters;

public class AlphabeticSortingFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = swaggerDoc.Paths
            .OrderBy(e => e.Key)
            .ToList();

        var openApiPaths = new OpenApiPaths();
        paths.ForEach(x => openApiPaths.Add(x.Key, x.Value));

        swaggerDoc.Paths = openApiPaths;
    }
}
