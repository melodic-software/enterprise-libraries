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
        MultiTenancyConfigOptions configOptions,
        IGetTenantId tenantIdService,
        ILogger logger,
        List<Type>? excludedEntityTypes = null)
    {
        MultitenancyEnabled = configOptions.MultiTenancyEnabled;
        TenantIdService = tenantIdService;
        Logger = logger;
        ExcludedEntityTypes = excludedEntityTypes ?? new();
    }
}
