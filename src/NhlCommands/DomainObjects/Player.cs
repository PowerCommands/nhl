using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects;

public class Player : INationality
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Link { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PrimaryNumber { get; set; }
    public string BirthDate { get; set; }
    public string BirthCity { get; set; }
    public string BirthCountry { get; set; }
    public string Nationality { get; set; }
    public string Height { get; set; }
    public int Weight { get; set; }
    public bool Active { get; set; }
    public bool Drafted { get; set; }
    public bool Rookie { get; set; }
    public string ShootsCatches { get; set; }
    public string RosterStatus { get; set; }
    public PrimaryPosition PrimaryPosition { get; set; }
    public ProspectAmateurTeam AmateurTeam { get; set; }
    public ProspectAmateurLeague AmateurLeague { get; set; }
}