namespace NhlCommands.DomainObjects;

public class Goalie
{
    public int Assists { get; set; }
    public int GamesPlayed { get; set; }
    public int GamesStarted { get; set; }
    public string GoalieFullName { get; set; }
    public int Goals { get; set; }
    public int GoalsAgainst { get; set; }
    public float? GoalsAgainstAverage { get; set; }
    public string LastName { get; set; }
    public int Losses { get; set; }
    public int? OtLosses { get; set; }
    public int PenaltyMinutes { get; set; }
    public int PlayerId { get; set; }
    public int Points { get; set; }
    public float? SavePct { get; set; }
    public int Saves { get; set; }
    public int SeasonId { get; set; }
    public string ShootsCatches { get; set; }
    public int ShotsAgainst { get; set; }
    public int Shutouts { get; set; }
    public string TeamAbbrevs { get; set; }
    public int TimeOnIce { get; set; }
    public int Wins { get; set; }
}