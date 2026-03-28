using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class MaintenancePartLogConfiguration : IEntityTypeConfiguration<MaintenancePartLog>
{
    public void Configure(EntityTypeBuilder<MaintenancePartLog> builder)
    {
        builder.ToTable("MaintenancePartLog");
        builder.HasKey(m => m.PartLogId);
        builder.Property(m => m.PartLogId).HasMaxLength(50).IsRequired();
        builder.Property(m => m.ProductId).HasMaxLength(50);
        builder.Property(m => m.ProductNo).HasMaxLength(100);
        builder.Property(m => m.SettleNo).HasMaxLength(50);
        builder.Property(m => m.LogType).HasConversion(v => (int)v, v => (StockActionType)v);
    }
}
