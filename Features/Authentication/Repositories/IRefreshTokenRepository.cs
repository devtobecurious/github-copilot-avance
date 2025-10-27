using Features.Authentication.Models;

namespace Features.Authentication.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
    Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
    Task<RefreshToken> UpdateAsync(RefreshToken refreshToken);
    Task DeleteAsync(Guid id);
    Task<List<RefreshToken>> GetByUserIdAsync(Guid userId);
    Task RevokeAllByUserIdAsync(Guid userId);
    Task RevokeTokenAsync(string tokenHash);
    Task DeleteExpiredTokensAsync();
    Task<int> GetActiveTokenCountByUserIdAsync(Guid userId);
}