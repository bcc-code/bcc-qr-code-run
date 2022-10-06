using api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Orleans;

namespace api.Grains;

public interface IResultsGrain : IGrainWithStringKey
{
    Task<ChurchResult[]> GetResults();
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

    public ChurchResult[]? CachedResults = null;
    
    private DateTime _cacheExpireTime = DateTime.Now;

    public async Task<ChurchResult[]> GetResults()
    {
        if (_cacheExpireTime > DateTime.Now && CachedResults != null)
        {
            return CachedResults;
        }
        
        var tasks = Churches.Select(x=>x.GetResult()).ToArray();
        var results = await Task.WhenAll(tasks);
        _cacheExpireTime = DateTime.Now.AddSeconds(20);
        return CachedResults = results.Where(x=>x is not null).OrderByDescending(x=>x!.Score).ToArray()!;
    }
}