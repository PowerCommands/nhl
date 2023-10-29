namespace NhlCommands.DomainObjects;

public class Skater
{
    public int Assists { get; set; }
    public int EvGoals { get; set; }
    public int EvPoints { get; set; }
    public float? FaceoffWinPct { get; set; }
    public int GameWinningGoals { get; set; }
    public int GamesPlayed { get; set; }
    public int Goals { get; set; }
    public string LastName { get; set; } = string.Empty;
    public int OtGoals { get; set; }
    public int PenaltyMinutes { get; set; }
    public int PlayerId { get; set; }
    public int PlusMinus { get; set; }
    public int Points { get; set; }
    public float? PointsPerGame { get; set; }
    public string PositionCode { get; set; } = string.Empty;
    public int PpGoals { get; set; }
    public int PpPoints { get; set; }
    public int SeasonId { get; set; }
    public int ShGoals { get; set; }
    public int ShPoints { get; set; }
    public float? ShootingPct { get; set; }
    public string ShootsCatches { get; set; } = string.Empty;
    public int Shots { get; set; }
    public string SkaterFullName { get; set; } = string.Empty;
    public string TeamAbbrevs { get; set; } = string.Empty;
    public float? TimeOnIcePerGame { get; set; }
}