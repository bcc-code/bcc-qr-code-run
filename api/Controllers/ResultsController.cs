using api.Data;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api")]
public class ResultsController : ControllerBase
{
    private readonly ResultsService _results;

    public ResultsController(ResultsService results)
    {
        _results = results;
    }

    [HttpGet("results")]
    public Task<List<ChurchResult?>> GetResults()
    {
        return _results.GetResultsAsync();
    }
    
    [HttpGet("results/mychurch")]
    [Authorize]
    public async Task<ActionResult<ChurchResult>> GetMyChurchResults()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        var teamId = int.Parse(User.Identity.Name!);
        return Ok(await _results.GetResultForChurchAsync(teamId));
        
    }
}

//public record ChurchResult(string ChurchName, int Teams, int Score, int SecretsFound, string TimeSpent);

//public record Result(string Church, int Points);