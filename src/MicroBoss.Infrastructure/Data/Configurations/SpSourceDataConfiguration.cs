using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class SpSourceDataConfiguration : IEntityTypeConfiguration<SpSourceData>
{
    public void Configure(EntityTypeBuilder<SpSourceData> builder)
    {
        builder.ToTable("SPSourceData");
        builder.HasKey(s => s.PN);
        builder.Property(s => s.PN).HasMaxLength(100).IsRequired();
        builder.Property(s => s.PG).HasMaxLength(100);
        builder.Property(s => s.Class).HasMaxLength(100);
        builder.Property(s => s.ENDesc).HasMaxLength(500);
        builder.Property(s => s.CNDesc).HasMaxLength(500);
        builder.Property(s => s.Successor).HasMaxLength(100);
    }
}
