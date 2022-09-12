using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace api.Features;

[ApiController]
[Route("team")]
public class RegisterTeamEndpoint : ControllerBase
{
    private readonly ILogger<RegisterTeamEndpoint> _logger;

    public RegisterTeamEndpoint(ILogger<RegisterTeamEndpoint> logger)
    {
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<TeamResult>> RegisterTeam(RegisterTeamEvent registerTeamEvent)
    {
        // register team

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, registerTeamEvent.TeamName),
        }, "qr-code");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal);

        return new TeamResult(registerTeamEvent.TeamName, registerTeamEvent.TeamMemberCount, registerTeamEvent.GroupCode);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
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