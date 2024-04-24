using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Enterprise.Api.Versioning.VersionSets;

public class ApiVersionSetService
{
    private readonly Dictionary<ApiVersion, ApiVersionSet> _dictionary = new();
    private static readonly Lazy<ApiVersionSetService> Lazy = new(() => new ApiVersionSetService());

    public static ApiVersionSetService Instance => Lazy.Value;

    public ApiVersionSet CreateVersionSet(IEndpointRouteBuilder builder, int majorVersion, int? minorVersion = 0)
    {
        ApiVersion apiVersion = new ApiVersion(majorVersion, minorVersion);
        ApiVersionSet result = CreateVersionSet(builder, apiVersion);
        return result;
    }

    public ApiVersionSet CreateVersionSet(IEndpointRouteBuilder builder, ApiVersion apiVersion)
    {
        ApiVersionSet? apiVersionSet = _dictionary
            .FirstOrDefault(x =>
                x.Key.MajorVersion == apiVersion.MajorVersion &&
                x.Key.MinorVersion == apiVersion.MinorVersion)
            .Value;

        if (apiVersionSet != null)
            return apiVersionSet;

        ApiVersionSetBuilder apiVersionSetBuilder = builder.NewApiVersionSet()
            .HasApiVersion(apiVersion)
            .ReportApiVersions();

        apiVersionSet = apiVersionSetBuilder.Build();

        _dictionary.Add(apiVersion, apiVersionSet);

        return apiVersionSet;
    }
}