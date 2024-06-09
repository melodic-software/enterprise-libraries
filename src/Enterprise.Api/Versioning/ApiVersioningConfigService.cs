using Asp.Versioning;
using Enterprise.Api.Versioning.Options;
using Enterprise.Options.Core.Services.Singleton;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using static Enterprise.Api.Versioning.Constants.VersioningConstants;

namespace Enterprise.Api.Versioning;

public static class ApiVersioningConfigService
{
    public static void ConfigureApiVersioning(this IServiceCollection services, IConfiguration configuration)
    {
        VersioningOptions options = OptionsInstanceService.Instance
            .GetOptionsInstance<VersioningOptions>(configuration, VersioningOptions.ConfigSectionKey);

        IApiVersioningBuilder apiVersioningBuilder = services.AddApiVersioning(o =>
        {
            // When false, a 400 response is returned when a version is not specified in the request.
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = options.DefaultApiVersion;
            o.ReportApiVersions = true; // Adds the "api-supported-versions" response header.

            o.ApiVersionReader = BuildVersionReader(options);
        });

        apiVersioningBuilder.AddApiExplorer(o =>
        {
            o.GroupNameFormat = VersionGroupNameFormat;

            // Only enable this if using the URL version reader.
            o.SubstituteApiVersionInUrl = options.EnableUrlVersioning;
        });
    }

    private static IApiVersionReader BuildVersionReader(VersioningOptions options)
    {
        List<IApiVersionReader> apiVersionReaders = [];

        if (options.EnableUrlVersioning)
        {
            apiVersionReaders.Add(new UrlSegmentApiVersionReader());
        }

        if (options.EnableQueryStringVersioning)
        {
            apiVersionReaders.Add(new QueryStringApiVersionReader(VersionQueryStringParameterName));
        }

        if (options.EnableHeaderVersioning)
        {
            apiVersionReaders.Add(new HeaderApiVersionReader(CustomVersionRequestHeader));
        }

        if (options.EnableMediaTypeVersioning)
        {
            apiVersionReaders.Add(new MediaTypeApiVersionReader(MediaTypeVersionParameterName));
        }

        return ApiVersionReader.Combine(apiVersionReaders.ToArray());
    }
}
