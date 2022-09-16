using api.Features;

namespace api.Repositories;

public class TeamRepository
{
    public TeamRepository()
    {
        
    }

    public async Task<Team> GetTeam(string teamName)
    {
        return new Team(teamName, "", 1);
    }

    public async Task<Team> SaveTeam(Team team)
    {
        return team;
    }
}

public record Team(string TeamName, string ChurchName, int Members)
{
    public List<Score> Posts { get; set; } = new();
    public List<Score> SecretsFound { get; set; } = new();

    public int Score => Posts.Sum(x => x.Points) + SecretsFound.Sum(x => x.Points);

    public DateTime? FirstScannedQrCode { get; set; }
    public DateTime? LastScannedQrCode { get; set; }
    public TimeSpan? TimeSpent => LastScannedQrCode - FirstScannedQrCode;

    public bool AddQrCode(QrCode qrCode)
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

        return true;
    }
}

public record Score(int Id, int Points);
