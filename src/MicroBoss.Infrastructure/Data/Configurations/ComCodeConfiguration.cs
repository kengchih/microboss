using MicroBoss.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroBoss.Infrastructure.Data.Configurations;

public class ComCodeConfiguration : IEntityTypeConfiguration<ComCode>
{
    public void Configure(EntityTypeBuilder<ComCode> builder)
    {
        builder.ToTable("ComCode");
        builder.HasKey(c => new { c.GroupId, c.CodeId });
        builder.Property(c => c.GroupId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.CodeId).HasMaxLength(50).IsRequired();
        builder.Property(c => c.CodeName).HasMaxLength(200).IsRequired();
    }
}
