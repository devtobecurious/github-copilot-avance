using System.Security.Cryptography;
using System.Text;

namespace Features.Authentication.Services;

public class PasswordService
{
    private const int SaltRounds = 12;

    /// <summary>
    /// Hash password using BCrypt with secure salt rounds
    /// </summary>
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password, SaltRounds);
    }

    /// <summary>
    /// Verify password against hash
    /// </summary>
    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Generate cryptographically secure random token
    /// </summary>
    public string GenerateSecureToken(int length = 32)
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[length];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }

    /// <summary>
    /// Validate password strength
    /// </summary>
    public bool IsPasswordStrong(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            return false;

        var hasLower = password.Any(char.IsLower);
        var hasUpper = password.Any(char.IsUpper);
        var hasDigit = password.Any(char.IsDigit);
        var hasSpecial = password.Any(c => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(c));

        return hasLower && hasUpper && hasDigit && hasSpecial;
    }
}