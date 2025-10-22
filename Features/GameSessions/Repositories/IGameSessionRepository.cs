using Features.GameSessions.Models;

namespace Features.GameSessions.Repositories;

public interface IGameSessionRepository
{
    Task<GameSession> AddAsync(GameSession gameSession);
    Task<GameSession> GetByIdAsync(Guid id);
    Task<GameSession> UpdateAsync(GameSession gameSession);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<GameSession>> GetAllAsync();
}
