using api.Controllers;
using api.Data;
using api.Repositories;
using Microsoft.EntityFrameworkCore;
using Orleans;

namespace api.Grains;

public interface ITeam : IGrainWithStringKey
{
    Task Register(RegisterTeamRequest request);

    Task<QrCodeResult?> RegisterPost(QrCodeData qrCodeData);
    Task<bool> IsActive();
    Task<Team?> GetTeamData();
    Task Clear();
    Task<IEnumerable<FunFact>> GetFunFacts();
    Task<TeamResult> GetTeamResult();
}

public class TeamGrain : Grain, ITeam
{
    private readonly IDbContextFactory<DataContext> _dbContextFactory;

    public TeamGrain(IDbContextFactory<DataContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    
    public IChurch Church { get; set; }
    
    /// <inheritdoc />
    public override async Task OnActivateAsync()
    {

        var context = await _dbContextFactory.CreateDbContextAsync();

        var team = await context.Teams.FirstOrDefaultAsync(x => x.ChurchName + "-" + x.TeamName == this.GetPrimaryKeyString());
        if (team == null) return;
        TeamData = team;

        Church = GrainFactory.GetGrain<IChurch>(team.ChurchName);

        _ = Church.RegisterTeam(this);
    }

    public Team? TeamData { get; set; }

    /// <inheritdoc />
    public async Task Register(RegisterTeamRequest request)
    {
        if (TeamData != null)
            throw new BadHttpRequestException("Team already exists");

        TeamData = new Team()
        {
            TeamName = request.TeamName,
            ChurchName = request.ChurchName,
        };

        await using (var context = await _dbContextFactory.CreateDbContextAsync())
        {
            context.Teams.Add(TeamData);
            await context.SaveChangesAsync();
        }

        Church = GrainFactory.GetGrain<IChurch>(TeamData.ChurchName);

        _ = Church.RegisterTeam(this);
    }

    

    /// <inheritdoc />
    public async Task<QrCodeResult?> RegisterPost(QrCodeData qrCodeData)
    {
        if (ScannedQrCodes.Any(x => x.QrCodeId == qrCodeData.QrCodeId))
            return null; // early return to avoid creating dbcontext
        
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        var qrCode = await context.QrCodes.FirstOrDefaultAsync(x => x.QrCodeId == qrCodeData.QrCodeId);
        if (qrCode == null)
            return null;
        
        if (TeamData.QrCodesScanned.Any(x=>x.Id == qrCode.GroupId))
            return null;

        context.Attach(TeamData);
        
        TeamData.QrCodesScanned.Add(new Score(qrCode.GroupId, qrCode.Points, qrCode.IsSecret));
        TeamData.FirstScannedQrCode ??= DateTime.UtcNow;
        TeamData.LastScannedQrCode = DateTime.UtcNow;

        await context.SaveChangesAsync();

        ScannedQrCodes.Add(qrCode);

        return new QrCodeResult()
        {
            Points = qrCode.Points,
            Team = TeamData,
            FunFact = qrCode.FunFact
        };
    }

    public List<QrCode> ScannedQrCodes { get; } = new();

    public Task<bool> IsActive() => Task.FromResult(TeamData != null);

    public Task<Team?> GetTeamData() => Task.FromResult(TeamData);

    public async Task Clear()
    {
        if (TeamData != null)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            context.Teams.Remove(TeamData);
            await context.SaveChangesAsync();
            _ = Church.RemoveTeam(this);
        }
        DeactivateOnIdle();
    }

    public Task<IEnumerable<FunFact>> GetFunFacts() => Task.FromResult<IEnumerable<FunFact>>(ScannedQrCodes.Select(x => x.FunFact).ToList());

    /// <inheritdoc />
    public Task<TeamResult> GetTeamResult()
    {
        if (TeamData != null)
        {
            return Task.FromResult(new TeamResult
            {
                Points = TeamData.QrCodesScanned.Sum(q => q.Points),
                Posts = TeamData.QrCodesScanned.Count(q => !q.IsSecret),
                SecretsFound = TeamData.QrCodesScanned.Count(q => q.IsSecret),
                TeamName = TeamData.TeamName,
                TimeSpent = TeamData.TimeSpent.GetValueOrDefault(),
                Members = TeamData.Members
            });
        }
        return Task.FromResult(TeamResult.Empty);
    }
}

public class TeamResult
{
    public string TeamName { get; set; } = "";

    public int Posts { get; set; }

    public int SecretsFound { get; set; }

    public TimeSpan TimeSpent { get; set; }

    public int Points { get; set; }

    public int Members { get; set; }

    public static TeamResult Empty => new()
    {
        Points = 0,
        Posts = 0,
        SecretsFound = 0,
        TeamName = "",
        TimeSpent = TimeSpan.Zero

    };
    
}