using Features.MagicSessions.Models;

namespace Features.MagicSessions.Repositories;

public interface IDeckRepository
{
    Task<Deck?> GetByIdAsync(Guid id);
    Task<List<Deck>> GetByIdsAsync(List<Guid> ids);
    Task<List<Deck>> GetAllAsync();
}