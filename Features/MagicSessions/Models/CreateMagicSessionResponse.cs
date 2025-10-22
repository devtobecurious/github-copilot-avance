namespace Features.MagicSessions.Models;

public class CreateMagicSessionResponse
{
    public Guid Id { get; set; }
    public DateOnly StartDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<SessionFriendResponse> Friends { get; set; } = new();
}

public class SessionFriendResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public DeckResponse Deck { get; set; } = null!;
}

public class DeckResponse
{
    public Guid DeckId { get; set; }
    public string Name { get; set; } = string.Empty;
}