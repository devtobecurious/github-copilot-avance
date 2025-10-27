using Features.Authentication.Models;

namespace Features.Authentication.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task DeleteAsync(Guid id);
    Task<List<User>> GetAllAsync();
    Task<bool> ExistsByEmailAsync(string email);
    Task<int> GetFailedLoginAttemptsAsync(Guid userId);
    Task IncrementFailedLoginAttemptsAsync(Guid userId);
    Task ResetFailedLoginAttemptsAsync(Guid userId);
    Task LockUserAsync(Guid userId, DateTime lockUntil);
}