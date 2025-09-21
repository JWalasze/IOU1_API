using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configurations;

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

        builder.Property(e => e.Description)
               .HasMaxLength(255);

        builder.HasOne(e => e.Group)
               .WithMany();

        builder.HasOne(e => e.Buyer)
               .WithMany();

        builder.HasOne(e => e.Currency)
               .WithMany();

        // Transactions navigation is configured on Transaction side
    }
}
