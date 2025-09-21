using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("GroupTransaction");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Amount)
               .IsRequired();

        builder.Property(t => t.AddDate)
               .HasDefaultValue(DateTime.UtcNow);

        builder.Property(t => t.ModificationDate);

        builder.HasOne(t => t.Group)
               .WithMany();

        builder.HasOne(t => t.Buyer)
               .WithMany();

        builder.HasOne(t => t.Borrower)
               .WithMany();

        builder.HasOne(t => t.Currency)
               .WithMany();

        builder.HasOne(t => t.Status)
               .WithMany();

        builder.HasOne(t => t.Expense)
               .WithMany(e => e.Transactions)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
