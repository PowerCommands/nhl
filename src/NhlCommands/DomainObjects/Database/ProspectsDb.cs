using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects.Database;

public class ProspectsDb : IDatabase
{
    public List<Prospect> Prospects { get; set; } = new();
    public DateTime Updated { get; set; }
    public List<string> GetDescriptions() => new() { $"Number of players: {Prospects.Count}" };
}