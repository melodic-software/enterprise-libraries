namespace Enterprise.MultiTenancy.Abstractions
{
    /// <summary>
    /// Implementations know how to retrieve the tenant ID.
    /// For ASP.NET Core applications, this is typically retrieved from the current request.
    /// </summary>
    public interface IGetTenantId
    {
        /// <summary>
        /// Get the current tenant identifier.
        /// </summary>
        /// <returns></returns>
        string? GetTenantId();
    }
}
