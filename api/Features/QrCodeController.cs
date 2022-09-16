using api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace api.Features;

[ApiController]
public class QrCodeController : ControllerBase
{
    private readonly ILogger<QrCodeController> _logger;
    private readonly TeamRepository _teamRepository;
    private readonly QrCodeRepository _qrCodeRepository;

    public QrCodeController(ILogger<QrCodeController> logger, TeamRepository teamRepository, QrCodeRepository qrCodeRepository)
    {
        _logger = logger;
        _teamRepository = teamRepository;
        _qrCodeRepository = qrCodeRepository;
    }

    /// <summary>
    /// Scan a QR Code 
    /// </summary>
    /// <response code="200">Points added</response>
    /// <response code="400">Bad request - QR Code is malformed or team is not found</response>
    /// <response code="401">Not authenticated</response>
    /// <response code="403">Already Scanned a QR Code from this post</response>

    [HttpPost("scan")]
    public async Task<ActionResult<QrCodeResult>> ScanQrCode(QrCodeScanned request)
    {
        if (User.Identity?.IsAuthenticated != true)
            return Unauthorized();

        var qrCodeData = QrCodeParser.Parse(request);
        _logger.LogInformation("Team {Team} scanned QR Code {Request}", User.Identity.Name!, qrCodeData?.QrCodeId.ToString() ?? "PARSE ERROR");

        if (qrCodeData == null)
            return BadRequest();
        
        var qrCode = await _qrCodeRepository.GetQrCode(qrCodeData);
        if (qrCode == null)
            return BadRequest();

        var team = await _teamRepository.GetTeam(User.Identity.Name!);

        if (!team.AddQrCode(qrCode))
            return Forbid();

        await _teamRepository.SaveTeam(team);

        return new QrCodeResult()
        {
            Team = team,
            FunFact = qrCode.FunFact,
        };
    }
}



public class QrCodeResult
{
    public Team Team { get; set; }
    public FunFact FunFact { get; set; }
}

public record QrCodeScanned(string Data);