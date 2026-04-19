using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("GroupExpense");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.TotalAmount)
               .IsRequired();

        builder.Property(e => e.Title)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(e => e.CreatedAt)
               .IsRequired();

        builder.Property(e => e.Description)
               .HasMaxLength(255);

        builder.HasOne(e => e.Group)
               .WithMany()
               .HasForeignKey("GroupId");

        builder.Ignore(e => e.Splits);

        builder.HasOne(e => e.Buyer)
               .WithMany()
               .HasForeignKey("BuyerId");

        builder.HasOne(e => e.Currency)
               .WithMany()
               .HasForeignKey("CurrencyKey");
    }
}
