using Data;
using Features.Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace Features.Authentication.Repositories.Implementations;

public class MySqlUserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public MySqlUserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users
            .OrderBy(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public async Task<int> GetFailedLoginAttemptsAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        return user?.FailedLoginAttempts ?? 0;
    }

    public async Task IncrementFailedLoginAttemptsAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.FailedLoginAttempts++;
            await _context.SaveChangesAsync();
        }
    }

    public async Task ResetFailedLoginAttemptsAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.FailedLoginAttempts = 0;
            user.LockedUntil = null;
            await _context.SaveChangesAsync();
        }
    }

    public async Task LockUserAsync(Guid userId, DateTime lockUntil)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.LockedUntil = lockUntil;
            await _context.SaveChangesAsync();
        }
    }
}