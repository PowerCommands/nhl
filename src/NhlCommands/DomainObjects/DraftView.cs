using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects;
public class DraftView : INationality
{
    public int Year { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string BirthCity{ get; set; } = string.Empty;
    public string BirthCountry{ get; set; } = string.Empty;
    public string AmateurTeam{ get; set; } = string.Empty;
    public int Round{ get; set; }
    public int PickOverall { get; set; }
    public string Nationality { get; set; } = string.Empty;
}