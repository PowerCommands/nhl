using System.Text.Json;
using NhlCommands.DomainObjects;

namespace NhlCommands.Managers;

public class DownloadSkaterStatsManager : DownloadBaseManager
{
    public DownloadSkaterStatsManager(DbManager dbManager, IConsoleWriter writer) : base(dbManager, writer) { }

    public async Task DownloadAsync(int seasonId)
    {
        var seasonStats = await GetSeason(seasonId);
        if(seasonStats.Total > 0) UpdateSkater(seasonStats, seasonId);
        var rank = 1;
        var newNhlPlayerFound = false;
        foreach (var player in seasonStats.Data)
        {
            var nationality = "";
            var existing = DBManager.PlayersDb.People.FirstOrDefault(p => p.Id == player.PlayerId);
            if (existing == null)
            {
                var nhlPlayer = await GetNhlPlayer(player.PlayerId);
                if (nhlPlayer.Id > 0)
                {
                    nationality = $"{nhlPlayer.Nationality}";
                    DBManager.PlayersDb.People.Add(nhlPlayer);
                    newNhlPlayerFound = true;
                }
            }
            else nationality = existing.Nationality;
            
            Writer.WriteLine($"{rank}. {player.SkaterFullName} {nationality} {player.Points} ({player.Goals}+{player.Assists}) {player.TeamAbbrevs}");
            rank++;
        }
        Writer.WriteSuccess($"\n{seasonStats.Data.Count} skaters fetched from nhl.com\n");
        if (newNhlPlayerFound)
        {
            DBManager.PlayersDb.Updated = DateTime.Now;
            DBManager.Save(DBManager.PlayersDb);
            Writer.WriteSuccess("New players saved to file.\n");
        }
    }

    private async Task<Skaters> GetSeason(int seasonId)
    {
        var retVal = new Skaters { SeasonId = seasonId};
        var start = 1;
        var maxIterations = 15;
        
        for(int  i = 0; i < maxIterations; i++)
        {
            var moreSeasonPlayers = await GetSeasonData(seasonId, start);
            Writer.WriteLine($"Fetching skaters from rank {start} to {retVal.Data.Count + moreSeasonPlayers.Data.Count}...\n");
            retVal.Data.AddRange(moreSeasonPlayers.Data);
            retVal.Total = moreSeasonPlayers.Total;
            var fetchMorePlayers = moreSeasonPlayers.Data.Count == 100;
            if(!fetchMorePlayers)  break;
            start += 100;
            Thread.Sleep(1000);
        }
        return retVal;
    }

    private async Task<Skaters> GetSeasonData(int seasonId, int start)
    {
        try
        {
            var season = $"{seasonId - 1}{seasonId}";
            var url = $"https://api.nhle.com/stats/rest/en/skater/summary?isAggregate=false&isGame=false&sort=[{{%22property%22:%22points%22,%22direction%22:%22DESC%22}},{{%22property%22:%22goals%22,%22direction%22:%22DESC%22}},{{%22property%22:%22assists%22,%22direction%22:%22DESC%22}},{{%22property%22:%22playerId%22,%22direction%22:%22ASC%22}}]&start={start - 1}&limit=100&factCayenneExp=gamesPlayed%3E=1&cayenneExp=gameTypeId=2%20and%20seasonId%3C={season}%20and%20seasonId%3E={season}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
            var stats = JsonSerializer.Deserialize<Skaters>(responseString, options) ?? new Skaters();
            return stats;
        }
        catch (Exception ex)
        {
            Writer.WriteFailure($"{ex.Message}\n");
            return new Skaters();
        }
    }
    private void UpdateSkater(Skaters season, int seasonId)
    {
        season.SeasonId = seasonId;
        var existing = DBManager.SeasonsDb.SkaterStats.FirstOrDefault(s => s.SeasonId == season.SeasonId);
        if (existing != null) DBManager.SeasonsDb.SkaterStats.Remove(existing);
        season.Updated = DateTime.Now;
        DBManager.SeasonsDb.SkaterStats.Add(season);
        DBManager.Save(DBManager.SeasonsDb);
        Writer.WriteSuccess($"Season {seasonId - 1}{seasonId} saved!\n");
    }
}