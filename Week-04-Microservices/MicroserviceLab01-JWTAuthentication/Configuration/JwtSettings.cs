namespace JWTAuthService.Configuration;

// ============================================================
//  JWT SETTINGS — CONFIGURATION BINDING
//  --------------------------------------
//  This class maps directly to the "JwtSettings" section
//  in appsettings.json using the Options pattern.
//
//  Instead of reading IConfiguration["JwtSettings:SecretKey"]
//  everywhere, we inject IOptions<JwtSettings> and get a
//  strongly-typed, validated configuration object.
//
//  Registered in Program.cs:
//      builder.Services.Configure<JwtSettings>(
//          builder.Configuration.GetSection("JwtSettings"));
// ============================================================

/// <summary>
/// Strongly-typed binding for the JwtSettings section in appsettings.json.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// The secret key used to sign JWT tokens.
    /// Must be at least 32 characters for HMAC-SHA256.
    /// Store in environment variables or Azure Key Vault in production.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>The token issuer — identifies who created the token.</summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>The intended audience — identifies who the token is for.</summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>Token lifetime in minutes. Default: 60.</summary>
    public int ExpirationMinutes { get; set; } = 60;
}
