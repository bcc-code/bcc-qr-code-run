using api.Data;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace api.Controllers;

[ApiController]
[Route("api")]
public class ChurchController : ControllerBase
{
    private readonly DataContext _context;
    private readonly CacheService _cache;

    public ChurchController(DataContext context, CacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet("churches")]
    public async Task<string[]> GetChurches()
    {
        
        return await _cache.GetOrCreateAsync("churches", TimeSpan.FromMinutes(5), () => _context.Churches.Select(x=>x.Name).ToArrayAsync()) ?? new string[0];
    }
}