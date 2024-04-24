using Microsoft.AspNetCore.Authentication;

namespace Enterprise.Api.Security.ApiKey.Options;

public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; } = "VerySecret"; // TODO: This should be loaded from Azure Key Vault, secrets manager, etc.
}