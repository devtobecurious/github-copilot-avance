using Features.MagicSessions.Models;

namespace Features.MagicSessions.Repositories.Implementations;

public class InMemoryMagicSessionRepository : IMagicSessionRepository
{
    private readonly List<MagicSession> _sessions = new();

    public Task<MagicSession> CreateAsync(MagicSession session)
    {
        _sessions.Add(session);
        return Task.FromResult(session);
    }

    public Task<MagicSession?> GetByIdAsync(Guid id)
    {
        var session = _sessions.FirstOrDefault(s => s.Id == id);
        return Task.FromResult(session);
    }

    public Task<List<MagicSession>> GetAllAsync()
    {
        return Task.FromResult(_sessions.ToList());
    }
}