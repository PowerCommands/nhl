using System.Text.Json;
using NhlCommands.DomainObjects;

namespace NhlCommands.Managers;

public class DownloadStandingsManager : DownloadBaseManager
{
    public DownloadStandingsManager(DbManager dbManager, IConsoleWriter writer) : base(dbManager, writer) { }

    public async Task DownloadAsync(int seasonId)
    {
        var standing = await GetStanding(seasonId);
        var existing = DBManager.StandingsDb.Standings.FirstOrDefault(s => s.SeasonId == seasonId);
        if (existing != null) DBManager.StandingsDb.Standings.Remove(existing);
        standing.SeasonId = seasonId;
        standing.Updated = DateTime.Now;
        DBManager.StandingsDb.Standings.Add(standing);
        DBManager.StandingsDb.Updated = DateTime.Now;
        DBManager.Save(DBManager.StandingsDb);
        Writer.WriteSuccess($"Season {seasonId} standings downloaded and saved to DB OK!\n");
    }
    private async Task<Standing> GetStanding(int seasonId)
    {
        try
        {
            var season = $"{seasonId - 1}{seasonId}";
            var url = $"https://statsapi.web.nhl.com/api/v1/standings?season={season}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
            var stats = JsonSerializer.Deserialize<Standing>(responseString, options) ?? new Standing();
            return stats;
        }
        catch (Exception ex)
        {
            Writer.WriteFailure($"{ex.Message}\n");
            return new Standing();
        }
    }
}