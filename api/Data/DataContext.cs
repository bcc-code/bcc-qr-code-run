using System.Reflection;
using api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Team> Teams { get; set; }
    public DbSet<QrCode> QrCodes { get; set; }
    public DbSet<Church> Churches { get; set; }
    public DbSet<CacheEntry> CacheEntries { get; set; }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

public class Church
{
    public string Name { get; set; }

    public string CountryCode { get; set;}

    public int Participants { get; set; }

    public int TargetParticipants { get; set; }
}

public class CacheEntry
{
    public string Key { get; set; }

    public string Value { get; set; }

    public DateTimeOffset? Expires { get; set;}

}