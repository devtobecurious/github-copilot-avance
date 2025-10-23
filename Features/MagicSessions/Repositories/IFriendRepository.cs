using Features.MagicSessions.Models;

namespace Features.MagicSessions.Repositories;

public interface IFriendRepository
{
    Task<Friend?> GetByIdAsync(Guid id);
    Task<List<Friend>> GetByIdsAsync(List<Guid> ids);
    Task<List<Friend>> GetAllAsync();
}