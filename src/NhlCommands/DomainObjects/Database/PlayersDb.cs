using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects.Database;

public class PlayersDb : IDatabase
{
    public List<Player> People { get; set; } = new();
    public DateTime Updated { get; set; }
    public List<string> GetDescriptions()
    {
        var descriptions = new List<string>
        {
            $"Number of players: {People.Count}",
            $"Nationalities: {string.Join('|', People.Select(p => p.Nationality).Distinct())}"
        };
        return descriptions;
    }
}