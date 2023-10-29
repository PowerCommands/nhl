namespace NhlCommands.DomainObjects;

public class Standing
{
    public Record[] Records { get; set; }
    public int SeasonId { get; set; }
    public DateTime Updated { get; set; }
}