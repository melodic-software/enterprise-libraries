using Asp.Versioning;
using Enterprise.Api.Versioning.Options;
using Enterprise.Options.Core.Singleton;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using static Enterprise.Api.Versioning.Constants.VersioningConstants;

namespace Enterprise.Api.Versioning;

public static class ApiVersioningConfigService
{
    public static void ConfigureApiVersioning(this IServiceCollection services, IConfiguration configuration)
    {
        VersioningConfigOptions versioningConfigOptions = OptionsInstanceService.Instance
            .GetOptionsInstance<VersioningConfigOptions>(configuration, VersioningConfigOptions.ConfigSectionKey);

        IApiVersioningBuilder apiVersioningBuilder = services.AddApiVersioning(options =>
        {
            // When false, a 400 response is returned when a version is not specified in the request.
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = versioningConfigOptions.DefaultApiVersion;
            options.ReportApiVersions = true; // Adds the "api-supported-versions" response header.

            options.ApiVersionReader = BuildVersionReader(versioningConfigOptions);
        });

        apiVersioningBuilder.AddApiExplorer(options =>
        {
            options.GroupNameFormat = VersionGroupNameFormat;

            // Only enable this if using the URL version reader.
            options.SubstituteApiVersionInUrl = versioningConfigOptions.EnableUrlVersioning;
        });
    }

    private static IApiVersionReader BuildVersionReader(VersioningConfigOptions configOptions)
    {
        List<IApiVersionReader> apiVersionReaders = [];

        if (configOptions.EnableUrlVersioning)
        {
            apiVersionReaders.Add(new UrlSegmentApiVersionReader());
        }

        if (configOptions.EnableQueryStringVersioning)
        {
            apiVersionReaders.Add(new QueryStringApiVersionReader(VersionQueryStringParameterName));
        }

        if (configOptions.EnableHeaderVersioning)
        {
            apiVersionReaders.Add(new HeaderApiVersionReader(CustomVersionRequestHeader));
        }

        if (configOptions.EnableMediaTypeVersioning)
        {
            apiVersionReaders.Add(new MediaTypeApiVersionReader(MediaTypeVersionParameterName));
        }

        return ApiVersionReader.Combine(apiVersionReaders.ToArray());
    }
}
