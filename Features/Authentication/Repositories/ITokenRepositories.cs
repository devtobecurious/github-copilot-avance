using Features.Authentication.Models;

namespace Features.Authentication.Repositories;

public interface IEmailVerificationTokenRepository
{
    Task<EmailVerificationToken?> GetByTokenAsync(string token);
    Task<EmailVerificationToken> CreateAsync(EmailVerificationToken token);
    Task DeleteAsync(Guid id);
    Task DeleteByUserIdAsync(Guid userId);
    Task DeleteExpiredTokensAsync();
}

public interface IPasswordResetTokenRepository
{
    Task<PasswordResetToken?> GetByTokenAsync(string token);
    Task<PasswordResetToken> CreateAsync(PasswordResetToken token);
    Task DeleteAsync(Guid id);
    Task DeleteByUserIdAsync(Guid userId);
    Task DeleteExpiredTokensAsync();
}