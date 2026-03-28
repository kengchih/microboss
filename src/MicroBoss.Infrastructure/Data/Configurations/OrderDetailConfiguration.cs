using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("OrderDetails");
        builder.HasKey(od => od.DetailId);
        builder.Property(od => od.DetailId).HasMaxLength(50).IsRequired();
        builder.Property(od => od.OrderNo).HasMaxLength(50).IsRequired();
        builder.Property(od => od.ProductId).HasMaxLength(50);
        builder.Property(od => od.ItemPrice).HasColumnType("decimal(18,2)");
        builder.Property(od => od.ItemAmount).HasColumnType("decimal(18,2)");
        builder.Property(od => od.ItemDiscount).HasColumnType("decimal(18,2)");

        builder.HasOne(od => od.Product)
               .WithMany(p => p.OrderDetails)
               .HasForeignKey(od => od.ProductId);
    }
}
