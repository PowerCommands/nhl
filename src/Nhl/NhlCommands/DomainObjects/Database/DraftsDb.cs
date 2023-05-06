using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects.Database;

public class DraftsDb : IDatabase
{
    public List<DraftYear> DraftYears { get; set; } = new();
    public DateTime Updated { get; set; }
    public List<string> GetDescriptions() => new() { $"Draft years in database: {string.Join(',', DraftYears.OrderBy(d => d.Year).Select(d => d.Year))}" };
}