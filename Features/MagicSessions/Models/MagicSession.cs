namespace Features.MagicSessions.Models;

public class MagicSession
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateOnly StartDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<SessionFriend> Friends { get; set; } = new();
}