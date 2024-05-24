using Asp.Versioning.ApiExplorer;
using Enterprise.Api.Swagger.Options;
using Enterprise.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Enterprise.Api.Swagger.Documents;

public static class SwaggerDocumentService
{
    public static void ConfigureSwaggerDocuments(SwaggerGenOptions options, SwaggerConfigOptions swaggerConfigOptions, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        IApiVersionDescriptionProvider? descriptionProvider = serviceProvider.GetService<IApiVersionDescriptionProvider>();

        if (descriptionProvider == null)
        {
            throw new Exception($"{nameof(descriptionProvider)} cannot be null");
        }

        string? environmentName = configuration.GetValue<string>(EnvironmentVariableConstants.AspNetCoreEnvironment);

        // register separate swagger documents per version
        foreach (ApiVersionDescription description in descriptionProvider.ApiVersionDescriptions)
        {
            string swaggerDocumentName = description.GroupName;
            string version = description.ApiVersion.ToString();
            string title = $"{swaggerConfigOptions.ApplicationName} v{description.ApiVersion}";

            if (!string.IsNullOrWhiteSpace(environmentName))
            {
                title += $" ({environmentName})";
            }

            var openApiInfo = new OpenApiInfo
            {
                Title = title,
                Description = swaggerConfigOptions.ApplicationDescription,
                Version = version,
                // TODO: Make this configurable.
                //TermsOfService = new Uri("https://example.com/terms"),
                //Contact = new OpenApiContact
                //{
                //    Name = "Example Contact",
                //    Email = "john.doe@example.com",
                //    Url = new Uri("https://example.com/contact")
                //},
                //License = new OpenApiLicense
                //{
                //    Name = "MIT License",
                //    Url = new Uri("https://opensource.org/licenses/MIT")
                //}
            };

            if (description.IsDeprecated)
            {
                // TODO: Make this configurable?
                // We should at least check the current description ending and dynamically add a sentence or parenthesized value.
                openApiInfo.Description += " This API version has been deprecated.";
            }

            options.SwaggerDoc(swaggerDocumentName, openApiInfo);
        }
    }
}
