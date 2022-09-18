using api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
public class ResultsController : ControllerBase
{
    private readonly DataContext _context;

    public ResultsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet("results")]
    public async Task<Result[]> GetResults()
    {
        return await _context.Teams.Select(x => new { x.ChurchName, x.Score }).GroupBy(x => x.ChurchName)
            .Select(x => new Result(x.Key, x.Sum(y => y.Score))).ToArrayAsync();
    }
    
    [HttpGet("results/mychurch")]
    [Authorize]
    public async Task<ActionResult<ChurchResult>> GetMyChurchResults()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();
        
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.TeamName == User.Identity.Name!);

        if (team == null)
            return Unauthorized();

        var teams = await _context.Teams.Where(x => x.ChurchName == team.ChurchName).Select(x=> new {x.Score, SecretsFound = x.SecretsFound.Count, x.TimeSpent}).ToListAsync();
        var score = teams.Sum(x => x.Score);
        var secrets = teams.Sum(x => x.SecretsFound);
        var timeSpent = teams.Select(x=>x.TimeSpent).Where(x=>x.HasValue).Select(x=>x!.Value).Aggregate((a, b) => a.Add(b));

        return new ChurchResult(team.ChurchName, teams.Count, score, secrets, timeSpent.ToString("HH:mm"));
    }
}

public record ChurchResult(string ChurchName, int teams, int Score, int SecretsFound, string TimeSpent);

public record Result(string Church, int Points);