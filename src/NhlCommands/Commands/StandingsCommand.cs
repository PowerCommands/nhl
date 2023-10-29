using NhlCommands.DomainObjects;

namespace NhlCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Show season stats",
                         options: "stop",
                         example: "season")]
public class StandingsCommand : NhlBaseCommand
{
    public StandingsCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var start = Input.FirstArgumentToInt() == 0 ? GetCurrentSeason() : Input.FirstArgumentToInt();
        var lastUpdated = DateTime.MinValue;
        var teamRecords = new List<TeamRecord>();
        foreach (var standing in DatabaseManager.StandingsDb.Standings.Where(s => s.SeasonId == start))
        {
            foreach (var record in standing.Records)
            {
                lastUpdated = record.TeamRecords.First().LastUpdated;
                teamRecords.AddRange(record.TeamRecords);
            }
        }
        var standings = teamRecords.OrderByDescending(t => t.Points).ThenBy(t => t.LeagueRank).Select(record => new StandingsView { Points = record.Points, Team = record.Team.Name, Rank = record.LeagueRank }).ToList();
        ConsoleTableService.RenderTable(standings, this);
        WriteLine($"Last updated: {lastUpdated}");
        return Ok();
    }
}