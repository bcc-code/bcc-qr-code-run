using System.Net.Sockets;
using System.Reflection;
using api.Data;
using api.Grains;
using Microsoft.EntityFrameworkCore;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace api;

public static class OrleansExtensions
{
    public static void AddOrleans(this WebApplicationBuilder builder)
    {
        builder.Host.UseOrleans(c =>
        {
            c.UseDashboard();
            c.AddApplicationInsightsTelemetryConsumer();

            c.AddStartupTask(StartupTask);

            if (builder.Environment.IsDevelopment())
            {
                c.UseLocalhostClustering()
                    .ConfigureEndpoints("localhost", 9889, 9099, AddressFamily.InterNetwork, true)
                    .ConfigureLogging(logging => logging.AddConsole());
            }
            else
            {
                c.ConfigureEndpoints(11111, 30000);
                c.Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "qr-code-run-cluster";
                    options.ServiceId = "QR-Code-Run";
                });
                
                var connectionString = builder.Configuration.GetValue<string>("STORAGE_AZ_ACCOUNT_KEY");
                c.UseAzureStorageClustering(options => options.ConfigureTableServiceClient(connectionString));
                c.ConfigureLogging(logging => logging.AddConsole());
            }

            c.ConfigureApplicationParts(manager => manager.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences());
        });
    }

    private static async Task StartupTask(IServiceProvider services, CancellationToken cancellationToken)
    {
        var dataContext = services.GetService<DataContext>()!;
        var grainFactory = services.GetService<IGrainFactory>()!;

        var teams = await dataContext.Teams.ToListAsync(cancellationToken: cancellationToken);
        foreach (var team in teams)
        {
            var teamGrain = grainFactory.GetGrain<ITeam>(team.ChurchName + "-" + team.TeamName);
            await teamGrain.IsActive(); // smooth startup to load everything sequentially, to avoid overloading the database
        }
    }
}