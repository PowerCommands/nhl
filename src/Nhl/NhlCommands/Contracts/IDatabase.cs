namespace NhlCommands.Contracts;

public interface IDatabase
{
    public DateTime Updated { get; set; }
    public List<string> GetDescriptions();
}