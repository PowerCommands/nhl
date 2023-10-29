using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects;

public class PlayerDetail : INationality
{
    public string FullName { get; set; } = string.Empty;
    public string PrimaryNumber { get; set; } = string.Empty;
    public string BirthDate { get; set; } = string.Empty;
    public string BirthCity { get; set; } = string.Empty;
    public string BirthCountry { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public bool Active { get; set; }
    public bool Drafted { get; set; }
    public bool Rookie { get; set; }
    public string PositionCode { get; set; } = string.Empty;
}