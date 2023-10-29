using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects;
public class GoalieStatsDetail : INationality
{
    public int Place { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string TeamAbbrevs { get; set; } = string.Empty;
    public string Nationality { get; set; }= string.Empty;
    public int GamesPlayed { get; set; }
    public decimal SavePct { get; set; }
    public int Saves { get; set; }
    public int GoalsAgainst { get; set; }
    public int ShotsAgainst { get; set; }
    public decimal GoalsAgainstAverage { get; set; }
    public int Points { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
}