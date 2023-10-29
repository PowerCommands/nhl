using NhlCommands.Contracts;

namespace NhlCommands.Commands;

[PowerCommandTest(         tests: " ")]
[PowerCommandDesign( description: "Shows metadata about the database",
                         example: "db")]
public class DbCommand : NhlBaseCommand
{
    public DbCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        WriteMetaData(DatabaseManager.PlayersDb, "Players");
        WriteLine("--------------------------------------------------------------------------------------------------------------------------");
        WriteLine("");
        WriteMetaData(DatabaseManager.SeasonsDb, "Season individual players statistic");
        WriteSeasonDetails();
        WriteLine("--------------------------------------------------------------------------------------------------------------------------");
        WriteLine("");
        WriteMetaData(DatabaseManager.StandingsDb, "Season team standings statistic");
        WriteLine("--------------------------------------------------------------------------------------------------------------------------");
        WriteLine("");
        WriteMetaData(DatabaseManager.DraftsDb, "Drafts");
        WriteDraftDetails();
        WriteLine("--------------------------------------------------------------------------------------------------------------------------");
        WriteLine("");
        WriteMetaData(DatabaseManager.ProspectsDb, "Prospects");
        return Ok();
    }

    private void WriteMetaData<T>(T database, string databaseName) where T : IDatabase
    {
        WriteHeadLine(databaseName);
        foreach (var description in database.GetDescriptions()) WriteLine(description);
        WriteHeadLine($"Last updated:{database.Updated}");
        WriteCodeExample("File size:", DatabaseManager.GetFileSize(database.GetType()));
    }

    private void WriteDraftDetails()
    {
        var draftCounts = new List<YearCount>();
        foreach (var draftYear in DatabaseManager.DraftsDb.DraftYears.OrderByDescending(y => y.Year))
        {
            var yearCount = 0;
            foreach (var draft in draftYear.Drafts)
            {
                foreach (var round in draft.Rounds)
                {
                    yearCount += round.Picks.Length;
                }
            }
            draftCounts.Add(new YearCount{Year = draftYear.Year, Count = yearCount});
        }
        WriteCodeExample("Drafts", $"{draftCounts.Sum(d => d.Count)}");
    }

    private void WriteSeasonDetails()
    {
        var seasonCount = DatabaseManager.SeasonsDb.SkaterStats.SelectMany(s => s.Data).Count();
        WriteCodeExample("Skaters", $"{seasonCount}");

        WriteHeadLine("Goalies");
        seasonCount = DatabaseManager.SeasonsDb.GoalieStats.SelectMany(s => s.Data).Count();
        WriteCodeExample("Goalies", $"{seasonCount}");
    }

    class YearCount{ public int Year { get; set; } public int Count { get; set; }}
}