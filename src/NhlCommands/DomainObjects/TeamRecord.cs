namespace NhlCommands.DomainObjects;

public class TeamRecord
{
    public Team Team { get; set; }
    public LeagueRecord LeagueRecord { get; set; }
    public int RegulationWins { get; set; }
    public int GoalsAgainst { get; set; }
    public int GoalsScored { get; set; }
    public int Points { get; set; }
    public string DivisionRank { get; set; }
    public string DivisionL10Rank { get; set; }
    public string DivisionRoadRank { get; set; }
    public string DivisionHomeRank { get; set; }
    public string ConferenceRank { get; set; }
    public string ConferenceL10Rank { get; set; }
    public string ConferenceRoadRank { get; set; }
    public string ConferenceHomeRank { get; set; }
    public string LeagueRank { get; set; }
    public string LeagueL10Rank { get; set; }
    public string LeagueRoadRank { get; set; }
    public string LeagueHomeRank { get; set; }
    public string WildCardRank { get; set; }
    public int Row { get; set; }
    public int GamesPlayed { get; set; }
    public Streak Streak { get; set; }
    public string ClinchIndicator { get; set; }
    public float? PointsPercentage { get; set; }
    public string PpDivisionRank { get; set; }
    public string PpConferenceRank { get; set; }
    public string PpLeagueRank { get; set; }
    public DateTime LastUpdated { get; set; }
}