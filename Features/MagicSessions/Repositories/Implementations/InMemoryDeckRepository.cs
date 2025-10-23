using Features.MagicSessions.Models;

namespace Features.MagicSessions.Repositories.Implementations;

public class InMemoryDeckRepository : IDeckRepository
{
    private readonly List<Deck> _decks;

    public InMemoryDeckRepository()
    {
        // Initialize with sample Magic decks for testing
        _decks = new List<Deck>
        {
            new() { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), Name = "Blue Control" },
            new() { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), Name = "Red Aggro" },
            new() { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), Name = "Green Ramp" },
            new() { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), Name = "White Weenie" },
            new() { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), Name = "Black Midrange" }
        };
    }

    public Task<Deck?> GetByIdAsync(Guid id)
    {
        var deck = _decks.FirstOrDefault(d => d.Id == id);
        return Task.FromResult(deck);
    }

    public Task<List<Deck>> GetByIdsAsync(List<Guid> ids)
    {
        var decks = _decks.Where(d => ids.Contains(d.Id)).ToList();
        return Task.FromResult(decks);
    }

    public Task<List<Deck>> GetAllAsync()
    {
        return Task.FromResult(_decks.ToList());
    }
}