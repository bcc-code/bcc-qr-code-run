using System.Security.Claims;
using api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features;

[ApiController]
[Route("team")]
public class TeamEndpoint : ControllerBase
{
    private readonly ILogger<TeamEndpoint> _logger;
    private readonly TeamRepository _repository;

    public TeamEndpoint(ILogger<TeamEndpoint> logger, TeamRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpPost("register")]
    public async Task<Team> RegisterTeam(RegisterTeamRequest registerTeamEvent)
    {
        _logger.LogInformation("Team {TeamName} logged in", registerTeamEvent.TeamName);

        var team = await _repository.SaveTeam(new Team(registerTeamEvent.TeamName, registerTeamEvent.ChurchName, registerTeamEvent.Members));

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, team.TeamName),
        }, "Cookies");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);

        return team;
    }

    [HttpGet()]
    [Authorize]
    public async Task<ActionResult<Team>> Get()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        return await _repository.GetTeam(User.Identity.Name!);
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
