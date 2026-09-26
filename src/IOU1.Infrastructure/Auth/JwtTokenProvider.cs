using IOU1.Application.Options;
using IOU1.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace IOU1.Infrastructure.Auth;

// TokenProvider is responsible for creating JWT tokens for authenticated users.
// It receives JwtConfig via DI (IOptions<JwtConfig>) and exposes GetJWT to
// generate a signed token string for a given User.
public sealed class JwtTokenProvider(IOptions<JwtConfig> options) : ITokenProvider
{
    // Store the Jwt configuration (secret, issuer, audience, expiry) from options
    private readonly JwtConfig _jwtConfig = options.Value;

    // GetJWT builds a JWT for the provided user and returns the serialized token.
    // Flow summary:
    // 1. Read the secret from configuration and create a symmetric security key.
    // 2. Create signing credentials using the desired algorithm (HMAC-SHA256).
    // 3. Build a list of claims that represent the user (sub, login, name, email).
    // 4. Create a SecurityTokenDescriptor containing Subject, expiry, signing creds,
    //    issuer and audience.
    // 5. Use a token handler to create/serialize the token and return it.
    public string CreateToken(User user)
    {
        // 1) Secret -> SymmetricSecurityKey
        var secretKey = _jwtConfig.SecretKey;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        // 2) Signing credentials (algorithm + key)
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 3) Create token descriptor with claims and metadata.
        var securityTokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("id", user.Id.ToString()),
                new Claim("login", user.Login),
                new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.EmailAddress),
            ]),

            // 4) Set token lifetime and signing information.
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience,
        };

        // 5) Create and serialize the token using a handler.
        // The code uses `JsonWebTokenHandler` (from Microsoft.IdentityModel.JsonWebTokens).
        // Alternatively, `JwtSecurityTokenHandler` (from System.IdentityModel.Tokens.Jwt)
        // is commonly used with SecurityTokenDescriptor. Either approach is fine but be
        // consistent with the types and packages used across the project.

        //Based on SecurityTokenDescriptor instance we create a token using a token handler
        var handler = new JsonWebTokenHandler();
        var token = handler.CreateToken(securityTokenDescriptor);

        // Return the serialized JWT string.
        return token;
    }
}
