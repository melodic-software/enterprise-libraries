namespace Enterprise.Multitenancy.Exceptions;

/// <summary>
/// This exception is thrown when a request to get a tenant ID has been made, and it can't be resolved.
/// This is typically only if the application has multi-tenancy enabled.
/// </summary>
public class TenantIdNotFoundException : Exception
{
    public TenantIdNotFoundException()
    {
    }

    public TenantIdNotFoundException(string message)
        : base(message)
    {
    }

    public TenantIdNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}