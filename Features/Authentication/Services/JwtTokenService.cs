using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Features.Authentication.Models;

namespace Features.Authentication.Services;

public class JwtOptions
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string AccessTokenLifetime { get; set; } = "00:15:00";
    public string RefreshTokenLifetime { get; set; } = "7.00:00:00";
}

public class JwtTokenService
{
    private readonly JwtOptions _jwtOptions;
    private readonly PasswordService _passwordService;

    public JwtTokenService(IOptions<JwtOptions> jwtOptions, PasswordService passwordService)
    {
        _jwtOptions = jwtOptions.Value;
        _passwordService = passwordService;
    }

    /// <summary>
    /// Generate JWT access token for user
    /// </summary>
    public string GenerateAccessToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim("email_verified", user.IsEmailVerified.ToString()),
            new Claim(ClaimTypes.Role, "User"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var lifetime = TimeSpan.Parse(_jwtOptions.AccessTokenLifetime);
        var expires = DateTime.UtcNow.Add(lifetime);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generate secure refresh token
    /// </summary>
    public string GenerateRefreshToken()
    {
        return _passwordService.GenerateSecureToken(64);
    }

    /// <summary>
    /// Get token expiration time
    /// </summary>
    public DateTime GetAccessTokenExpirationTime()
    {
        var lifetime = TimeSpan.Parse(_jwtOptions.AccessTokenLifetime);
        return DateTime.UtcNow.Add(lifetime);
    }

    /// <summary>
    /// Get refresh token expiration time
    /// </summary>
    public DateTime GetRefreshTokenExpirationTime()
    {
        var lifetime = TimeSpan.Parse(_jwtOptions.RefreshTokenLifetime);
        return DateTime.UtcNow.Add(lifetime);
    }

    /// <summary>
    /// Validate JWT token structure (without signature verification)
    /// </summary>
    public ClaimsPrincipal? ValidateTokenStructure(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Extract user ID from token claims
    /// </summary>
    public Guid? GetUserIdFromToken(ClaimsPrincipal principal)
    {
        var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub);
        return Guid.TryParse(userIdClaim?.Value, out var userId) ? userId : null;
    }
}