namespace Enterprise.Multitenancy.Options;

public class MultitenancyConfigOptions
{
    public const string ConfigSectionKey = "Custom:Multitenancy";

    /// <summary>
    /// Determines if multi tenancy related services are registered.
    /// If this is not enabled, the application will fail when requests for any of these services are made.
    /// </summary>
    public bool MultiTenancyEnabled { get; set; } = false;

    /// <summary>
    /// Determines if a <see cref="Exceptions.TenantIdNotFoundException"/> will be thrown if the tenant ID cannot be located (when requested).
    /// </summary>
    public bool TenantIdRequired { get; set; } = false;
}
