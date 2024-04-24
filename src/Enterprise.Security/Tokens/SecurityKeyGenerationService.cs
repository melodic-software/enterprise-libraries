using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Enterprise.Security.Tokens;

public class SecurityKeyGenerationService
{
    public SymmetricSecurityKey GenerateSecurityKey(string secret)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(secret);

        byte[] secretBytes = Encoding.ASCII.GetBytes(secret);
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secretBytes);

        return securityKey;
    }
}