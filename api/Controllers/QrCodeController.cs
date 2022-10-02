using api.Data;
using api.Repositories;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace api.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class QrCodeController : ControllerBase
{
    private readonly ILogger<QrCodeController> _logger;
    private readonly DataContext _context;
    private readonly CacheService _cache;

    public QrCodeController(ILogger<QrCodeController> logger, DataContext dataContext, CacheService cache)
    {
        _logger = logger;
        _context = dataContext;
        _cache = cache;
    }

    /// <summary>
    /// Scan a QR Code 
    /// </summary>
    /// <response code="200">Points added</response>
    /// <response code="400">Bad request - QR Code is malformed</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Already Scanned a QR Code from this post</response>
    [HttpPost("scan")]
    public async Task<ActionResult<QrCodeResult>> ScanQrCode(QrCodeScanned request)
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        var qrCodeData = QrCodeParser.Parse(request);
        _logger.LogInformation("Team {Team} scanned QR Code {Request}", User.Identity.Name!,
            qrCodeData?.QrCodeId.ToString() ?? $"PARSE ERROR: {request.Data}");

        if (qrCodeData == null)
            return BadRequest("Ukjent QR-kode!");

        var qrCode = await _cache.GetOrCreateAsync(qrCodeData.QrCodeId.ToString(), TimeSpan.FromMinutes(10),
            () => { return _context.QrCodes.FirstOrDefaultAsync(x => x.QrCodeId == qrCodeData.QrCodeId); });
        if (qrCode == null)
            return BadRequest("Ukjent QR-kode!");

        _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;

        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == int.Parse(User.Identity.Name!));

        if (team == null)
            return Unauthorized();

        if (!await team.AddQrCodeAsync(qrCode))
            return BadRequest("Du har allerede scannet denne QR-koden!");

        return new QrCodeResult()
        {
            Points = qrCode.Points,
            Team = team,
            FunFact = qrCode.FunFact,
        };
    }
}

public class QrCodeResult
{
    public Team Team { get; set; }
    public FunFact FunFact { get; set; }
    public int Points { get; set; }
}

public record QrCodeScanned(string Data);