namespace NhlCommands.Managers;

public class IntegrityCheckManager : DownloadBaseManager
{
    public IntegrityCheckManager(DbManager dbManager, IConsoleWriter writer) : base(dbManager, writer) { }

    public async Task DownloadAsync(int seasonId)
    {
        var newNhlPlayerFound = false;
        foreach (var seasonStat in DBManager.SeasonsDb.SkaterStats)
        {
            Writer.WriteHeadLine($"Checking season [{seasonStat.SeasonId}]");
            foreach (var player in seasonStat.Data)
            {
                var existing = DBManager.PlayersDb.People.FirstOrDefault(p => p.Id == player.PlayerId);
                if (existing == null)
                {
                    var nhlPlayer = await GetNhlPlayer(player.PlayerId);
                    if (nhlPlayer.Id > 0)
                    {
                        DBManager.PlayersDb.People.Add(nhlPlayer);
                        newNhlPlayerFound = true;
                        Writer.WriteSuccess($"{nhlPlayer.FullName} found!\n");
                    }
                }
            }
        }
        if (newNhlPlayerFound)
        {
            DBManager.PlayersDb.Updated = DateTime.Now;
            DBManager.Save(DBManager.PlayersDb);
            Writer.WriteSuccess("New players saved to file.\n");
        }
    }
}