using NhlCommands.DomainObjects;

namespace NhlCommands.Commands;

[PowerCommandTest(         tests: " wayne gretzky")]
[PowerCommandDesign( description: "Search player with filters",
                         options: "goalies|active|un-drafted",
                     suggestions: "SWE|FIN|CAN|USA|CZE|SVK|DEU|AUS|CHE|SVN|NOR|DNK|NLD|BLR|LVA|FRA|AUT|GBR|UKR|HRV|LTU|KAZ|POL|NGA|BHS|ITA|RUS",
                         example: "//Search \"wayne gretzky\"|player \"wayne gretzky\"|//Search swedish players|player SWE|//Search Canadian undrafted players named \"wayne\"|player \"wayne\" CAN --un-drafted")]
public class PlayerCommand : NhlBaseCommand
{
    public PlayerCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var playerDetails = new List<PlayerDetail>();
        var nations = GetNations();

        var nameSearch = Input.SingleQuote.ToLower();
        //var birthNation = GetOptionValue("birth-nation");
        //var birthYear = Input.OptionToInt("birth-year");
        var goalies = HasOption("goalies") ? "G" : "";
        var active = HasOption("active");
        var unDrafted = HasOption("drafted");

        var players = DatabaseManager.PlayersDb.People.Where(p => (string.IsNullOrEmpty(nameSearch) || p.FullName.ToLower().Contains(nameSearch)) && (string.IsNullOrEmpty(goalies) || p.PrimaryPosition.Code == goalies) && (!active || p.Active) && (!unDrafted || !p.Drafted));
        foreach (var player in players)
        {
            var detail = new PlayerDetail { FullName = player.FullName, PrimaryNumber = player.PrimaryNumber, BirthDate = player.BirthDate, Active = player.Active, Drafted = player.Drafted, Rookie = player.Rookie, PositionCode = player.PrimaryPosition.Code, BirthCity = player.BirthCity, BirthCountry = player.BirthCountry, Nationality = player.Nationality };
            if (nations.Count == 0) playerDetails.Add(detail);
            else if (nations.Count > 0 && nations.Any(n => string.Equals(player.Nationality, n, StringComparison.CurrentCultureIgnoreCase))) playerDetails.Add(detail);
        }

        ConsoleTableService.RenderTable(playerDetails, this);

        WriteNationsSummary(playerDetails);

        WriteCodeExample("Total:",$"{playerDetails.Count}");
        return Ok();
    }
}