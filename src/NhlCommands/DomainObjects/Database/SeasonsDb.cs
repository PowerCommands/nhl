using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects.Database;

public class SeasonsDb : IDatabase
{
    public List<Skaters> SkaterStats { get; set; } = new();
    public List<Goalies> GoalieStats { get; set; } = new();
    public DateTime Updated { get; set; }
    public List<string> GetDescriptions() => new() { $"Seasons: {string.Join(',', SkaterStats.OrderBy(s => s.SeasonId).Select(s => s.SeasonId))}" };
}