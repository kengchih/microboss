using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(p => p.ProductId);
        builder.Property(p => p.ProductId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.ProductNo).HasMaxLength(100);
        builder.Property(p => p.ProductName).HasMaxLength(200);
        builder.Property(p => p.ProductNameExt).HasMaxLength(200);
        builder.Property(p => p.Weight).HasColumnType("decimal(18,2)");
        builder.Property(p => p.ProductClass)
               .HasConversion(v => v.HasValue ? (int?)v.Value : null, v => v.HasValue ? (ProductClass?)v.Value : null);

        builder.HasOne(p => p.ProductCost)
               .WithOne(pc => pc.Product)
               .HasForeignKey<ProductCost>(pc => pc.ProductId);

        builder.HasMany(p => p.ProductStocks)
               .WithOne(ps => ps.Product)
               .HasForeignKey(ps => ps.ProductId);

        builder.HasMany(p => p.OrderDetails)
               .WithOne(od => od.Product)
               .HasForeignKey(od => od.ProductId);
    }
}
