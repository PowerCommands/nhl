using NhlCommands.Managers;

namespace NhlCommands.Commands;

[PowerCommandDesign( description: "Download data from nhl.com, skaters stats is default and does not explicit have to been set by option.",
                         options: "goalies|standings|drafts|find-missing-players",
                         useAsync: true,
                         example: "//Download skater statistic for current season|download|//Download skater statistic for season 2000|download 2000|//Download goalies statistic for every skater seasons previously downloaded.|download --goalies|//Look for players that are missing and download them.|download --find-missing-players|//Download drafts (and prospects) from 2010 until an already existing year.|download 2010 --drafts|//Download NHL team standings from the min skaters year downloaded to current season|download --standings ")]
public class DownloadCommand : NhlBaseCommand
{
    public DownloadCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    public override async Task<RunResult> RunAsync()
    {
        if (HasOption("drafts")) await DownloadDrafts();
        else if (HasOption("goalies")) await DownloadGoaliesStats();
        else if (HasOption("standings")) await DownloadStandings();
        else if (HasOption("find-missing-players")) await DownloadMissingPlayers();
        else await DownloadSkaterStats();
        Write(ConfigurationGlobals.Prompt);
        return Ok();
    }
    public async Task DownloadDrafts()
    {
        var startYear = GetSeasonId();
        var downloadManager = new DownloadDraftsManager(DatabaseManager, this);
        await downloadManager.DownloadAsync(startYear);
    }
    public async Task DownloadSkaterStats()
    {
        var seasonId = GetSeasonId();
        var downloadManager = new DownloadSkaterStatsManager(DatabaseManager, this);
        await downloadManager.DownloadAsync(seasonId);
    }

    public async Task DownloadGoaliesStats()
    {
        var downloadManager = new DownloadGoalieStatsManager(DatabaseManager, this);
        await downloadManager.DownloadAsync();
    }

    public async Task DownloadMissingPlayers()
    {
        var downloadManager = new IntegrityCheckManager(DatabaseManager, this);
        await downloadManager.DownloadAsync(0);
    }

    public async Task DownloadStandings()
    {
        var minSavedSeason = Input.FirstArgumentToInt();
        
        var startSeason = minSavedSeason == 0 ? DatabaseManager.SeasonsDb.SkaterStats.Min(s => s.SeasonId) : minSavedSeason;
        var stopSeason = GetSeasonId();
        for (int seasonId = startSeason; seasonId < stopSeason+1; seasonId++)
        {
            var downloadManager = new DownloadStandingsManager(DatabaseManager, this);
            await downloadManager.DownloadAsync(seasonId);
        }
    }
}