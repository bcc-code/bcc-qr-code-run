using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features;

[ApiController]
[Route("team")]
public class TeamEndpoint : ControllerBase
{
    private readonly ILogger<TeamEndpoint> _logger;

    public TeamEndpoint(ILogger<TeamEndpoint> logger)
    {
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<TeamResult>> RegisterTeam(RegisterTeamEvent registerTeamEvent)
    {
        _logger.LogInformation("Team {TeamName} logged in", registerTeamEvent.TeamName);
        // register team

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, registerTeamEvent.TeamName),
        }, "Cookies");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);

        return new TeamResult(registerTeamEvent.TeamName, registerTeamEvent.TeamMemberCount, registerTeamEvent.GroupCode);
    }

    [HttpGet()]
    [Authorize]
    public IActionResult Get()
    {
        var userClaims = User.Claims.Select(x => new {x.Type, x.Value}).ToList();
        return Ok(userClaims);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Team {TeamName} logged in", User.FindFirst(ClaimTypes.Name)?.Value);

        await HttpContext.SignOutAsync();
        return NoContent();
    }
}

public class RegisterTeamEvent
{
    public string GroupCode { get; set; } = "";
    public string TeamName { get; set; } = "";
    public int TeamMemberCount { get; set; }
}

public record TeamResult(string TeamName, int TeamMemberCount, string GroupName);