using api.Grains;
using api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace api.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public class QrCodeController : ControllerBase
{
    private readonly ILogger<QrCodeController> _logger;
    private readonly IGrainFactory _factory;

    public QrCodeController(ILogger<QrCodeController> logger, IGrainFactory factory)
    {
        _logger = logger;
        _factory = factory;
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

        var team = _factory.GetGrain<ITeam>(User.Identity.Name);
        if (!await team.IsActive())
            return Unauthorized();

        var result = await team.RegisterPost(qrCodeData);
        if (result == null)
            return BadRequest("Du har allerede scannet denne QR-koden!");

        return result;
    }
}

public class QrCodeResult
{
    public Team Team { get; set; }
    public FunFact FunFact { get; set; }
    public int Points { get; set; }
}

public record QrCodeScanned(string Data);