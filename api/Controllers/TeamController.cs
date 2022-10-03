﻿using System.Security.Claims;
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
        
        if (registerTeamEvent.Members <= 0 || registerTeamEvent.Members > 5)
        {
            return BadRequest("Antall på laget må være mellom 1 og 5.");
        }

        if (string.IsNullOrEmpty(registerTeamEvent.ChurchName))
        {
            return BadRequest("Velg en menighet fra listen.");
        }

         if (string.IsNullOrEmpty(registerTeamEvent.TeamName))
        {
            return BadRequest("Lagnavn kan ikke være blank.");
        }

        var team = await _context.Teams.FirstOrDefaultAsync(x =>
            x.TeamName == registerTeamEvent.TeamName && x.ChurchName == registerTeamEvent.ChurchName);
        
        if (team != null)
        {
            return BadRequest("Det finnes allerede et lag med dette navnet.");
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
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == int.Parse(User.Identity.Name!));
        if (team != null)
        {
            _context.Remove(team);
            await _context.SaveChangesAsync();
        }
        
        await HttpContext.SignOutAsync();
        return NoContent();
    }
}

public record RegisterTeamRequest(string TeamName, string ChurchName, int Members);
