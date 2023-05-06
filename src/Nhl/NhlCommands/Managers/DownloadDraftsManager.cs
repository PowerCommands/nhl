using System.Text.Json;
using NhlCommands.DomainObjects;
using NhlCommands.DomainObjects.Database;

namespace NhlCommands.Managers;

public class DownloadDraftsManager : DownloadBaseManager
{
    public DownloadDraftsManager(DbManager dbManager, IConsoleWriter writer) : base(dbManager, writer) { }
    public async Task DownloadAsync(int startYear)
    {
        var draftYears = await GetDrafts(startYear);
        var hasChanges = false;
        var stopProspectYear = DateTime.Now.Year;
        foreach (var draftYear in from draftYear in draftYears let existing = DBManager.DraftsDb.DraftYears.FirstOrDefault(d => d.Year == draftYear.Year) where existing == null select draftYear)
        {
            stopProspectYear = draftYear.Year;
            hasChanges = true;
            DBManager.DraftsDb.DraftYears.Add(draftYear);
        }
        if (hasChanges)
        {
            DBManager.Save(DBManager.DraftsDb);
            Writer.WriteSuccess("DraftsDB updated\n");
        }
        //If new drafts has been added the prospects needs to be updated as well
        var updatedProspects = await GetUpdateProspects(stopProspectYear);
        if (updatedProspects.Count > 0)
        {
            DBManager.ProspectsDb.Prospects.AddRange(updatedProspects);
            DBManager.Save(DBManager.ProspectsDb);
            Writer.WriteSuccess("Updated prospects saved!\n");
        }
    }
    private async Task<List<DraftYear>> GetDrafts(int startYear)
    {
        var retVal = new List<DraftYear>();
        for (int year = startYear; year < DateTime.Now.Year; year++)
        {
            if(DBManager.DraftsDb.DraftYears.Any(y => y.Year == year)) continue;
            var draftYear = await GetDraft(year);
            draftYear.Year = year;
            retVal.Add(draftYear);
            Writer.WriteSuccess($"Downloaded {draftYear.Drafts.Count} for draft year {year} OK\n");
        }
        return retVal;
    }
    private async Task<DraftYear> GetDraft(int year)
    {
        try
        {
            var url = $"https://statsapi.web.nhl.com/api/v1/draft/{year}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
            var draftYear = JsonSerializer.Deserialize<DraftYear>(responseString, options) ?? new DraftYear();
            return draftYear;
        }
        catch (Exception ex)
        {
            Writer.WriteFailure($"{ex.Message}\n");
            return new DraftYear();
        }
    }
    #region prospects
    private async Task<List<Prospect>> GetUpdateProspects(int stopProspectYear)
    {
        var updatedProspects = new List<Prospect>();
        var counter = 0;
        foreach (var draftYears in DBManager.DraftsDb.DraftYears.Where(y => y.Year < stopProspectYear+1))
        {
            foreach (var draft in draftYears.Drafts)
            {
                foreach (var round in draft.Rounds)
                {
                    foreach (var draftPick in round.Picks)
                    {
                        counter++;
                        if (DBManager.ProspectsDb.Prospects.Any(p => p.Id == draftPick.Prospect.Id &&  p.Id > 0))
                        {
                            Writer.WriteLine("Already exist, skipping...");
                            continue;
                        }
                        var prospect = await GetProspect(draftPick.Prospect.Id);
                        if (prospect.Id > 0)
                        {
                            updatedProspects.Add(prospect);
                            Writer.WriteSuccess($"{counter} New prospect {prospect.FullName} added {draft.DraftYear} OK\n");
                        }
                        else
                        {
                            var player = DBManager.PlayersDb.People.FirstOrDefault(p => p.FullName == draftPick.Prospect.FullName);
                            if (player != null)
                            {
                                var id = DBManager.ProspectsDb.Prospects.Min(p => p.Id) - 1;
                                Writer.WriteLine($"Found player {player.FullName} by full name!");
                                prospect.FirstName = player.FirstName;
                                prospect.LastName = player.LastName;
                                prospect.Nationality = player.Nationality;
                                prospect.BirthCity = player.BirthCity;
                                prospect.BirthCountry = player.BirthCountry;
                                prospect.AmateurLeague = player.AmateurLeague;
                                prospect.AmateurTeam = player.AmateurTeam;
                                prospect.BirthDate = player.BirthDate;
                                prospect.FullName = player.FullName;
                                prospect.Id = id;
                                prospect.NhlPlayerId = player.Id;
                                updatedProspects.Add(prospect);
                                Writer.WriteSuccess($"{counter} New prospect {prospect.FullName} added OK\n");
                            }
                        }
                    }
                }
            }
        }
        return updatedProspects;
    }
    private async Task<Prospect> GetProspect(int prospectId)
    {
        try
        {
            var url = $"https://statsapi.web.nhl.com/api/v1/draft/prospects/{prospectId}";

            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, };
            var prospects = JsonSerializer.Deserialize<ProspectsDb>(responseString, options) ?? new ();
            var prospect = prospects.Prospects.FirstOrDefault();
            if (prospect == null) return new Prospect();
            
            var player = await GetNhlPlayer(prospect);
            prospect.Nationality = player.Nationality;
            return prospect;
        }
        catch (Exception ex)
        {
            Writer.WriteFailure($"{ex.Message}\n");
            return new Prospect();
        }
    }
    #endregion

}