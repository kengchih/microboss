using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class PurchaseConfiguration : IEntityTypeConfiguration<Purchase>
{
    public void Configure(EntityTypeBuilder<Purchase> builder)
    {
        builder.ToTable("Purchase");
        builder.HasKey(p => p.PurchaseId);
        builder.Property(p => p.PurchaseId).HasMaxLength(50).IsRequired();
        builder.Property(p => p.SupplierId).HasMaxLength(50);
        builder.Property(p => p.TaxRate).HasColumnType("decimal(18,2)");
        builder.Property(p => p.TaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.TaxType).HasConversion(v => (int)v, v => (TaxType)v);
        builder.Property(p => p.InvoiceType).HasConversion(v => (int)v, v => (InvoiceType)v);
        builder.Property(p => p.Status)
               .HasConversion(v => v.HasValue ? (int?)v.Value : null, v => v.HasValue ? (PurchaseStatus?)v.Value : null);

        builder.HasMany(p => p.PurchaseDetails)
               .WithOne(pd => pd.Purchase)
               .HasForeignKey(pd => pd.PurchaseId);
    }
}
