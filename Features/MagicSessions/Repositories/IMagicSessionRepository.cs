using Features.MagicSessions.Models;

namespace Features.MagicSessions.Repositories;

public interface IMagicSessionRepository
{
    Task<MagicSession> CreateAsync(MagicSession session);
    Task<MagicSession?> GetByIdAsync(Guid id);
    Task<List<MagicSession>> GetAllAsync();
}