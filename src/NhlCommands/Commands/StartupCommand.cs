using NhlCommands.Managers;

namespace NhlCommands.Commands;

[PowerCommandDesign( description: "To be run as a scheduled task",
                         example: "startup")]
public class StartupCommand : NhlBaseCommand
{
    public StartupCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        Task.Run(() => DownloadSkaterStats()).Wait();
        return Quit();
    }

    public async Task DownloadSkaterStats()
    {
        var seasonId = GetSeasonId();
        var downloadManager = new DownloadSkaterStatsManager(DatabaseManager, this);
        await downloadManager.DownloadAsync(seasonId);
    }
}