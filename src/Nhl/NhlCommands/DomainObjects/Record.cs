namespace NhlCommands.DomainObjects;

public class Record
{
    public string StandingsType { get; set; }
    public League League { get; set; }
    public Division Division { get; set; }
    public Conference Conference { get; set; }
    public TeamRecord[] TeamRecords { get; set; }
    
}