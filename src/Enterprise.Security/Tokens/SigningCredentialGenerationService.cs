using Microsoft.IdentityModel.Tokens;

namespace Enterprise.Security.Tokens;

public class SigningCredentialGenerationService
{
    public SigningCredentials Generate(SymmetricSecurityKey securityKey)
    {
        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }
}
