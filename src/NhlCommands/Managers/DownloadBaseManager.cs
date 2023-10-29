using System.Text.Json;
using NhlCommands.DomainObjects;
using NhlCommands.DomainObjects.Database;

namespace NhlCommands.Managers;

public class DownloadBaseManager
{
    protected readonly DbManager DBManager;
    protected readonly IConsoleWriter Writer;

    public DownloadBaseManager(DbManager dbManager, IConsoleWriter writer)
    {
        DBManager = dbManager;
        Writer = writer;
    }

    public async Task<Player> GetNhlPlayer(Prospect prospect)
    {
        try
        {
            var player = await GetNhlPlayer(prospect.NhlPlayerId);
            player.AmateurTeam = prospect.AmateurTeam;
            player.AmateurLeague = prospect.AmateurLeague;
            return player;
        }
        catch (Exception ex)
        {
            Writer.WriteFailure($"{ex.Message}\n");
            return new Player();
        }
    }
    protected async Task<Player> GetNhlPlayer(int playerId)
    {
        try
        {
            var url = $"https://statsapi.web.nhl.com/api/v1/people/{playerId}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
            var players = JsonSerializer.Deserialize<PlayersDb>(responseString, options) ?? new();
            var player = players.People.First();
            var prospect = DBManager.ProspectsDb.Prospects.FirstOrDefault(p => p.NhlPlayerId == playerId);
            
            if (prospect == null) return player;
            
            player.AmateurTeam = prospect.AmateurTeam;
            player.AmateurLeague = prospect.AmateurLeague;
            player.Drafted = true;
            return player;
        }
        catch (Exception ex)
        {
            Writer.WriteFailure($"{ex.Message}\n");
            return new Player();
        }
    }
}