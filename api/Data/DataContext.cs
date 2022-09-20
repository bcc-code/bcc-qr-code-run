using System.Reflection;
using api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class DataContext : DbContext
{

    public static string GetConnectionString()
    {
        var connectionString = Environment.GetEnvironmentVariable("AZURE_POSTGRESQL_CONNECTIONSTRING");
        if (!string.IsNullOrEmpty(connectionString))
        {
            return connectionString;
        }
        var dbPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
        var dbName = Environment.GetEnvironmentVariable("POSTGRES_DB");
        var dbUser = Environment.GetEnvironmentVariable("POSTGRES_USER");
        var dbHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
        var dbPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
        return $"Host={dbHost}{(dbPort != "5432" ? ";Port=" + (dbPort ?? "") : "")};Database={dbName};Username={dbUser};Password={dbPassword};Timeout=300;CommandTimeout=300";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {

        options.UseNpgsql(GetConnectionString());
    }

    public DbSet<Team> Teams { get; set; }
    public DbSet<QrCode> QrCodes { get; set; }
    public DbSet<Church> Churches { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

public class Church
{
    public string Name { get; set; }
}