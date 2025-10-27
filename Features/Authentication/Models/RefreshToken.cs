using System.ComponentModel.DataAnnotations;

namespace Features.Authentication.Models;

public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(128)]
    public string TokenHash { get; set; } = string.Empty;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [MaxLength(500)]
    public string? DeviceInfo { get; set; }
    
    [MaxLength(45)]
    public string? IpAddress { get; set; }
    
    // Helper properties
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
}