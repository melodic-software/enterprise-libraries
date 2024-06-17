using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Enterprise.Api.Swagger.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using static Enterprise.Api.Swagger.Constants.SwaggerConstants;
using static Enterprise.Api.Swagger.Endpoints.SwaggerEndpointService;
using static Enterprise.Api.Swagger.UI.SwaggerUISecurityConfigurer;
using SwaggerUIOptions = Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions;

namespace Enterprise.Api.Swagger.UI;

public static class SwaggerUIConfigurer
{
    public static void Configure(WebApplication app, SwaggerSecurityOptions swaggerSecurityOptions, Options.SwaggerUIOptions swaggerUIOptions, SwaggerUIOptions options)
    {
        ConfigureSecurity(swaggerSecurityOptions, options);

        IApiVersionDescriptionProvider? descriptionProvider = app.Services.GetService<IApiVersionDescriptionProvider>();
        ConfigureSwaggerEndpoints(options, descriptionProvider);
        options.RoutePrefix = RoutePrefix;

        if (swaggerUIOptions.CustomConfigureUI != null)
        {
            // This is a complete customization.
            swaggerUIOptions.CustomConfigureUI(options);
        }
        else
        {
            ConfigureUI(options);
            swaggerUIOptions.PostConfigureUI?.Invoke(options);
        }
    }

    private static void ConfigureUI(SwaggerUIOptions options)
    {
        options.DefaultModelExpandDepth(2);
        options.DefaultModelRendering(ModelRendering.Example);
        options.DocExpansion(DocExpansion.None);
        options.EnableDeepLinking();
        options.DisplayOperationId();

        options.DisplayRequestDuration();
        options.ShowExtensions();
        options.EnableFilter();

        // Without this, authorization is needed when switching documents.
        options.EnablePersistAuthorization();

        //options.InjectStylesheet("/swagger-ui/custom.css");
        //options.InjectJavascript("https://code.jquery.com/jquery-3.6.0.min.js");
        //options.InjectJavascript("/swagger-ui/custom.js");

        // Allows for complete customization with an embedded index.html resource.
        // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/src/Swashbuckle.AspNetCore.SwaggerUI/index.html

        //CustomizeIndex(options);
    }

    private static void CustomizeIndex(SwaggerUIOptions options)
    {
        options.IndexStream = () =>
        {
            Type type = typeof(Options.SwaggerOptions);
            Assembly assembly = type.Assembly;
            string? assemblyName = assembly.GetName().Name;
            string relativeNamespace = "Swagger.EmbeddedAssets";
            string fileName = "index.html";
            string embeddedAssetName = $"{assemblyName}.{relativeNamespace}.{fileName}";
            Stream? indexStream = assembly.GetManifestResourceStream(embeddedAssetName);
            return indexStream;
        };
    }
}
