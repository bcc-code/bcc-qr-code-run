using api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data;

public class TeamsConfiguration : IEntityTypeConfiguration<Team>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        
    }
}

public class QrCodeConfiguration : IEntityTypeConfiguration<QrCode>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<QrCode> builder)
    {
        builder.OwnsOne(x => x.FunFact);
    }
}


public class ChurchConfiguration : IEntityTypeConfiguration<Church>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Church> builder)
    {
        builder.HasKey(x => x.Name);
    }
}