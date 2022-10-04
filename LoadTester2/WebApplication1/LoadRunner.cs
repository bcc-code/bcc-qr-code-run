using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace LoadTester;

public class LoadRunner
{
    private readonly string _churchName;
    private readonly string _teamName;

    public LoadRunner(string churchName, string teamName)
    {
        _churchName = churchName;
        _teamName = teamName;
    }


    public Task Run()
    {
        return Task.Run(RunTask);
    }

    public async Task RunTask()
    {
        var cookies = new CookieContainer();
        using var handler = new HttpClientHandler()
        {
            CookieContainer = cookies,
        };
        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://jordenrundt.bcc.no/api/"),
        };

        await client.PostAsJsonAsync("team/register", new {
            TeamName = _teamName,
            ChurchName = _churchName,
            Members = Random.Shared.Next(1,5),
        });

        foreach (var i in GetPosts())
        {
            await client.PostAsJsonAsync("scan", new
            {
                Data = GetQrCode(i)
            });

            for (var j = 0; j < 2; j++)
            {
                await Task.Delay(Random.Shared.Next(5000));

                await client.GetAsync("team");
                await Task.Delay(Random.Shared.Next(5000));

                await client.GetAsync("results/mychurch");
                await Task.Delay(Random.Shared.Next(5000));

                await client.GetAsync("results");
                await Task.Delay(Random.Shared.Next(5000));

                await client.GetAsync("trivia");
                await Task.Delay(Random.Shared.Next(5000));
            }
        }

        Console.WriteLine($"Logging out of team {_teamName}");
        await client.PostAsJsonAsync("team/logout", new
        {
            TeamName = _teamName,
            ChurchName = _churchName,
            Members = Random.Shared.Next(1, 5),
        });
    }

    static int[] GetPosts()
    {
        int Post(int start) => Random.Shared.Next(start, start+3);
        return new[]  {
            Post(1),Post(4),Post(7),Post(10),Post(13),Post(16),19,20,21,22,23,24,25,
        };
    }
    
    
    static string GetQrCode(int i) => Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new QrCodeData(i))));
}

public record QrCodeData(int QrCodeId);
