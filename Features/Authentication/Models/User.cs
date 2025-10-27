using System.ComponentModel.DataAnnotations;

namespace Features.Authentication.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(128)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    public bool IsEmailVerified { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
    public int FailedLoginAttempts { get; set; } = 0;
    public DateTime? LockedUntil { get; set; }
    
    // Navigation properties
    public List<RefreshToken> RefreshTokens { get; set; } = new();
    
    // Helper properties
    public string FullName => $"{FirstName} {LastName}";
    public bool IsLocked => LockedUntil.HasValue && LockedUntil > DateTime.UtcNow;
}