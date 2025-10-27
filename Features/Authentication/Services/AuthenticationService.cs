using Features.Authentication.Models;
using Features.Authentication.Repositories;

namespace Features.Authentication.Services;

public class AuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
    private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;
    private readonly JwtTokenService _jwtTokenService;
    private readonly PasswordService _passwordService;
    private readonly ILogger<AuthenticationService> _logger;

    private const int MaxFailedAttempts = 5;
    private const int LockoutDurationMinutes = 15;

    public AuthenticationService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IEmailVerificationTokenRepository emailVerificationTokenRepository,
        IPasswordResetTokenRepository passwordResetTokenRepository,
        JwtTokenService jwtTokenService,
        PasswordService passwordService,
        ILogger<AuthenticationService> logger)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _emailVerificationTokenRepository = emailVerificationTokenRepository;
        _passwordResetTokenRepository = passwordResetTokenRepository;
        _jwtTokenService = jwtTokenService;
        _passwordService = passwordService;
        _logger = logger;
    }

    /// <summary>
    /// Register new user account
    /// </summary>
    public async Task<UserRegistrationResponse> RegisterAsync(UserRegistrationRequest request)
    {
        // Check if email already exists
        if (await _userRepository.ExistsByEmailAsync(request.Email))
        {
            throw new InvalidOperationException("Email already registered");
        }

        // Validate password strength
        if (!_passwordService.IsPasswordStrong(request.Password))
        {
            throw new ArgumentException("Password does not meet security requirements");
        }

        // Create user
        var user = new User
        {
            Email = request.Email.ToLower().Trim(),
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            PasswordHash = _passwordService.HashPassword(request.Password),
            IsEmailVerified = false,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateAsync(user);

        // Generate email verification token
        await GenerateEmailVerificationTokenAsync(user.Id);

        _logger.LogInformation("User registered successfully: {UserId}", user.Id);

        return new UserRegistrationResponse
        {
            Id = user.Id,
            Email = user.Email,
            Message = "Registration successful. Please check your email to verify your account."
        };
    }

    /// <summary>
    /// Authenticate user and generate tokens
    /// </summary>
    public async Task<AuthenticationResponse> LoginAsync(UserLoginRequest request, string? ipAddress = null, string? deviceInfo = null)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null)
        {
            _logger.LogWarning("Login attempt with non-existent email: {Email}", request.Email);
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Check if account is locked
        if (user.IsLocked)
        {
            _logger.LogWarning("Login attempt on locked account: {UserId}", user.Id);
            throw new UnauthorizedAccessException("Account is temporarily locked. Please try again later.");
        }

        // Verify password
        if (!_passwordService.VerifyPassword(request.Password, user.PasswordHash))
        {
            await _userRepository.IncrementFailedLoginAttemptsAsync(user.Id);
            
            // Lock account after max failed attempts
            if (user.FailedLoginAttempts + 1 >= MaxFailedAttempts)
            {
                await _userRepository.LockUserAsync(user.Id, DateTime.UtcNow.AddMinutes(LockoutDurationMinutes));
                _logger.LogWarning("Account locked due to failed attempts: {UserId}", user.Id);
            }

            _logger.LogWarning("Failed login attempt for user: {UserId}", user.Id);
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        // Check email verification
        if (!user.IsEmailVerified)
        {
            throw new UnauthorizedAccessException("Please verify your email address before logging in");
        }

        // Reset failed login attempts on successful login
        await _userRepository.ResetFailedLoginAttemptsAsync(user.Id);

        // Update last login
        user.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Generate tokens
        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var refreshTokenHash = _passwordService.HashPassword(refreshToken);

        // Store refresh token
        var refreshTokenEntity = new RefreshToken
        {
            TokenHash = refreshTokenHash,
            UserId = user.Id,
            ExpiresAt = _jwtTokenService.GetRefreshTokenExpirationTime(),
            IpAddress = ipAddress,
            DeviceInfo = deviceInfo
        };

        await _refreshTokenRepository.CreateAsync(refreshTokenEntity);

        _logger.LogInformation("User logged in successfully: {UserId}", user.Id);

        return new AuthenticationResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = _jwtTokenService.GetAccessTokenExpirationTime(),
            User = MapToUserDto(user)
        };
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    public async Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenHash = _passwordService.HashPassword(refreshToken);
        var tokenEntity = await _refreshTokenRepository.GetByTokenHashAsync(refreshTokenHash);

        if (tokenEntity == null || !tokenEntity.IsActive)
        {
            _logger.LogWarning("Invalid refresh token used");
            throw new UnauthorizedAccessException("Invalid refresh token");
        }

        var user = tokenEntity.User;
        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("User account is disabled");
        }

        // Generate new tokens
        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var newRefreshTokenHash = _passwordService.HashPassword(newRefreshToken);

        // Revoke old refresh token
        tokenEntity.IsRevoked = true;
        await _refreshTokenRepository.UpdateAsync(tokenEntity);

        // Create new refresh token
        var newTokenEntity = new RefreshToken
        {
            TokenHash = newRefreshTokenHash,
            UserId = user.Id,
            ExpiresAt = _jwtTokenService.GetRefreshTokenExpirationTime(),
            IpAddress = tokenEntity.IpAddress,
            DeviceInfo = tokenEntity.DeviceInfo
        };

        await _refreshTokenRepository.CreateAsync(newTokenEntity);

        _logger.LogInformation("Token refreshed for user: {UserId}", user.Id);

        return new AuthenticationResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = _jwtTokenService.GetAccessTokenExpirationTime(),
            User = MapToUserDto(user)
        };
    }

    /// <summary>
    /// Logout user and revoke refresh token
    /// </summary>
    public async Task<LogoutResponse> LogoutAsync(string refreshToken)
    {
        var refreshTokenHash = _passwordService.HashPassword(refreshToken);
        await _refreshTokenRepository.RevokeTokenAsync(refreshTokenHash);

        _logger.LogInformation("User logged out successfully");

        return new LogoutResponse
        {
            Message = "Logged out successfully"
        };
    }

    /// <summary>
    /// Generate email verification token
    /// </summary>
    private async Task GenerateEmailVerificationTokenAsync(Guid userId)
    {
        // Delete existing tokens
        await _emailVerificationTokenRepository.DeleteByUserIdAsync(userId);

        var token = new EmailVerificationToken
        {
            Token = _passwordService.GenerateSecureToken(),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddHours(24)
        };

        await _emailVerificationTokenRepository.CreateAsync(token);

        // TODO: Send email with verification link
        _logger.LogInformation("Email verification token generated for user: {UserId}", userId);
    }

    /// <summary>
    /// Verify email address
    /// </summary>
    public async Task<EmailVerificationResponse> VerifyEmailAsync(string token)
    {
        var tokenEntity = await _emailVerificationTokenRepository.GetByTokenAsync(token);

        if (tokenEntity == null || tokenEntity.IsExpired)
        {
            throw new ArgumentException("Invalid or expired verification token");
        }

        var user = tokenEntity.User;
        user.IsEmailVerified = true;
        await _userRepository.UpdateAsync(user);

        // Clean up token
        await _emailVerificationTokenRepository.DeleteAsync(tokenEntity.Id);

        _logger.LogInformation("Email verified for user: {UserId}", user.Id);

        return new EmailVerificationResponse
        {
            Message = "Email verified successfully"
        };
    }

    private static UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            IsEmailVerified = user.IsEmailVerified,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}