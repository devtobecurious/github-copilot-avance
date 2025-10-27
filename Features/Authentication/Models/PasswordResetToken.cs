using System.ComponentModel.DataAnnotations;

namespace Features.Authentication.Models;

public class PasswordResetToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [MaxLength(128)]
    public string Token { get; set; } = string.Empty;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Helper properties
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
}