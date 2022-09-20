using api.Data;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("Trivia")]
public class TriviaController : ControllerBase
{
    private readonly DataContext _context;

    public TriviaController(DataContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<FunFact[]>> GetFunFacts()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();
        
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.TeamName == User.Identity.Name!);

        if (team == null)
            return Unauthorized();

        var qrCodes = team.Posts.Select(x => x.Id).Concat(team.SecretsFound.Select(x => x.Id)).ToList();

        var funfacts = await _context.QrCodes.Where(x => qrCodes.Contains(x.Id)).Select(x=>x.FunFact).ToArrayAsync();
        
        return funfacts;
    }
}