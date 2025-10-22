using Features.GameSessions.Models;
using Features.GameSessions.Repositories;

namespace Features.GameSessions.Services;

/// <summary>
/// Service responsable de la gestion du domaine User.
/// Applique validation & règles métiers avant persistance.
/// </summary>
public class GameSessionService
{
    private readonly IGameSessionRepository _gameSessionRepository;

    public GameSessionService(IGameSessionRepository gameSessionRepository)
    {
        _gameSessionRepository = gameSessionRepository;
    }

    public async Task<GameSession> CreateGameSessionAsync(GameSession gameSession)
    {
        // Validation et règles métiers ici

        return await _gameSessionRepository.AddAsync(gameSession);
    }

    public async Task<GameSession> GetGameSessionByIdAsync(Guid id)
    {
        return await _gameSessionRepository.GetByIdAsync(id);
    }

    // Autres méthodes de gestion des GameSessions
}
