using Enterprise.Multitenancy.Abstractions;
using Enterprise.Multitenancy.Options;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.AspNetCore.Multitenancy;

public class MultitenancyParameters
{
    public bool MultitenancyEnabled { get; }
    public IGetTenantId? TenantIdService { get; }
    public IReadOnlyCollection<Type> ExcludedEntityTypes { get; }
    public ILogger? Logger { get; }

    public MultitenancyParameters()
    {
        MultitenancyEnabled = false;
        TenantIdService = null;
        ExcludedEntityTypes = new List<Type>();
    }

    public MultitenancyParameters(
        MultitenancyOptions options,
        IGetTenantId tenantIdService,
        ILogger logger,
        List<Type>? excludedEntityTypes = null)
    {
        MultitenancyEnabled = options.MultitenancyEnabled;
        TenantIdService = tenantIdService;
        Logger = logger;
        ExcludedEntityTypes = excludedEntityTypes ?? new();
    }
}
