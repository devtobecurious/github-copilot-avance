using Data;
using Features.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace Features.Authentication.Repositories.Implementations;

public class MySqlEmailVerificationTokenRepository : IEmailVerificationTokenRepository
{
    private readonly ApplicationDbContext _context;

    public MySqlEmailVerificationTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EmailVerificationToken?> GetByTokenAsync(string token)
    {
        return await _context.EmailVerificationTokens
            .Include(evt => evt.User)
            .FirstOrDefaultAsync(evt => evt.Token == token);
    }

    public async Task<EmailVerificationToken> CreateAsync(EmailVerificationToken token)
    {
        _context.EmailVerificationTokens.Add(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task DeleteAsync(Guid id)
    {
        var token = await _context.EmailVerificationTokens.FindAsync(id);
        if (token != null)
        {
            _context.EmailVerificationTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteByUserIdAsync(Guid userId)
    {
        var tokens = await _context.EmailVerificationTokens
            .Where(evt => evt.UserId == userId)
            .ToListAsync();

        _context.EmailVerificationTokens.RemoveRange(tokens);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpiredTokensAsync()
    {
        var expiredTokens = await _context.EmailVerificationTokens
            .Where(evt => evt.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        _context.EmailVerificationTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync();
    }
}

public class MySqlPasswordResetTokenRepository : IPasswordResetTokenRepository
{
    private readonly ApplicationDbContext _context;

    public MySqlPasswordResetTokenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PasswordResetToken?> GetByTokenAsync(string token)
    {
        return await _context.PasswordResetTokens
            .Include(prt => prt.User)
            .FirstOrDefaultAsync(prt => prt.Token == token);
    }

    public async Task<PasswordResetToken> CreateAsync(PasswordResetToken token)
    {
        _context.PasswordResetTokens.Add(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task DeleteAsync(Guid id)
    {
        var token = await _context.PasswordResetTokens.FindAsync(id);
        if (token != null)
        {
            _context.PasswordResetTokens.Remove(token);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteByUserIdAsync(Guid userId)
    {
        var tokens = await _context.PasswordResetTokens
            .Where(prt => prt.UserId == userId)
            .ToListAsync();

        _context.PasswordResetTokens.RemoveRange(tokens);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteExpiredTokensAsync()
    {
        var expiredTokens = await _context.PasswordResetTokens
            .Where(prt => prt.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();

        _context.PasswordResetTokens.RemoveRange(expiredTokens);
        await _context.SaveChangesAsync();
    }
}