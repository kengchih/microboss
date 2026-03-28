using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ProductCostConfiguration : IEntityTypeConfiguration<ProductCost>
{
    public void Configure(EntityTypeBuilder<ProductCost> builder)
    {
        builder.ToTable("ProductCost");
        builder.HasKey(pc => pc.ProductId);
        builder.Property(pc => pc.ProductId).HasMaxLength(50).IsRequired();
        builder.Property(pc => pc.MarketPrice).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.UnitPriceA).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.InPackagePriceA).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.OutPackagePriceA).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.UnitPriceB).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.InPackagePriceB).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.OutPackagePriceB).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.UnitPriceC).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.InPackagePriceC).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.OutPackagePriceC).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.LastInStockPrice).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.UnitCost).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.InPackCost).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.OutPackCost).HasColumnType("decimal(18,2)");
        builder.Property(pc => pc.CostGrossRate).HasColumnType("decimal(18,4)");
        builder.Property(pc => pc.PackageCost).HasColumnType("decimal(18,2)");
    }
}
