using NhlCommands.DomainObjects;

namespace NhlCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Show goal or point stats for seasons",
                         options: "stop|goals",
                         example: "//Show points leader for current season|seasons|//Show goal leader stats for current season|seasons --goals|//Show points winners for seasons 2010 to 2015|seasons 2010 --stop 2015")]
public class SeasonsCommand : NhlBaseCommand
{
    public SeasonsCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var start = Input.FirstArgumentToInt() == 0 ? GetCurrentSeason() : Input.FirstArgumentToInt();
        var stop = Input.OptionToInt("stop", start)+1;

        if(HasOption("goals")) ShowSeasonGoalStats(start, stop);
        else ShowSeasonPointStats(start, stop);

        return Ok();
    }

    #region points
    private void ShowSeasonPointStats(int start, int stop)
    {
        var stats = new List<SeasonPointStats>();
        for (int seasonId = start; seasonId < stop; seasonId++) stats.Add(GetPointStats(seasonId));
        
        ConsoleTableService.RenderTable(stats, this);
    }
    private SeasonPointStats GetPointStats(int seasonId)
    {
        var stats = DatabaseManager.SeasonsDb.SkaterStats.FirstOrDefault(s => s.SeasonId == seasonId);
        if (stats == null) return new SeasonPointStats { Season = $"{GetSeasonForDisplay(seasonId)}", Status = "LOCKOUT", Winner = "-",Nation = "-"};
        var standings = DatabaseManager.StandingsDb.Standings.Where(s => s.SeasonId == seasonId).ToList();
        
        
        var rounds = standings.First().Records.First().TeamRecords.First().GamesPlayed;
        var status = seasonId > 1993 && rounds < 82 ? "Interrupted" : "Completed";
        if (seasonId == GetCurrentSeason() && DateTime.Now.Month < 5) status = "Current";
        var winner = stats.Data.OrderByDescending(d => d.Points).First();
        var player = DatabaseManager.PlayersDb.People.First(p => p.Id == winner.PlayerId);
        var over99 = stats.Data.Count(p => p.Points > 99);

        return new SeasonPointStats { Season = GetSeasonForDisplay(seasonId), Status = status, Winner = player.FullName, Nation = player.Nationality, WinnerPoint = winner.Points, Over99 = over99, Games = rounds, PointsPerGame = winner.PointsPerGame };
    }
    #endregion

    #region goals
    private void ShowSeasonGoalStats(int start, int stop)
    {
        var goalStats = new List<SeasonGoalStats>();
        for (int seasonId = start; seasonId < stop; seasonId++) goalStats.Add(GetGoalStats(seasonId));
        
        ConsoleTableService.RenderTable(goalStats, this);
    }
    private SeasonGoalStats GetGoalStats(int seasonId)
    {
        var stats = DatabaseManager.SeasonsDb.SkaterStats.FirstOrDefault(s => s.SeasonId == seasonId);
        if (stats == null) return new SeasonGoalStats { Season = $"{GetSeasonForDisplay(seasonId)} LOCKOUT" };
        var standings = DatabaseManager.StandingsDb.Standings.Where(s => s.SeasonId == seasonId);
        var totalGoals = stats.Data.Sum(d => d.Goals);
        var totalTeams = standings.SelectMany(standing => standing.Records).Sum(record => record.TeamRecords.Length);
        var totalGames = (from standing in standings from record in standing.Records from teamRecord in record.TeamRecords select teamRecord.GamesPlayed).Sum()/2;
        
        var goalsPerGame = (decimal)totalGoals / totalGames;

        return new SeasonGoalStats{Goals = totalGoals, GoalsPerGame = Math.Round(goalsPerGame,2), Matches = totalGames,Season = GetSeasonForDisplay(seasonId),Teams = totalTeams};
    }
    #endregion
}