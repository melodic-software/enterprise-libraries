using System.Reflection;
using Enterprise.Api.Swagger.Options;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.Extensions;

public static class SwaggerXmlCommentExtensions
{
    public static void AddXmlComments(this SwaggerGenOptions options, SwaggerOptions swaggerOptions)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        // This should be the main application.
        var entryAssembly = Assembly.GetEntryAssembly();
        AddXmlComments(entryAssembly, options);

        // We can include the comments associated with those API contract models (if applicable).
        List<Assembly> apiClientAssemblies = swaggerOptions.ApiClientAssemblies;
        apiClientAssemblies.ForEach(a => AddXmlComments(a, options));
    }

    private static void AddXmlComments(Assembly? assembly, SwaggerGenOptions options)
    {
        if (assembly == null)
        {
            return;
        }

        string baseDirectory = AppContext.BaseDirectory;

        string? assemblyName = assembly.GetName().Name;
        string xmlCommentsFilename = $"{assemblyName}.xml";
        string xmlCommentsFilePath = Path.Combine(baseDirectory, xmlCommentsFilename);

        if (File.Exists(xmlCommentsFilePath))
        {
            options.IncludeXmlComments(xmlCommentsFilePath);
        }
        else
        {
            // Throw configuration exception?
        }
    }
}
