using Features.MagicSessions.Models;

namespace Features.MagicSessions.Repositories.Implementations;

public class InMemoryFriendRepository : IFriendRepository
{
    private readonly List<Friend> _friends;

    public InMemoryFriendRepository()
    {
        // Initialize with sample data for testing
        _friends = new List<Friend>
        {
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), FirstName = "John", LastName = "Doe", Nickname = "JD" },
            new() { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), FirstName = "Jane", LastName = "Smith", Nickname = "JS" },
            new() { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), FirstName = "Bob", LastName = "Wilson", Nickname = "Bobby" }
        };
    }

    public Task<Friend?> GetByIdAsync(Guid id)
    {
        var friend = _friends.FirstOrDefault(f => f.Id == id);
        return Task.FromResult(friend);
    }

    public Task<List<Friend>> GetByIdsAsync(List<Guid> ids)
    {
        var friends = _friends.Where(f => ids.Contains(f.Id)).ToList();
        return Task.FromResult(friends);
    }

    public Task<List<Friend>> GetAllAsync()
    {
        return Task.FromResult(_friends.ToList());
    }
}