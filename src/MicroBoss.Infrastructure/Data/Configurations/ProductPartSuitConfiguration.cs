using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ProductPartSuitConfiguration : IEntityTypeConfiguration<ProductPartSuit>
{
    public void Configure(EntityTypeBuilder<ProductPartSuit> builder)
    {
        builder.ToTable("ProductPartSuit");
        builder.HasKey(ps => ps.SuitId);
        builder.Property(ps => ps.ReferenceProductId).HasMaxLength(50);
    }
}
