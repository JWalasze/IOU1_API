using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class ExpenseCategoryConfiguration : IEntityTypeConfiguration<ExpenseCategory>
{
    public void Configure(EntityTypeBuilder<ExpenseCategory> builder)
    {
        builder.ToTable("ExpenseCategory");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title)
               .IsRequired()
               .HasMaxLength(50);
        builder.Property(e => e.Description)
               .HasMaxLength(255);
        builder.Property(e => e.IconKey)
                .HasMaxLength(20);
        builder.HasOne(e => e.Group)
               .WithMany()
               .HasForeignKey("GroupId");
        builder.Property(e => e.Version)
               .IsRowVersion();
    }
}
