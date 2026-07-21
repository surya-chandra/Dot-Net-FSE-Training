using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWTAuthService.Configuration;
using JWTAuthService.Interfaces;
using JWTAuthService.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JWTAuthService.Services;






























public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<JwtService> _logger;

    public JwtService(IOptions<JwtSettings> jwtSettings, ILogger<JwtService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _logger      = logger;
    }

    public string GenerateToken(User user)
    {
        _logger.LogInformation("Generating JWT token for user Id={Id}, Role={Role}",
            user.Id, user.Role);

        var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = GetExpirationTime();

        var claims = new[]
        {

            new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),

            new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),

            new Claim(JwtRegisteredClaimNames.Email, user.Email),

            new Claim(JwtRegisteredClaimNames.Name,  user.FullName),

            new Claim(ClaimTypes.Role,               user.Role),

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
                ClockSkew                = TimeSpan.Zero   
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

    public DateTime GetExpirationTime() =>
        DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);
}
