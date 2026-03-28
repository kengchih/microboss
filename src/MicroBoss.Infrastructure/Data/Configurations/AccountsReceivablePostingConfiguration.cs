using MicroBoss.Domain.Entities;
using MicroBoss.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class AccountsReceivablePostingConfiguration : IEntityTypeConfiguration<AccountsReceivablePosting>
{
    public void Configure(EntityTypeBuilder<AccountsReceivablePosting> builder)
    {
        builder.ToTable("AccountReceivablePosting");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
        builder.Property(p => p.AccountsReceivableId).HasColumnName("AccountsReceivable_Id");
        builder.Property(p => p.Account).HasMaxLength(100);
        builder.Property(p => p.Bank).HasMaxLength(100);
        builder.Property(p => p.CheckNumber).HasMaxLength(50);
        builder.Property(p => p.Type).HasConversion(v => (int)v, v => (AccountsReceivablePostingType)v);
    }
}
