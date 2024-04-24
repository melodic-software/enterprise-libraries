using Asp.Versioning.Builder;
using Enterprise.Api.Versioning.VersionSets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Enterprise.Api.Minimal.Groups;

public static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder WithApiVersion(this RouteGroupBuilder builder, int majorVersion, int? minorVersion = 0)
    {
        // Ensure non-negative version numbers
        if (majorVersion < 0 || minorVersion < 0)
            throw new ArgumentException("Major and minor version numbers must be non-negative.");

        ApiVersionSet apiVersionSet = ApiVersionSetService.Instance.CreateVersionSet(builder, majorVersion, minorVersion);

        builder.WithApiVersionSet(apiVersionSet);

        return builder;
    }
}