using System.Security.Claims;
using api.Data;
using api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/team")]
public class TeamEndpoint : ControllerBase
{
    private readonly ILogger<TeamEndpoint> _logger;
    private readonly DataContext _context;

    public TeamEndpoint(ILogger<TeamEndpoint> logger, DataContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpPost("register")]
    public async Task<ActionResult<Team>> RegisterTeam(RegisterTeamRequest registerTeamEvent)
    {
        _logger.LogInformation("Team {TeamName} logged in", registerTeamEvent.TeamName);
        
        if (registerTeamEvent.Members <= 0)
        {
            return ValidationProblem("Members");
        }

        var team = await _context.Teams.FirstOrDefaultAsync(x =>
            x.TeamName == registerTeamEvent.TeamName && x.ChurchName == registerTeamEvent.ChurchName);
        
        if (team == null)
        {
            var team1 = new Team(_context)
            {
                Members = registerTeamEvent.Members,
                TeamName = registerTeamEvent.TeamName,
                ChurchName = registerTeamEvent.ChurchName,
            };

            _context.Teams.Add(team1);
            await _context.SaveChangesAsync();
            team = team1;
        }

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, team.Id.ToString()),
        }, "Cookies");

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
        };

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal, authProperties);

        return team;
    }

    [HttpGet()]
    [Authorize]
    public async Task<ActionResult<Team>> Get()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == int.Parse(User.Identity.Name!));
        if (team == null)
            return Unauthorized();
        
        return team;
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Team {TeamName} logged in", User.FindFirst(ClaimTypes.Name)?.Value);

        await HttpContext.SignOutAsync();
        return NoContent();
    }
}

public record RegisterTeamRequest(string TeamName, string ChurchName, int Members);
