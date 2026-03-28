using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.OrderNo);
        builder.Property(o => o.OrderNo).HasMaxLength(50).IsRequired();
        builder.Property(o => o.CustomerId).HasMaxLength(50);
        builder.Property(o => o.CustomerName).HasMaxLength(200);
        builder.Property(o => o.InvoiceDiscount).HasColumnType("decimal(18,2)");
        builder.Property(o => o.OrderDiscount).HasColumnType("decimal(18,2)");
        builder.Property(o => o.PrePayAmount).HasColumnType("decimal(18,2)");
        builder.Property(o => o.TaxRate).HasColumnType("decimal(18,2)");
        builder.Property(o => o.TaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(o => o.DiscountRate).HasColumnType("decimal(18,2)");
        builder.Property(o => o.AccountReceivableAmount).HasColumnType("decimal(18,2)");
        builder.Property(o => o.ActualAmount).HasColumnType("decimal(18,2)");
        builder.Property(o => o.Status).HasConversion(v => (int)v, v => (OrderStatus)v);
        builder.Property(o => o.TaxType)
               .HasConversion(v => v.HasValue ? (int?)v.Value : null, v => v.HasValue ? (TaxType?)v.Value : null);
        builder.Property(o => o.InvoiceType)
               .HasConversion(v => v.HasValue ? (int?)v.Value : null, v => v.HasValue ? (InvoiceType?)v.Value : null);

        builder.HasOne(o => o.Customer)
               .WithMany(c => c.Orders)
               .HasForeignKey(o => o.CustomerId);

        builder.HasMany(o => o.OrderDetails)
               .WithOne(od => od.Order)
               .HasForeignKey(od => od.OrderNo);
    }
}
