using NhlCommands.DomainObjects;

namespace NhlCommands.Commands;

[PowerCommandDesign(description: "Fetch draft data from NHL api to build up your base data or just display drafts from the local database file.",
                         options: "take|include-all|delete",
                         suggestions: "SWE|FIN|CAN|USA|CZE|SVK|DEU|AUS|CHE|SVN|NOR|DNK|NLD|BLR|LVA|FRA|AUT|GBR|UKR|HRV|LTU|KAZ|POL|NGA|BHS|ITA|RUS",
                         example: "//Show draft for season 2010/2011|draft 2010|//Include skaters that for some reason missing in the database (probably never made it to the NHL?)|draft 2010 --include-all|Delete a draft year (in case you want to download it again)|draft --delete 1980")]
public class DraftCommand : NhlBaseCommand
{
    public DraftCommand(string identifier, PowerCommandsConfiguration configuration) : base(identifier, configuration) { }

    public override RunResult Run()
    {
        var draftsCount = 0;
        var year = GetSeasonId();
        var take = Input.OptionToInt("take", 100000);

        if (HasOption("delete"))
        {
            Delete(year);
            return Ok();
        }

        var picks = new List<DraftPick>();
        foreach (var draftYear in DatabaseManager.DraftsDb.DraftYears.Where(d => d.Year == year || year == 0))
        {
            foreach (var draft in draftYear.Drafts)
            {
                foreach (var round in draft.Rounds)
                {
                    foreach (var draftPick in round.Picks)
                    {
                        picks.Add(draftPick);
                    }
                }
            }
        }

        var nations = GetNations();
        var drafts = new List<DraftView>();
        foreach (var draftPick in picks.Take(take))
        {
            var round = Convert.ToInt16(draftPick.Round);
            var draftView = new DraftView{Round = round, PickOverall = draftPick.PickOverall, Year = draftPick.Year, FullName = draftPick.Prospect.FullName, Nationality = "?"};
            var prospect = DatabaseManager.ProspectsDb.Prospects.FirstOrDefault(p => p.Id == draftPick.Prospect.Id) ?? new Prospect { BirthCity = "?", BirthCountry = "?", AmateurLeague = new ProspectAmateurLeague { Name = "?" }, AmateurTeam = new ProspectAmateurTeam { Name = "?" } };
            if (draftPick.Prospect.Id == 0)
            {
                prospect = DatabaseManager.ProspectsDb.Prospects.FirstOrDefault(p => p.Year == draftPick.Year && p.FullName == draftPick.Prospect.FullName);
            }

            if (prospect == null)
            {
                var people = DatabaseManager.PlayersDb.People.FirstOrDefault(p => p.FullName == draftPick.Prospect.FullName);
                if (people != null) prospect = new Prospect { AmateurLeague = people.AmateurLeague, Year = draftPick.Year, AmateurTeam = people.AmateurTeam, BirthCity = people.BirthCity, BirthCountry = people.BirthCountry, BirthDate = people.BirthDate, FullName = people.FullName, Nationality = people.Nationality, NhlPlayerId = people.Id };
            }
            if (prospect == null)
            {
                if (HasOption("include-all")) drafts.Add(draftView);
                continue;
            }

            draftView.AmateurTeam = $"{prospect.AmateurLeague?.Name}";
            draftView.BirthCity = $"{prospect.BirthCity}";
            draftView.BirthCountry = $"{prospect.BirthCountry}";
            draftView.Nationality = $"{prospect.Nationality}";

            if (nations.Count == 0)
            {
                draftsCount++;
                drafts.Add(draftView);
                continue;
            }
            if (nations.Count > 0 && nations.Any(n => string.Equals(prospect.Nationality, n, StringComparison.CurrentCultureIgnoreCase)))
            {
                draftsCount++;
                drafts.Add(draftView);
            }
        }
        ConsoleTableService.RenderTable(drafts.OrderBy(d => d.Round).ThenBy(d => d.PickOverall), this);
        WriteLine($"Total drafts count:{draftsCount}\n");
        WriteNationsSummary(drafts);
        return Ok();
    }

    private void Delete(int year)
    {
        var existing = DatabaseManager.DraftsDb.DraftYears.FirstOrDefault(y => y.Year == year);
        if (existing != null)
        {
            var confirm = DialogService.YesNoDialog($"Do you want to remove the draft year [{year}]?");
            if (confirm)
            {
                DatabaseManager.DraftsDb.DraftYears.Remove(existing);
                DatabaseManager.Save(DatabaseManager.DraftsDb);
                WriteSuccessLine($"Draft year [{year}] removed!");
            }
        }
    }
}