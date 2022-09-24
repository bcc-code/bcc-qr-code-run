using api.Data;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api")]
public class TriviaController : ControllerBase
{
    private readonly DataContext _context;

    public TriviaController(DataContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("trivia")]
    public async Task<ActionResult<FunFact[]>> GetFunFacts()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();
        
        var team = await _context.Teams.Include(x=>x.QrCodesScanned).FirstOrDefaultAsync(x => x.Id == int.Parse(User.Identity.Name!));

        if (team == null)
            return Unauthorized();

        var qrCodes = team.QrCodesScanned.Select(x => x.Id).ToList();

        var funfacts = await _context.QrCodes.AsNoTracking().Include(x=>x.FunFact).Where(x => qrCodes.Contains(x.GroupId)).Select(x=>x.FunFact).Distinct().ToArrayAsync();
        
        return funfacts;
    }
}