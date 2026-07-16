using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class ExpenseShareConfiguration : IEntityTypeConfiguration<ExpenseShare>
{
    public void Configure(EntityTypeBuilder<ExpenseShare> builder)
    {
        builder.ToTable("ExpenseShare");

        builder.HasKey(es => es.Id);

        builder.Property(es => es.Amount)
               .IsRequired();

        builder.HasOne(es => es.Expense)
               .WithMany(e => e.ExpenseShares)
               .HasForeignKey(es => es.ExpenseId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(es => es.Member)
               .WithMany()
               .HasForeignKey(es => es.MemberId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(es => new { es.ExpenseId, es.MemberId })
               .IsUnique();
    }
}
