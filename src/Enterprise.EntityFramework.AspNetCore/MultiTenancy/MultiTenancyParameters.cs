using Enterprise.Multitenancy.Abstractions;
using Enterprise.Multitenancy.Options;
using Microsoft.Extensions.Logging;

namespace Enterprise.EntityFramework.AspNetCore.MultiTenancy;

public class MultiTenancyParameters
{
    public bool MultiTenancyEnabled { get; }
    public IGetTenantId? TenantIdService { get; }
    public IReadOnlyCollection<Type> ExcludedEntityTypes { get; }
    public ILogger? Logger { get; }

    public MultiTenancyParameters()
    {
        MultiTenancyEnabled = false;
        TenantIdService = null;
        ExcludedEntityTypes = new List<Type>();
    }

    public MultiTenancyParameters(
        MultiTenancyConfigOptions configOptions,
        IGetTenantId tenantIdService,
        ILogger logger,
        List<Type>? excludedEntityTypes = null)
    {
        MultiTenancyEnabled = configOptions.MultiTenancyEnabled;
        TenantIdService = tenantIdService;
        Logger = logger;
        ExcludedEntityTypes = excludedEntityTypes ?? new();
    }
}