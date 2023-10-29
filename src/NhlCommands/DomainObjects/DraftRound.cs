namespace NhlCommands.DomainObjects;

public class DraftRound
{
    public int RoundNumber { get; set; }
    public string Round { get; set; }
    public DraftPick[] Picks { get; set; }
}