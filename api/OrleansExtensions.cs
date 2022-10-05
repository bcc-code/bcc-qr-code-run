using System.Net.Sockets;
using System.Reflection;
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
}