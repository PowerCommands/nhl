namespace NhlCommands.DomainObjects;

public class Draft
{
    public int DraftYear { get; set; }
    public DraftRound[] Rounds { get; set; }
}