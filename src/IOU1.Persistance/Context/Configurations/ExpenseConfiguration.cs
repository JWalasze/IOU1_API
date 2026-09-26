using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expense");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Amount)
               .IsRequired();

        builder.Property(e => e.Title)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.Property(e => e.IsDeleted)
               .IsRequired();

        builder.Property(e => e.IsSettled)
               .IsRequired();

        builder.Property(e => e.SplitId)
               .HasColumnName("SplitMethodId");

        builder.Property(e => e.Description)
               .HasMaxLength(255);

        builder.Property(e => e.Version)
               .IsRowVersion();

        builder.HasOne(e => e.Group)
               .WithMany()
               .HasForeignKey("GroupId");

        builder.Ignore(e => e.Splits);

        builder.HasOne(e => e.Payer)
               .WithMany()
               .HasForeignKey("PayerId");

        builder.HasMany(e => e.Shares)
               .WithOne(es => es.Expense)
               .HasForeignKey(es => es.ExpenseId);

        builder.HasOne(e => e.Category)
               .WithMany()
               .HasForeignKey("CategoryId");

        builder.HasOne(e => e.Split)
               .WithMany()
               .HasForeignKey("SplitId");
    }
}
