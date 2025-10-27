namespace Features.Authentication.Models;

public class AuthenticationResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserDto User { get; set; } = null!;
}

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}

public class UserRegistrationResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class PasswordResetResponse
{
    public string Message { get; set; } = string.Empty;
}

public class EmailVerificationResponse
{
    public string Message { get; set; } = string.Empty;
}

public class LogoutResponse
{
    public string Message { get; set; } = string.Empty;
}