using NhlCommands.Contracts;
using NhlCommands.DomainObjects.Database;

namespace NhlCommands.Managers;
public class DbManager
{
    private readonly string _path;
    public PlayersDb PlayersDb;
    public SeasonsDb SeasonsDb;
    public DraftsDb DraftsDb;
    public ProspectsDb ProspectsDb;
    public StandingsDb StandingsDb;
    public DbManager(string path)
    {
        _path = path;
        if(!Directory.Exists(_path)) Directory.CreateDirectory(_path);
        PlayersDb = GetDatabase<PlayersDb>();
        SeasonsDb = GetDatabase<SeasonsDb>();
        DraftsDb = GetDatabase<DraftsDb>();
        ProspectsDb = GetDatabase<ProspectsDb>();
        StandingsDb = GetDatabase<StandingsDb>();
    }
    public void Save<T>(T database) where T : class, IDatabase, new()
    {
        var fileName = $"{typeof(T).Name.Replace("Db","").ToLower()}.json";
        database.Updated = DateTime.Now;
        StorageService<T>.Service.StoreObject(database, Path.Combine(_path, fileName));
    }
    private T GetDatabase<T>() where T : class, IDatabase, new()
    {
        var fileName = Path.Combine(_path, $"{typeof(T).Name.Replace("Db","").ToLower()}.json");
        return StorageService<T>.Service.GetObject(fileName);
    }
    public string GetFileSize(Type database)
    {
        var fileName = Path.Combine(_path, $"{database.Name.Replace("Db","").ToLower()}.json");
        if (!File.Exists(fileName)) return $"{fileName} does not exist.";
        var fileInfo = new FileInfo(fileName);
        var megaBytes = (fileInfo.Length / 1024) / 1024;
        return $"{megaBytes} MB";
    }
}