using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class AccountsReceivableDetailConfiguration : IEntityTypeConfiguration<AccountsReceivableDetail>
{
    public void Configure(EntityTypeBuilder<AccountsReceivableDetail> builder)
    {
        builder.ToTable("AccountsReceivableDetail");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.OrderNo).HasMaxLength(50);
        builder.Property(d => d.Amount).HasColumnType("decimal(18,2)");
        builder.Property(d => d.AccountsReceivableId).HasColumnName("AccountsReceivable_Id");
        builder.Property(d => d.Type).HasConversion(v => (int)v, v => (AccountsReceivableType)v);
    }
}
