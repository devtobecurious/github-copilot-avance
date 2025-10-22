namespace Features.GameSessions.Models;

public class GameSession
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public bool IsActive { get; set; }
}
