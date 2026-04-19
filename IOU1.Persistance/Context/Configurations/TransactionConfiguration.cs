using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("GroupTransaction");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Amount)
               .IsRequired();

        builder.Property(t => t.CreatedAt)
               .HasDefaultValue(DateTime.UtcNow)
               .HasColumnName("AddDate");

        builder.HasOne(t => t.Group)
               .WithMany();

        builder.HasOne(t => t.Buyer)
               .WithMany()
               .HasForeignKey("BuyerId");

        builder.HasOne(t => t.Borrower)
               .WithMany()
               .HasForeignKey("BorrowerId");

        builder.HasOne(e => e.Currency)
               .WithMany()
               .HasForeignKey("CurrencyKey");

        builder.HasOne(t => t.Expense)
               .WithMany(e => e.Transactions)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.BuyerMember)
               .WithMany()
               .HasForeignKey("BuyerMemberId");

        builder.HasOne(t => t.BorrowerMember)
               .WithMany()
               .HasForeignKey("BorrowerMemberId");
    }
}
