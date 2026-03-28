using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");
        builder.HasKey(c => c.CustomerId);
        builder.Property(c => c.CustomerId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.CustomerName).HasMaxLength(200);
        builder.Property(c => c.SettleDiscount).HasColumnType("decimal(18,2)");
        builder.Property(c => c.AR).HasColumnType("decimal(18,2)");
        builder.Property(c => c.CL).HasColumnType("decimal(18,2)");
        builder.Property(c => c.ASR).HasColumnType("decimal(18,2)");
    }
}
