using Enterprise.Multitenancy.Abstractions;
using Enterprise.Multitenancy.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using static Enterprise.Multitenancy.AspNetCore.Constants.TenantConstants;

namespace Enterprise.Multitenancy.AspNetCore.Services;

/// <summary>
/// This implementation allows for clients to impersonate any tenant if they know the tenant ID.
/// In real applications, the tenant ID should be set based on secure authentication data (claims).
/// TODO: Implement a version of this that uses JWT, API lookup (cached), or claims data.
/// </summary>
public class TenantRequestHeaderService : IGetTenantId
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TenantRequestHeaderService> _logger;
    private readonly bool _tenantIdRequired;
    private string? _tenantId;

    public TenantRequestHeaderService(
        IHttpContextAccessor httpContextAccessor,
        ILogger<TenantRequestHeaderService> logger,
        bool tenantIdRequired = false
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _tenantIdRequired = tenantIdRequired;
    }

    public string? GetTenantId()
    {
        _logger.LogDebug("Getting tenant ID.");

        if (_tenantId is not null)
        {
            LogReturn();
            return _tenantId;
        }

        _logger.LogDebug("Retrieving value from request header.");

        if (_httpContextAccessor.HttpContext?.Request.Headers.TryGetValue(TenantRequestHeader, out StringValues value) == true)
        {
            _tenantId = value.FirstOrDefault();
        }
        else
        {
            _logger.LogWarning("The request does not contain a {TenantRequestHeader} header.", TenantRequestHeader);

            if (_tenantIdRequired)
                throw new TenantIdNotFoundException();
        }

        LogReturn();

        return _tenantId;
    }

    private void LogReturn()
    {
        _logger.LogDebug("Returning tenant ID: {TenantId}.", _tenantId);
    }
}
