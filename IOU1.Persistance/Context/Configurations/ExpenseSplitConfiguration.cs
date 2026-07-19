using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class ExpenseSplitConfiguration : IEntityTypeConfiguration<ExpenseSplit>
{
    public void Configure(EntityTypeBuilder<ExpenseSplit> builder)
    {
        builder.ToTable("ExpenseSplit");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title)
               .IsRequired()
               .HasMaxLength(20);
        builder.Property(e => e.Description)
               .HasMaxLength(255);
        builder.Property(e => e.IconKey)
                .HasMaxLength(20);
        builder.Property(e => e.Version)
               .IsRowVersion();
    }
}
