using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class PurchaseDetailConfiguration : IEntityTypeConfiguration<PurchaseDetail>
{
    public void Configure(EntityTypeBuilder<PurchaseDetail> builder)
    {
        builder.ToTable("PurchaseDetail");
        builder.HasKey(pd => new { pd.PurchaseId, pd.ItemId });
        builder.Property(pd => pd.PurchaseId).HasMaxLength(50).IsRequired();
        builder.Property(pd => pd.ItemId).HasMaxLength(50).IsRequired();
        builder.Property(pd => pd.ProductId).HasMaxLength(50);
        builder.Property(pd => pd.ItemPrice).HasColumnType("decimal(18,2)");
        builder.Property(pd => pd.ItemDiscount).HasColumnType("decimal(18,2)");
        builder.Property(pd => pd.SubTotal).HasColumnType("decimal(18,2)");
        builder.Property(pd => pd.ItemStatus)
               .HasConversion(v => v.HasValue ? (int?)v.Value : null, v => v.HasValue ? (PurchaseItemStatus?)v.Value : null);
    }
}
