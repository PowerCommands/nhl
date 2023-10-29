using NhlCommands.Contracts;

namespace NhlCommands.DomainObjects;

public class SkaterStatsDetail : INationality
{
    private float? _ppg;
    private float? _gpg;
    public int Place { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public string TeamAbbrevs { get; set; } = string.Empty;
    public string Nationality { get; set; }
    public int GamesPlayed { get; set; }
    public int Points { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }

    public float? PPG
    {
        get => _ppg.HasValue ? (float?)Math.Round(_ppg.Value, 2) : 0;
        set => _ppg = value;
    }
    public decimal GPG => Goals > 0 && GamesPlayed > 0 ? Math.Round((decimal)((decimal)Goals/GamesPlayed), 2) : 0;
    public string Pos { get; set; } = string.Empty;

}