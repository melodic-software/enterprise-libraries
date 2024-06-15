using System.Globalization;
using System.Security.Claims;
using Enterprise.Security.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace Enterprise.Security.Tests.Unit.Tokens;

public class JwtTokenTests
{
    [Fact]
    public void GenerateTokenString_ShouldReturnToken_WhenValidInputsProvided()
    {
        // Arrange
        var securityKeyGenerationService = new SecurityKeyGenerationService();
        var signingCredentialGenerationService = new SigningCredentialGenerationService();
        var jwtTokenGenerationService = new JwtTokenGenerationService();

        string secret = "thisisthesecretforgeneratingakey(mustbeatleast32bitlong)";
        SymmetricSecurityKey securityKey = securityKeyGenerationService.GenerateSecurityKey(secret);

        SigningCredentials signingCredentials = signingCredentialGenerationService.Generate(securityKey);

        int userId = 1;
        string userFirstName = "John";
        string userLastName = "Doe";
        string city = "New York City";

        List<Claim> claims =
        [
            new("sub", userId.ToString(CultureInfo.InvariantCulture)),
            new("given_name", userFirstName),
            new("family_name", userLastName),
            new("city", city)
        ];

        string issuer = "https://security-token-service:12345";
        string audience = "configuration-api";
        DateTime notBefore = DateTime.UtcNow;
        DateTime expiration = notBefore.AddHours(1);

        // Act
        string token = jwtTokenGenerationService.GenerateTokenString(issuer, audience, claims, notBefore, expiration, signingCredentials);

        // Assert
        token.Should().NotBeNull();
    }
}
