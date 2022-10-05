using api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace api.Controllers;

[ApiController]
[Route("api")]
public class ChurchController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IMemoryCache _cache;

    public ChurchController(DataContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet("churches")]
    public async Task<string[]> GetChurches()
    {
        return await _cache.GetOrCreateAsync("churches", e =>
        {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
            return _context.Churches.Select(x => x.Name).OrderBy(x => x).ToArrayAsync();
        }) ?? Array.Empty<string>();
    }
}