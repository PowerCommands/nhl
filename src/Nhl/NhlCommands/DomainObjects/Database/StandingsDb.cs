using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects.Database;
public class StandingsDb : IDatabase
{
    public List<Standing> Standings { get; set; } = new();
    public DateTime Updated { get; set; }
    public List<string> GetDescriptions() => new() { $"Season years in database: {string.Join(',', Standings.OrderBy(d => d.SeasonId).Select(d => d.SeasonId))}" };
}