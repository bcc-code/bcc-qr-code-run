using api.Controllers;
using api.Data;
using api.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace api.Services
{
    public class ResultsService
    {
        private readonly DataContext _context;
        private readonly CacheService _cache;

        private TimeSpan LOCALTTL = TimeSpan.FromSeconds(15);
        private TimeSpan DISTRIBUTEDTTL = TimeSpan.FromMinutes(5);

        public ResultsService(DataContext db, CacheService cache)
        {
            _context = db;
            _cache = cache;
        }

        public async Task<TeamResult> GetResultForTeamAsync(int teamId)
        {
            var team = await GetTeamByIdAsync(teamId);
            return team != null ? GetTeamResult(team) : TeamResult.Empty;
        }

        public async Task<ChurchResult> GetResultForChurchAsync(int teamId)
        {
            var team = await GetTeamByIdAsync(teamId);
            if (team != null)
            {
                return team != null ? await GetResultForChurchAsync(team.ChurchName) : ChurchResult.Empty;
            }
            return ChurchResult.Empty;
        }

        public async Task<TeamResult> GetResultForTeamAsync(string teamName, string churchName)
        {
            var team = await GetTeamAsync(teamName, churchName);
            return team != null ? GetTeamResult(team) : TeamResult.Empty;
            
        }

        private TeamResult GetTeamResult(Team team)
        {
            if (team != null)
            {
                return new TeamResult
                {
                    Points = team.QrCodesScanned.Sum(q => q.Points),
                    Posts = team.QrCodesScanned.Count(q => !q.IsSecret),
                    Secrets = team.QrCodesScanned.Count(q => q.IsSecret),
                    TeamName = team.TeamName,
                    TimeSpent = team.TimeSpent.GetValueOrDefault(),
                    Members = team.Members
                };
            }
            return TeamResult.Empty;
        }

        public Task<ChurchResult?> GetResultForChurchAsync(string churchName)
        {
            return _cache.GetOrCreateAsync($"results_{churchName}", LOCALTTL, DISTRIBUTEDTTL, async () =>
            {
                var church = await GetChurchAsync(churchName);
                if (church != null)
                {
                    var teams = await _context.Teams.Where(x => x.ChurchName == churchName).ToArrayAsync();
                    var results = teams.Select(t => GetTeamResult(t)).ToArray();
                    var activeTeams = results.Where(r => r.Points > 0);
                    var activeParticipants = activeTeams.Sum(t => t.Members);
                    var totalTimeSpent = activeTeams.Any() ? activeTeams.Select(t => t.TimeSpent).Aggregate((c, n) => c.Add(n)) : TimeSpan.Zero;
                    var participation = Math.Min((int)Math.Round((church.Participants > 0 ? ((decimal)activeParticipants / church.Participants) : 1) * 100), 100);
                    var totalPoints = activeTeams.Sum(t => t.Points);
                    var teamsCount = activeTeams.Count();
                    var averagePoints = teamsCount > 0 ? Math.Round(((decimal)totalPoints / (decimal)teamsCount), 2) : 0;
                    return new ChurchResult
                    {
                        Church = church.Name,
                        Country = GetCountryName(church.CountryCode),
                        Participants = activeParticipants,
                        Participation = participation,
                        Teams = teamsCount,
                        Registrations = church.Participants,
                        Points = totalPoints,
                        AveragePoints = averagePoints,
                        Score = participation * averagePoints,
                        TimeSpent = totalTimeSpent.ToString("hh\\:mm", CultureInfo.InvariantCulture)
                    };
                }

                return ChurchResult.Empty;
            });


        }

        private string GetCountryName(string countryCode)
        {
            try
            {
                var region = new RegionInfo(countryCode);
                return region.EnglishName;
            }
            catch
            {
                return countryCode ?? "";
            }
        }

        public async Task<List<ChurchResult?>> GetResultsAsync()
        {
            var churchNames = await GetChurchNamesAsync() ?? new List<string>();
            var results = new List<ChurchResult?>();
            foreach (var church in churchNames)
            {
                results.Add(await GetResultForChurchAsync(church));
            }
            return results.OrderByDescending(r => r.Score).ToList();
        }

        public async Task UpdateResultsFor(string teamName, string churchName)
        {
            var team = await GetTeamAsync(teamName, churchName);
            if (team != null)
            {
                var tasks = new List<Task>();
                tasks.Add(_cache.ClearAsync($"results_team_{team.Id}"));
                tasks.Add(_cache.ClearAsync($"results_team_{teamName}_{churchName}"));
                tasks.Add(_cache.ClearAsync($"results_{churchName}"));
            }
        }



        private Task<Team?> GetTeamByIdAsync(int teamId)
        {
            return _cache.GetOrCreateAsync($"results_team_{teamId}", LOCALTTL, DISTRIBUTEDTTL, async () =>
            {
                var team = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teamId);
                return team;
            });
        }

        private Task<Team?> GetTeamAsync(string teamName, string churchName)
        {
            return _cache.GetOrCreateAsync($"results_team_{teamName}_{churchName}", LOCALTTL, DISTRIBUTEDTTL, async () =>
            {
                var team = await _context.Teams.FirstOrDefaultAsync(x => x.TeamName == teamName && x.ChurchName == churchName);
                return team;
            });
        }

        private Task<List<string>?> GetChurchNamesAsync()
        {
            return _cache.GetOrCreateAsync($"results_church_names", DISTRIBUTEDTTL, async () =>
            {
                return await _context.Churches.Select(c => c.Name).ToListAsync() ?? new List<string>();
            });
        }

        private Task<Church?> GetChurchAsync(string churchName)
        {
            return _cache.GetOrCreateAsync($"results_church_{churchName}", DISTRIBUTEDTTL, async () =>
            {
                return await _context.Churches.FirstOrDefaultAsync(c => c.Name == churchName);
            });
        }


    }



    public class TeamResult
    {
        public string TeamName { get; set; } = "";

        public int Posts { get; set; }

        public int Secrets { get; set; }

        public TimeSpan TimeSpent { get; set; }

        public int Points { get; set; }

        public int Members { get; set; }

        public static TeamResult Empty => new TeamResult
        {
            Points = 0,
            Posts = 0,
            Secrets = 0,
            TeamName = "",
            TimeSpent = TimeSpan.Zero

        };
    
    }

    public class ChurchResult
    {
        public string Church { get; set; } = "";

        public string Country { get; set; } = "";

        public int Teams { get; set; }

        public string TimeSpent { get; set; } = "00:00";

        public int Registrations { get; set; }

        public int Participants { get; set; }

        public decimal Participation { get; set; }

        public int Points { get; set; }

        public decimal Score { get; set; }

        public decimal AveragePoints { get; set; }

        public static ChurchResult Empty => new ChurchResult
        {
            Church = "",
            Country = "",
        };


    }
}
