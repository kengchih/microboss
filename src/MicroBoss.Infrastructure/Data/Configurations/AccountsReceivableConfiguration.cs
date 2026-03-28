using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class AccountsReceivableConfiguration : IEntityTypeConfiguration<AccountsReceivable>
{
    public void Configure(EntityTypeBuilder<AccountsReceivable> builder)
    {
        builder.ToTable("AccountsReceivable");
        builder.HasKey(ar => ar.Id);
        builder.Property(ar => ar.CustomerId).HasMaxLength(50);
        builder.Property(ar => ar.Number).HasMaxLength(50);
        builder.Property(ar => ar.OrderItemAmount).HasColumnType("decimal(18,2)");
        builder.Property(ar => ar.OrderTaxAmount).HasColumnType("decimal(18,2)");
        builder.Property(ar => ar.OrderTotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(ar => ar.PriorPeriodBalanceAmount).HasColumnType("decimal(18,2)");
        builder.Property(ar => ar.TotalAmount).HasColumnType("decimal(18,2)");
        builder.Property(ar => ar.ActualAmount).HasColumnType("decimal(18,2)");
        builder.Property(ar => ar.Status).HasConversion(v => (int)v, v => (AccountsReceivableStatus)v);

        builder.HasMany(ar => ar.Details)
               .WithOne(d => d.AccountsReceivable)
               .HasForeignKey(d => d.AccountsReceivableId);

        builder.HasMany(ar => ar.Postings)
               .WithOne(p => p.AccountsReceivable)
               .HasForeignKey(p => p.AccountsReceivableId);
    }
}
