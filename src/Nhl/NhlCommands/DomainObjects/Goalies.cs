namespace NhlCommands.DomainObjects;

public class Goalies
{
    public List<Goalie> Data { get; set; } = new();
    public int Total { get; set; }
    public DateTime Updated { get; set; }
    public int SeasonId { get; set; }
}