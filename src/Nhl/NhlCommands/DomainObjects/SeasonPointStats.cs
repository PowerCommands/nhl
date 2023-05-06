namespace NhlCommands.DomainObjects;

public class SeasonPointStats
{
    public string Season { get; set; } = string.Empty;
    public string Winner { get; set; } = string.Empty;
    public string Nation { get; set; } = string.Empty;
    public int WinnerPoint { get; set; }
    public float? PointsPerGame { get; set; }
    public int Over99 { get; set; }
    public int Games { get; set; }
    public string Status { get; set; } = string.Empty;
}