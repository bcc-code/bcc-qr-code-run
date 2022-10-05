using api.Grains;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace api.Controllers;

[ApiController]
[Route("api")]
public class TriviaController : ControllerBase
{
    private readonly IGrainFactory _factory;

    public TriviaController(IGrainFactory factory)
    {
        _factory = factory;
    }

    [Authorize]
    [HttpGet("trivia")]
    public async Task<ActionResult<FunFact[]>> GetFunFacts()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        var team = _factory.GetGrain<ITeam>(User.Identity.Name);
        var funFacts = await team.GetFunFacts();
        return funFacts.ToArray();
    }
}