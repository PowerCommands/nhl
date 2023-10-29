using NhlCommands.DomainObjects;
using NhlCommands.Extensions;

namespace NhlCommands.Commands;

[PowerCommandDesign( description: "Show points statistic for a specific season or current season, default top count is 25",
                         options: "team|goalies|at-least-game-count|name|top|goals|assists|points-per-game|forward|defense|rookie",
                     suggestions: "SWE|FIN|CAN|USA|CZE|SVK|DEU|AUS|CHE|SVN|NOR|DNK|NLD|BLR|LVA|FRA|AUT|GBR|UKR|HRV|LTU|KAZ|POL|NGA|BHS|ITA|RUS",
                         example: "//Show points stats for current top 25 (default)|stats|//Show points stats for 2010, show first top 100|stats 2010 --top 100|//Show points stats for all swedish players for current season|stats --nation swe|//Compare swedish and finnish players for the current season in the top 100|stats SWE FIN --top 100|//Compare swedish and finnish players for season 2016/2017 in the top 100|stats 2017 SWE FIN --top 100|//Show rookie points stats for current season top 25 (default)|stats --rookie|//Show stats for Finnish players current season in Carolina|stats FIN --team car|//Show defense men points stats for current season top 25 (default)|stats --defense|//Show stats for current season top 25 goal scorer (default)|stats --goals|//Show stats for current season top 25 assists (default)|stats --assists|//Show stats for current season top 25 points per game (default)|stats --goals-per-game")]
public class StatsCommand : NhlBaseCommand
{
    public StatsCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override RunResult Run()
    {
        var seasonId = GetSeasonId();
        var nations = GetNations();
        var team = GetOptionValue("team").ToLower();
        var take = Input.OptionToInt("top", nations.Count > 0 || HasOption("rookie") ? 2000 : 25);
        if (!string.IsNullOrEmpty(team)) take = 2000;
        var name = GetOptionValue("name").ToLower();
        var atLeastGameCount = Input.OptionToInt("at-least-game-count", 1);
        
        
        if(HasOption("goalies")) ShowGoalies(seasonId, nations, take, name, atLeastGameCount, team);
        else ShowSkaters(seasonId, nations, take, name, atLeastGameCount, team);
        
        return Ok();
    }
    public void ShowSkaters(int seasonId, List<string> nations, int take, string name, int atLeastGameCount, string team)
    {
        var season = DatabaseManager.SeasonsDb.SkaterStats.First(s => s.SeasonId == seasonId);
        var stats = season.Data.Take(take).ToArray();

        //filter
        if(HasOption("forward")) stats = season.Data.Where(d => d.PositionCode != "D").Take(take).ToArray();
        if(HasOption("defense")) stats = season.Data.Where(d => d.PositionCode == "D").Take(take).ToArray();
        

        //Order by other then points
        if(HasOption("goals")) stats = season.Data.OrderByDescending(d => d.Goals).Take(take).ToArray();
        else if(HasOption("points-per-game")) stats = season.Data.OrderByDescending(d => d.PointsPerGame).Take(take).ToArray();
        
        var pointsTable = new List<SkaterStatsDetail>();
        for (int i= 0; i < stats.Length; i++)
        {
            var playerStat = stats[i];
            var player = DatabaseManager.PlayersDb.People.FirstOrDefault(p => p.Id == playerStat.PlayerId) ?? new Player{FullName = "MISSING"};
            var pointTableItem = new SkaterStatsDetail { Place = i + 1, Nationality = player.Nationality, GamesPlayed = playerStat.GamesPlayed, Assists = playerStat.Assists, FullName = playerStat.SkaterFullName, Points = playerStat.Points, Goals = playerStat.Goals, PPG = playerStat.PointsPerGame, TeamAbbrevs = playerStat.TeamAbbrevs, Pos = playerStat.PositionCode, Age = Convert.ToDateTime(player.BirthDate).GetAge()};
            if(HasOption("rookie") && !player.Rookie) continue;
            if(!string.IsNullOrEmpty(name) && !player.FullName.ToLower().Contains(name)) continue;
            if(playerStat.GamesPlayed < atLeastGameCount) continue;

            if(nations.Count == 0) pointsTable.Add(pointTableItem);
            else if(nations.Count > 0 && nations.Any(n => string.Equals(player.Nationality, n, StringComparison.CurrentCultureIgnoreCase))) pointsTable.Add(pointTableItem);
        }
        if (!string.IsNullOrEmpty(team)) pointsTable = pointsTable.Where(p => p.TeamAbbrevs.ToLower().Contains(team)).ToList();
        ConsoleTableService.RenderTable(pointsTable, this);

        WriteNationsSummary(pointsTable);
        
        WriteSuccessLine($"Total count: {pointsTable.Count}");
        WriteSuccessLine($"Last updated: {season.Updated}");
    }

    public void ShowGoalies(int seasonId, List<string> nations, int take, string name, int atLeastGameCount, string team)
    {
        var season = DatabaseManager.SeasonsDb.GoalieStats.First(s => s.SeasonId == seasonId);
        var stats = season.Data.Take(take).ToArray();

        var detailsTable = new List<GoalieStatsDetail>();
        for (int i= 0; i < stats.Length; i++)
        {
            var playerStat = stats[i];
            var player = DatabaseManager.PlayersDb.People.FirstOrDefault(p => p.Id == playerStat.PlayerId) ?? new Player{FullName = "MISSING"};
            var pointTableItem = new GoalieStatsDetail { Place = i + 1, Nationality = player.Nationality, GamesPlayed = playerStat.GamesPlayed, Assists = playerStat.Assists, FullName = playerStat.GoalieFullName, Points = playerStat.Points, Goals = playerStat.Goals, TeamAbbrevs = playerStat.TeamAbbrevs, GoalsAgainst = playerStat.GoalsAgainst, Saves = playerStat.Saves, ShotsAgainst = playerStat.ShotsAgainst, SavePct = (decimal)(Math.Round(playerStat.SavePct.GetValueOrDefault(0), 4) * 100), GoalsAgainstAverage = (decimal)Math.Round(playerStat.GoalsAgainstAverage.GetValueOrDefault(0), 2) };
            if (HasOption("rookie") && !player.Rookie) continue;
            if (!string.IsNullOrEmpty(name) && !player.FullName.ToLower().Contains(name)) continue;
            if (playerStat.GamesPlayed < atLeastGameCount) continue;

            if (nations.Count == 0) detailsTable.Add(pointTableItem);
            else if (nations.Count > 0 && nations.Any(n => string.Equals(player.Nationality, n, StringComparison.CurrentCultureIgnoreCase))) detailsTable.Add(pointTableItem);
        }
        if (!string.IsNullOrEmpty(team)) detailsTable = detailsTable.Where(p => p.TeamAbbrevs.ToLower() == team).ToList();
        ConsoleTableService.RenderTable(detailsTable, this);

        WriteNationsSummary(detailsTable);
        
        WriteSuccessLine($"Total count: {detailsTable.Count}");
        WriteSuccessLine($"Last updated: {season.Updated}");
    }
}