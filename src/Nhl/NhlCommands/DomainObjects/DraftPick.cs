namespace NhlCommands.DomainObjects;

public class DraftPick
{
    public int Year { get; set; }
    public string Round { get; set; }
    public int PickOverall { get; set; }
    public int PickInRound { get; set; }
    public DraftTeam Team { get; set; }
    public DraftProspect Prospect { get; set; }
}