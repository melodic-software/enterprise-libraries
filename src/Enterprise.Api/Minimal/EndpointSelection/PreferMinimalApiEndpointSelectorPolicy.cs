using Asp.Versioning;
using Enterprise.Api.Endpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Enterprise.Api.Minimal.EndpointSelection;

public class PreferMinimalApiEndpointSelectorPolicy : MatcherPolicy, IEndpointSelectorPolicy
{
    private readonly ILogger<PreferMinimalApiEndpointSelectorPolicy> _logger;
    private readonly ApiVersioningOptions? _versioningOptions;

    public override int Order => 1;

    public PreferMinimalApiEndpointSelectorPolicy(ILogger<PreferMinimalApiEndpointSelectorPolicy> logger, IOptions<ApiVersioningOptions>? versioningOptions)
    {
        _logger = logger;
        _versioningOptions = versioningOptions?.Value;
    }

    public bool AppliesToEndpoints(IReadOnlyList<Endpoint> endpoints)
    {
        // Check if there are multiple endpoints, indicating a potential ambiguity
        // This policy applies only if there's at least one minimal API endpoint and one controller endpoint.
        return endpoints.HaveBothMinimalApiAndControllerEndpoint();
    }

    public Task ApplyAsync(HttpContext httpContext, CandidateSet candidates)
    {
        ApiVersion? requestedApiVersion = GetRequestedApiVersion(httpContext);

        for (int i = 0; i < candidates.Count; i++)
        {
            // We put this here because they technically can be invalidated in any order.
            if (!candidates.IsValidCandidate(i))
            {
                continue;
            }

            Endpoint endpoint = candidates[i].Endpoint;

            if (VersionIsInvalid(candidates, endpoint, requestedApiVersion, i))
            {
                continue;
            }

            if (!endpoint.IsMinimalApiEndpoint())
            {
                continue;
            }

            // Prefer a minimal API endpoint (if the versions match).
            _logger.LogInformation("Preferred minimal API endpoint: {EndpointName}.", endpoint.DisplayName);
            MarkAllOtherCandidatesAsInvalid(candidates, i);
            break;
        }

        return Task.CompletedTask;
    }

    private ApiVersion? GetRequestedApiVersion(HttpContext httpContext)
    {
        if (_versioningOptions == null)
        {
            return null;
        }

        ApiVersion? requestedApiVersion = httpContext.GetRequestedApiVersion();

        if (requestedApiVersion == null && _versioningOptions.AssumeDefaultVersionWhenUnspecified)
        {
            ApiVersion defaultApiVersion = _versioningOptions.DefaultApiVersion;
            _logger.LogInformation("No API version specified. Using default: {RequestedApiVersion}.", defaultApiVersion);
            requestedApiVersion = defaultApiVersion;
        }
        else if (requestedApiVersion != null)
        {
            _logger.LogInformation("Requested API version: {RequestedApiVersion}.", requestedApiVersion);
        }
        else
        {
            _logger.LogInformation("No API version specified.");
        }

        return requestedApiVersion;
    }

    private bool VersionIsInvalid(CandidateSet candidateSet, Endpoint endpoint, ApiVersion? requestedApiVersion, int i)
    {
        if (_versioningOptions == null)
        {
            return false;
        }

        // First, attempt to use the built-in mechanism to determine if the endpoint is valid for the requested version.
        ApiVersionMetadata? apiVersionMetadata = endpoint.Metadata.GetMetadata<ApiVersionMetadata>();

        if (apiVersionMetadata != null && apiVersionMetadata.IsMappedTo(requestedApiVersion))
        {
            // If the endpoint is explicitly mapped to the requested version, it's valid.
            return false;
        }

        // Fallback to checking against declared versions if ApiVersionMetadata is not conclusive.
        ApiVersionAttribute? apiVersionAttribute = endpoint.Metadata.GetMetadata<ApiVersionAttribute>();
        IReadOnlyList<ApiVersion> declaredApiVersions = apiVersionAttribute?.Versions ?? [];

        // Assume the default version if no versions are declared and the configuration specifies to do so.
        if (!declaredApiVersions.Any() && _versioningOptions.AssumeDefaultVersionWhenUnspecified)
        {
            declaredApiVersions = [_versioningOptions.DefaultApiVersion];
        }

        // No need to continue if no version is requested and no versions are declared.
        if (requestedApiVersion == null || !declaredApiVersions.Any())
        {
            return false;
        }

        // Check if any of the declared versions match the requested version.
        if (declaredApiVersions.Any(v => v == requestedApiVersion))
        {
            return false;
        }

        // If we reach here, it means there's a version mismatch.
        _logger.LogInformation("Version mismatch. Excluding endpoint: {EndpointName}.", endpoint.DisplayName);
        candidateSet.SetValidity(i, false);
        return true;
    }

    private void MarkAllOtherCandidatesAsInvalid(CandidateSet candidateSet, int validCandidateIndex)
    {
        CandidateState validCandidate = candidateSet[validCandidateIndex];
        _logger.LogInformation("Valid candidate selected: {DisplayName}. Invalidating other candidates.", validCandidate.Endpoint.DisplayName);

        for (int j = 0; j < candidateSet.Count; j++)
        {
            if (j == validCandidateIndex)
            {
                continue;
            }

            CandidateState invalidCandidate = candidateSet[j];
            candidateSet.SetValidity(j, false);
            _logger.LogInformation("Invalidated non-minimal API endpoint: {EndpointName}.", invalidCandidate.Endpoint.DisplayName);
        }
    }
}
