using NhlCommands.Contracts;
using NhlCommands.Managers;

namespace NhlCommands.Commands;

//https://github.com/erunion/sport-api-specifications/tree/master/nhl
public abstract class NhlBaseCommand : CommandBase<PowerCommandsConfiguration>
{
    public static readonly DbManager DatabaseManager = new DbManager(Path.Combine(ConfigurationGlobals.ApplicationDataFolder, "nhl"));
    protected NhlBaseCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }
    protected int GetSeasonId()
    {
        var seasonId = Input.FirstArgumentToInt();
        if(seasonId < 1900) seasonId = DateTime.Now.Month < 9 ? seasonId = DateTime.Now.Year : DateTime.Now.Year+1;   //The current season is the year that the season ends
        return seasonId;
    }
    protected int GetCurrentSeason() => DateTime.Now.Month < 9 ? DateTime.Now.Year : DateTime.Now.Year + 1;
    protected string GetSeasonForDisplay(int seasonId) => $"{seasonId - 1}/{seasonId.ToString().Substring(2,2)}";
    protected List<string> GetNations()
    {
        var knownNations = "SWE|FIN|CAN|USA|CZE|SVK|DEU|NOR|DNK|NLD|BLR|CHE|LVA|RUS".Split('|');
        return Input.Arguments.Select(argument => knownNations.FirstOrDefault(n => n == argument)).Where(nation => !string.IsNullOrEmpty(nation)).ToList()!;
    }
    protected void WriteNationsSummary<T>(List<T> nationalities) where T : INationality
    {
        var nations = GetNations();
        if (nations.Count <= 1) return;
        var nationsCount = (from nation in nations let count = nationalities.Count(p => !string.IsNullOrEmpty(p.Nationality) &&  p.Nationality.Equals(nation, StringComparison.CurrentCultureIgnoreCase)) select new NationCount { Count = count, Nation = nation }).ToList();
        ConsoleTableService.RenderTable(nationsCount, this);
    }
    private class NationCount { public string? Nation { get; init; } public int Count { get; init; }}
}