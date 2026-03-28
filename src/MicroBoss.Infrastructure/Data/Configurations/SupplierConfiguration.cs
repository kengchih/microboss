using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class SupplierConfiguration : IEntityTypeConfiguration<Supplier>
{
    public void Configure(EntityTypeBuilder<Supplier> builder)
    {
        builder.ToTable("Supplier");
        builder.HasKey(s => s.SupplierId);
        builder.Property(s => s.SupplierId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.SupplierShortName).HasMaxLength(100);
        builder.Property(s => s.SupplierFullName).HasMaxLength(200);
        builder.Property(s => s.Tel1).HasColumnName("Tel_1");
        builder.Property(s => s.Tel2).HasColumnName("Tel_2");
        builder.Property(s => s.PrepareAmount).HasColumnType("decimal(18,2)");
        builder.Property(s => s.TransQuotaAmount).HasColumnType("decimal(18,2)");
        builder.Property(s => s.SettleDiscount).HasColumnType("decimal(18,2)");

        builder.HasMany(s => s.Banks)
               .WithOne(b => b.Supplier)
               .HasForeignKey(b => b.SupplierId);
    }
}
