using api.Grains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace api.Controllers;

[ApiController]
[Route("api")]
public class ResultsController : ControllerBase
{
    private readonly IGrainFactory _factory;

    public ResultsController(IGrainFactory factory)
    {
        _factory = factory;
    }

    [HttpGet("results")]
    public async Task<ChurchResult[]> GetResults()
    {
        var resultGrain =  _factory.GetGrain<IResultsGrain>("results");
        return await resultGrain.GetResults();
    }
    
    [HttpGet("results/mychurch")]
    [Authorize]
    public async Task<ActionResult<ChurchResult?>> GetMyChurchResults()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        var church = _factory.GetGrain<IChurch>(User.Identity.Name?.Split("-")[0] ?? "nullGrain");
        return await church.GetResult();
    }
}