namespace Features.MagicSessions.Models;

public class SessionFriend
{
    public Guid SessionId { get; set; }
    public MagicSession Session { get; set; } = null!;
    public Guid FriendId { get; set; }
    public Friend Friend { get; set; } = null!;
    public Guid DeckId { get; set; }
    public Deck Deck { get; set; } = null!;
}