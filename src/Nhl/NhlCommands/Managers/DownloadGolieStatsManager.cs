using NhlCommands.DomainObjects;
using System.Text.Json;

namespace NhlCommands.Managers;

public class DownloadGoalieStatsManager : DownloadBaseManager
{
    public DownloadGoalieStatsManager(DbManager dbManager, IConsoleWriter writer) : base(dbManager, writer) { }

    public async Task DownloadAsync()
    {
        var newNhlPlayerFound = false;
        var noOverwrite = false;
        int currentSeason = DateTime.Now.Month < 9 ? DateTime.Now.Year : DateTime.Now.Year + 1;

        foreach (var seasonStat in DBManager.SeasonsDb.SkaterStats.OrderBy(s => s.SeasonId))
        {
            var seasonId = seasonStat.SeasonId;
            Writer.WriteHeadLine($"Season {seasonId}");

            var exists = DBManager.SeasonsDb.GoalieStats.FirstOrDefault(g => g.SeasonId == seasonId);
            if (exists != null && seasonId != currentSeason)
            {
                if(noOverwrite) continue;
                noOverwrite = !DialogService.YesNoDialog("This season already exists, continue anyway?");
                if(noOverwrite) continue;
            }
            var seasonStats = await GetSeason(seasonId);
            if(seasonStats.Total > 0) UpdateGoalie(seasonStats, seasonId);
            var rank = 1;
        
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
                Writer.WriteLine($"{rank}. {player.GoalieFullName} {nationality} {player.Points} ({player.Goals}+{player.Assists}) {player.TeamAbbrevs}");
                rank++;
            }
            Writer.WriteSuccess($"\n{seasonStats.Data.Count} goalies fetched from nhl.com\n");
        }
        if (newNhlPlayerFound)
        {
            DBManager.PlayersDb.Updated = DateTime.Now;
            DBManager.Save(DBManager.PlayersDb);
            Writer.WriteSuccess("New players saved to file.\n");
        }
    }

    private async Task<Goalies> GetSeason(int seasonId)
    {
        var retVal = new Goalies { SeasonId = seasonId};
        var start = 1;
        var maxIterations = 15;
        
        for(int  i = 0; i < maxIterations; i++)
        {
            var moreSeasonGoalies = await GetSeasonData(seasonId, start);
            Writer.WriteLine($"Fetching goalies from rank {start} to {retVal.Data.Count + moreSeasonGoalies.Data.Count}...\n");
            retVal.Data.AddRange(moreSeasonGoalies.Data);
            retVal.Total = moreSeasonGoalies.Total;
            var fetchMorePlayers = moreSeasonGoalies.Data.Count == 100;
            if(!fetchMorePlayers)  break;
            start += 100;
            Thread.Sleep(1000);
        }
        return retVal;
    }

    private async Task<Goalies> GetSeasonData(int seasonId, int start)
    {
        try
        {
            var season = $"{seasonId - 1}{seasonId}";
            //var babar = $"https://api.nhle.com/stats/rest/en/goalie/summary?isAggregate=false&isGame=false&sort=[{{%22property%22:%22wins%22,%22direction%22:%22DESC%22}},{{22property%22:%22savePct%22,%22direction%22:%22DESC%22}},{{%22property%22:%22playerId%22,%22direction%22:%22ASC%22%}}]&start=0&limit=50&factCayenneExp=gamesPlayed%3E=1&cayenneExp=gameTypeId=2%20";
            var url = $"https://api.nhle.com/stats/rest/en/goalie/summary?isAggregate=false&isGame=false&sort=%5B%7B%22property%22:%22wins%22,%22direction%22:%22DESC%22%7D,%7B%22property%22:%22savePct%22,%22direction%22:%22DESC%22%7D,%7B%22property%22:%22playerId%22,%22direction%22:%22ASC%22%7D%5D&start={start - 1}&limit=100&factCayenneExp=gamesPlayed%3E=1&cayenneExp=gameTypeId=2%20and%20seasonId%3C={season}%20and%20seasonId%3E={season}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
            var stats = JsonSerializer.Deserialize<Goalies>(responseString, options) ?? new Goalies();
            return stats;
        }
        catch (Exception ex)
        {
            Writer.WriteFailure($"{ex.Message}\n");
            return new Goalies();
        }
    }
    private void UpdateGoalie(Goalies stats, int seasonId)
    {
        stats.SeasonId = seasonId;
        var existing = DBManager.SeasonsDb.GoalieStats.FirstOrDefault(s => s.SeasonId == stats.SeasonId);
        if (existing != null) DBManager.SeasonsDb.GoalieStats.Remove(existing);
        stats.Updated = DateTime.Now;
        DBManager.SeasonsDb.GoalieStats.Add(stats);
        DBManager.Save(DBManager.SeasonsDb);
        Writer.WriteSuccess($"Season {seasonId - 1}{seasonId} saved!\n");
    }
}