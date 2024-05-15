using Enterprise.Multitenancy.Abstractions;
using Enterprise.Multitenancy.Options;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.AspNetCore.MultiTenancy;

public class MultiTenancyParameters1
{
    public bool MultitenancyEnabled { get; }
    public IGetTenantId? TenantIdService { get; }
    public IReadOnlyCollection<Type> ExcludedEntityTypes { get; }
    public ILogger? Logger { get; }

    public MultiTenancyParameters1()
    {
        MultitenancyEnabled = false;
        TenantIdService = null;
        ExcludedEntityTypes = new List<Type>();
    }

    public MultiTenancyParameters1(
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
