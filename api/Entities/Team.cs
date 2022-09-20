using api.Data;
using api.Controllers;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class Team
{
    private readonly DataContext _dataContext;

    public Team(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public int Id { get; set; }
    
    public string TeamName { get; init; }
    public string ChurchName { get; init; }
    public int Members { get; init; }

    public List<Score> Posts { get; set; } = new();
    public List<Score> SecretsFound { get; set; } = new();

    public int Score => Posts.Sum(x => x.Points) + SecretsFound.Sum(x => x.Points);

    public DateTime? FirstScannedQrCode { get; set; }
    public DateTime? LastScannedQrCode { get; set; }
    public TimeSpan? TimeSpent => LastScannedQrCode - FirstScannedQrCode;


    public async Task<bool> AddQrCodeAsync(QrCode qrCode)
    {
        if (qrCode.IsSecret)
        {
            if (SecretsFound.Any(x => x.Id == qrCode.Id))
                return false;
            SecretsFound.Add(new (qrCode.Id, qrCode.Points));
        }
        else
        {
            if (Posts.Any(x => x.Id == qrCode.Id))
                return false;
            Posts.Add(new (qrCode.Id, qrCode.Points));
        }
        
        FirstScannedQrCode ??= DateTime.Now;
        LastScannedQrCode = DateTime.Now;

        await _dataContext.SaveChangesAsync();

        return true;
    }
}

public record Score(int Id, int Points);
