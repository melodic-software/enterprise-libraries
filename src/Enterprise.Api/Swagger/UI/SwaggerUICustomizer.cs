﻿using Enterprise.Api.Swagger.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace Enterprise.Api.Swagger.UI;

public static class SwaggerUICustomizer
{
    public static void CustomizeUI(SwaggerUIOptions options, IConfiguration configuration)
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
            Type type = typeof(SwaggerConfigOptions);
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