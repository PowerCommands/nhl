namespace NhlCommands.DomainObjects;

public class Skaters
{
    public List<Skater> Data { get; set; } = new();
    public int Total { get; set; }
    public int SeasonId { get; set; }
    public DateTime Updated { get; set; }
}