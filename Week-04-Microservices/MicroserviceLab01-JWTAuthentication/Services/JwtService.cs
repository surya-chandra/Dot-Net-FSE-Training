using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTAuthService.Configuration;
using JWTAuthService.Interfaces;
using JWTAuthService.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthService.Services;

// ============================================================
//  JWT SERVICE — TOKEN GENERATION & VALIDATION
//  ---------------------------------------------
//  This service is responsible ONLY for JWT operations.
//  It has no knowledge of users, passwords, or the database.
//
//  JWT STRUCTURE:
//  --------------
//  A JWT has three Base64-encoded parts separated by dots:
//
//  Header.Payload.Signature
//
//  Header   — algorithm (HS256) and token type (JWT)
//  Payload  — claims: userId, email, role, expiry, issuer, audience
//  Signature — HMAC-SHA256(header + payload, secretKey)
//
//  The signature ensures the token has not been tampered with.
//  Anyone can decode the header and payload (they are Base64, not encrypted).
//  Only the server can VERIFY the signature because only it knows the secret key.
//
//  CLAIMS:
//  -------
//  Claims are key-value pairs embedded in the token payload.
//  Standard claims: sub (subject/userId), email, role, exp (expiry)
//  Custom claims: FullName
// ============================================================

/// <summary>
/// Handles JWT token generation, signing, and validation.
/// Injected as a scoped service via IOptions&lt;JwtSettings&gt;.
/// </summary>
public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<JwtService> _logger;

    public JwtService(IOptions<JwtSettings> jwtSettings, ILogger<JwtService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _logger      = logger;
    }

    /// <inheritdoc/>
    public string GenerateToken(User user)
    {
        _logger.LogInformation("Generating JWT token for user Id={Id}, Role={Role}",
            user.Id, user.Role);

        // The signing key — must match what's used in AddAuthentication()
        var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = GetExpirationTime();

        // Claims embedded in the token payload
        var claims = new[]
        {
            // sub — standard claim for the subject (user identifier)
            new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),

            // jti — unique token ID (prevents token replay attacks)
            new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),

            // email — user's email address
            new Claim(JwtRegisteredClaimNames.Email, user.Email),

            // name — user's full name
            new Claim(JwtRegisteredClaimNames.Name,  user.FullName),

            // role — used by [Authorize(Roles="Admin")] / [Authorize(Roles="User")]
            new Claim(ClaimTypes.Role,               user.Role),

            // NameIdentifier — used by User.FindFirstValue(ClaimTypes.NameIdentifier)
            new Claim(ClaimTypes.NameIdentifier,     user.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer:             _jwtSettings.Issuer,
            audience:           _jwtSettings.Audience,
            claims:             claims,
            notBefore:          DateTime.UtcNow,
            expires:            expires,
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        _logger.LogInformation("JWT token generated. Expires at {Expiry}", expires);
        return tokenString;
    }

    /// <inheritdoc/>
    public int? ValidateTokenAndGetUserId(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key          = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = new SymmetricSecurityKey(key),
                ValidateIssuer           = true,
                ValidIssuer              = _jwtSettings.Issuer,
                ValidateAudience         = true,
                ValidAudience            = _jwtSettings.Audience,
                ValidateLifetime         = true,
                ClockSkew                = TimeSpan.Zero   // no tolerance for expiry
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId   = int.Parse(jwtToken.Claims
                .First(c => c.Type == JwtRegisteredClaimNames.Sub).Value);

            return userId;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Token validation failed: {Message}", ex.Message);
            return null;
        }
    }

    /// <inheritdoc/>
    public DateTime GetExpirationTime() =>
        DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);
}
