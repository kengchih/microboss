using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class SupplierBankConfiguration : IEntityTypeConfiguration<SupplierBank>
{
    public void Configure(EntityTypeBuilder<SupplierBank> builder)
    {
        builder.ToTable("SupplierBank");
        builder.HasKey(s => new { s.SupplierId, s.BankAccount });
        builder.Property(s => s.SupplierId).HasMaxLength(50).IsRequired();
        builder.Property(s => s.BankAccount).HasMaxLength(100).IsRequired();
        builder.Property(s => s.BankCode).HasMaxLength(20);
        builder.Property(s => s.BankName).HasMaxLength(200);
    }
}
