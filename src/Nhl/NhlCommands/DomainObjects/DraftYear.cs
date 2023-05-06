namespace NhlCommands.DomainObjects;

public class DraftYear
{
    public int Year { get; set; }
    public string Copyright { get; set; }
    public List<Draft> Drafts { get; set; }
}