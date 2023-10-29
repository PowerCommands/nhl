using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects;

public class Prospect : INationality
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string BirthDate { get; set; } = string.Empty;
    public string BirthCity { get; set; } = string.Empty;
    public string BirthCountry { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string Height { get; set; } = string.Empty;
    public int Weight { get; set; }
    public string ShootsCatches { get; set; } = string.Empty;
    public PrimaryPosition PrimaryPosition { get; set; }
    public int NhlPlayerId { get; set; }
    public string DraftStatus { get; set; } = string.Empty;
    public ProspectCategory ProspectCategory { get; set; }
    public ProspectAmateurTeam AmateurTeam { get; set; }
    public ProspectAmateurLeague AmateurLeague { get; set; }
    public int Year { get; set; }
}