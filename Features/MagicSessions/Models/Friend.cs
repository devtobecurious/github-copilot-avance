namespace Features.MagicSessions.Models;

public class Friend
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
}