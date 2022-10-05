using System.Globalization;
using api.Data;
using Microsoft.EntityFrameworkCore;
using Orleans;

namespace api.Grains;

public interface IChurch : IGrainWithStringKey
{
    Task RegisterTeam(ITeam teamGrain);
    Task<ChurchResult?> GetResult();
    Task RemoveTeam(ITeam teamGrain);
}


public class ChurchGrain : Grain, IChurch
{
    private readonly IDbContextFactory<DataContext> _contextFactory;

    public ChurchGrain(IDbContextFactory<DataContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    public Church? ChurchData { get; set; }
    public List<ITeam> Teams { get; set; }
    public IResultsGrain? ResultsGrain { get; set; }


    public override async Task OnActivateAsync()
    {
        ResultsGrain = GrainFactory.GetGrain<IResultsGrain>("results");
        
        await using var context = await _contextFactory.CreateDbContextAsync();

        var name = this.GetPrimaryKeyString();
        
        ChurchData = await context.Churches.FirstOrDefaultAsync(x => x.Name == name);
        var teamIds = await context.Teams.Where(x => x.ChurchName == name).Select(x=>x.ChurchName+"-"+x.TeamName).ToListAsync();

        Teams = teamIds.Select(x => GrainFactory.GetGrain<ITeam>(x)).ToList();
    }

    public Task RegisterTeam(ITeam teamGrain)
    {
        Teams.Add(teamGrain);
        return Task.CompletedTask;
    }

    public async Task<ChurchResult?> GetResult()
    {
        if (ChurchData == null)
            return null;
        
        var tasks = Teams.Select(x => x.GetTeamResult()).ToArray();
        var results = await Task.WhenAll(tasks);
        
        var churchParticipants = ChurchData.Participants;
        var activeTeams = results.Where(r => r.Points > 0).ToArray();
        var activeParticipants = activeTeams.Sum(t => t.Members);
        var totalTimeSpent = activeTeams.Any() ? activeTeams.Select(t => t.TimeSpent).Aggregate((c, n) => c.Add(n)) : TimeSpan.Zero;
        var participation = churchParticipants <= 0 ? 0 : Math.Min((int)Math.Round((churchParticipants > 0 ? ((decimal)activeParticipants / churchParticipants) : 1) * 100), 100);
        var totalPoints = activeTeams.Sum(t => t.Points);
        var secretsFound = activeTeams.Sum(t => t.SecretsFound);
        var teamsCount = activeTeams.Count();
        var averagePoints = teamsCount > 0 ? Math.Round(((decimal)totalPoints / (decimal)teamsCount), 2) : 0;
        return new ChurchResult
        {
            Church = ChurchData.Name,
            Country = GetCountryName(ChurchData.CountryCode),
            Participants = activeParticipants,
            Participation = participation,
            Teams = teamsCount,
            Registrations = ChurchData.Participants,
            Points = totalPoints,
            SecretsFound = secretsFound,
            AveragePoints = averagePoints,
            Score = participation * averagePoints,
            TimeSpent = totalTimeSpent.ToString("hh\\:mm", CultureInfo.InvariantCulture)
        };
    }

    public Task RemoveTeam(ITeam teamGrain)
    {
        Teams.Remove(teamGrain);
        return Task.CompletedTask;
    }
    
    private string GetCountryName(string countryCode)
    {
        try
        {
            var region = new RegionInfo(countryCode);
            return region.Name;
        }
        catch
        {
            return countryCode;
        }
    }
}

public class ChurchResult
{
    public string Church { get; set; } = "";

    public string Country { get; set; } = "";

    public int Teams { get; set; }

    public string TimeSpent { get; set; } = "00:00";

    public int Registrations { get; set; }

    public int Participants { get; set; }

    public decimal Participation { get; set; }

    public int Points { get; set; }

    public decimal Score { get; set; }

    public decimal AveragePoints { get; set; }

    public int SecretsFound { get; set; }

    public static ChurchResult Empty => new ChurchResult
    {
        Church = "",
        Country = "",
    };


}