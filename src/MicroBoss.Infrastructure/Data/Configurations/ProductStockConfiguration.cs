using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ProductStockConfiguration : IEntityTypeConfiguration<ProductStock>
{
    public void Configure(EntityTypeBuilder<ProductStock> builder)
    {
        builder.ToTable("ProductStock");
        builder.HasKey(ps => new { ps.StockNo, ps.ProductId });
        builder.Property(ps => ps.StockNo).HasMaxLength(50).IsRequired();
        builder.Property(ps => ps.ProductId).HasMaxLength(50).IsRequired();
        builder.Property(ps => ps.Location).HasMaxLength(200);
    }
}
