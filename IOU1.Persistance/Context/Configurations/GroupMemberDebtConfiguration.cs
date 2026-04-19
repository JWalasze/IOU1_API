using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class GroupMemberDebtConfiguration : IEntityTypeConfiguration<GroupMemberDebt>
{
    public void Configure(EntityTypeBuilder<GroupMemberDebt> builder)
    {
        builder.ToTable("GroupMemberDebt");
        builder.HasKey(gmd => gmd.Id);

        builder
            .Property(gmd => gmd.Balance)
            .HasColumnName("Balance");

        builder
            .HasOne(gmd => gmd.Member)
            .WithMany()
            .HasForeignKey(gmd => gmd.MemberId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(gmd => gmd.Debtor)
            .WithMany()
            .HasForeignKey(gmd => gmd.DebtorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(gmd => gmd.Group)
            .WithMany()
            .HasForeignKey(gmd => gmd.GroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
