using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class RowIndexConfiguration : IEntityTypeConfiguration<RowIndex>
{
    public void Configure(EntityTypeBuilder<RowIndex> builder)
    {
        builder.ToTable("RowIndex");
        builder.HasKey(r => r.RowKey);
        builder.Property(r => r.RowKey).HasMaxLength(50).IsRequired();
        builder.Property(r => r.NextValue).HasMaxLength(100);
        builder.Property(r => r.RowVersion).IsRowVersion();
    }
}
