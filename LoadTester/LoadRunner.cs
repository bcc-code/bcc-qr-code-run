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

    public async Task Run()
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

        for (var i = 1; i <= 26; i++)
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

        await client.PostAsync("team/logout", new StringContent(""));
    }
    
    
    static string GetQrCode(int i) => Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new QrCodeData(i))));
}

public record QrCodeData(int QrCodeId);
