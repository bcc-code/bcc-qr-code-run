using System.Security.Claims;
using api.Grains;
using api.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace api.Controllers;

[ApiController]
[Route("api/team")]
public class TeamEndpoint : ControllerBase
{
    private readonly ILogger<TeamEndpoint> _logger;
    private readonly IGrainFactory _factory;

    public TeamEndpoint(ILogger<TeamEndpoint> logger, IGrainFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    [HttpPost("register")]
    public async Task<ActionResult<Team?>> RegisterTeam(RegisterTeamRequest registerTeamEvent)
    {
        _logger.LogInformation("Team {TeamName} logged in", registerTeamEvent.TeamName);

        if (registerTeamEvent.Members <= 0 || registerTeamEvent.Members > 5)
            return BadRequest("Antall deltakere må være mellom 1 og 5.");

        if (string.IsNullOrEmpty(registerTeamEvent.ChurchName)) return BadRequest("Velg menighet fra listen.");

        if (string.IsNullOrEmpty(registerTeamEvent.TeamName)) return BadRequest("Lagnavn kan ikke være blank.");

        var team = _factory.GetGrain<ITeam>($"{registerTeamEvent.ChurchName}-{registerTeamEvent.TeamName}");
        if (await team.IsActive())
            return BadRequest("Det finnes allerede et lag med dette navnet");

        await team.Register(registerTeamEvent);

        var claimsIdentity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, team.GetPrimaryKeyString()),
        }, "Cookies");

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
        };

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
        await Request.HttpContext.SignInAsync("Cookies", claimsPrincipal, authProperties);

        return await team.GetTeamData();
    }

    [HttpGet()]
    [Authorize]
    public async Task<ActionResult<Team?>> Get()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        var team = _factory.GetGrain<ITeam>(User.Identity.Name);
        if (!await team.IsActive())
        {
            return Unauthorized();
        }

        return await team.GetTeamData();
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Team {TeamName} logged out", User.FindFirst(ClaimTypes.Name)?.Value);
        var team = _factory.GetGrain<ITeam>(User.Identity?.Name);
        await team.Clear();

        await HttpContext.SignOutAsync();
        return NoContent();
    }
}

public record RegisterTeamRequest(string TeamName, string ChurchName, int Members);