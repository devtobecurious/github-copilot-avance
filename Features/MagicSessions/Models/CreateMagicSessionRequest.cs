using System.ComponentModel.DataAnnotations;

namespace Features.MagicSessions.Models;

public class CreateMagicSessionRequest
{
    [Required]
    public DateOnly StartDate { get; set; }
    
    [Required]
    public TimeOnly StartTime { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "At least one friend is required")]
    public List<SessionFriendRequest> Friends { get; set; } = new();
}

public class SessionFriendRequest
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public Guid DeckId { get; set; }
}