using Microsoft.AspNetCore.Authorization;

namespace Enterprise.Api.Security.Permissions;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) : base(policy: permission)
    {

    }
}