using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.Documents.Filters;

public class PathLowercaseDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var dictionaryPath = swaggerDoc.Paths
            .ToDictionary(x => ToLowercase(x.Key), x => x.Value);

        var newPaths = new OpenApiPaths();

        foreach (KeyValuePair<string, OpenApiPathItem> path in dictionaryPath)
        {
            newPaths.Add(path.Key, path.Value);
        }

        swaggerDoc.Paths = newPaths;
    }

    private static string ToLowercase(string key)
    {
        string[] keySplit = key.Split('/');
        IEnumerable<string> parts = keySplit.Select(part => part.Contains('}') ? part : part.ToLowerInvariant());
        string result = string.Join('/', parts);
        return result;
    }
}
