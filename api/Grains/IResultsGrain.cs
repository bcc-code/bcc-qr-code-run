using api.Data;
using Microsoft.EntityFrameworkCore;
using Orleans;

namespace api.Grains;

public interface IResultsGrain : IGrainWithStringKey
{
    Task<ChurchResult[]> GetResults();
    Task RegisterTeam(ITeam team);
}

public class ResultsGrain : Grain, IResultsGrain
{
    private readonly IDbContextFactory<DataContext> _contextFactory;

    public ResultsGrain(IDbContextFactory<DataContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    /// <inheritdoc />
    public override async Task OnActivateAsync()
    {
        var context = await _contextFactory.CreateDbContextAsync();
        var churchNames = await context.Churches.Select(x => x.Name).ToListAsync();
        Churches = churchNames.Select(x => GrainFactory.GetGrain<IChurch>(x)).ToList();
    }

    public List<IChurch> Churches { get; set; }

    public async Task<ChurchResult[]> GetResults()
    {
        var tasks = Churches.Select(x=>x.GetResult()).ToArray();
        var results = await Task.WhenAll(tasks);
        return results.Where(x=>x is not null).OrderByDescending(x=>x!.Score).ToArray()!;
    }

    public Task RegisterTeam(ITeam team)
    {
        throw new NotImplementedException();
    }
}