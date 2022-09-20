using api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data;

public class TeamsConfiguration : IEntityTypeConfiguration<Team>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.Navigation(x => x.QrCodesScanned).AutoInclude();
        
    }
}

public class QrCodeConfiguration : IEntityTypeConfiguration<QrCode>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<QrCode> builder)
    {
        builder.HasKey(x => x.QrCodeId);
        builder.Property(x => x.QrCodeId).ValueGeneratedNever();
        builder.Navigation(x => x.FunFact).AutoInclude();
        //builder.OwnsOne(x => x.FunFact);
    }
}

public class FunFactConfiguration : IEntityTypeConfiguration<FunFact>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<FunFact> builder)
    {
        builder.HasKey(x => x.FunFactId);
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