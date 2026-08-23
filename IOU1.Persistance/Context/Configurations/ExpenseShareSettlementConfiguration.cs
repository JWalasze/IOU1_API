using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public sealed class ExpenseShareSettlementConfiguration : IEntityTypeConfiguration<ExpenseShareSettlement>
{
    public void Configure(EntityTypeBuilder<ExpenseShareSettlement> builder)
    {
        builder.ToTable("ExpenseShareSettlement");

        builder.HasKey(ess => ess.Id);

        builder.HasOne(ess => ess.ExpenseShare)
               .WithMany(es => es.ExpenseShareSettlements)
               .HasForeignKey(ess => ess.ExpenseShareId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ess => ess.Settlement)
               .WithMany()
               .HasForeignKey(es => es.SettlementId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
