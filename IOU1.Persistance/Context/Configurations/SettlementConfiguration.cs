using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class SettlementConfiguration : IEntityTypeConfiguration<Settlement>
{
    public void Configure(EntityTypeBuilder<Settlement> builder)
    {
        builder.ToTable("Settlement");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Amount)
               .IsRequired();

        builder.Property(s => s.SettledAt)
               .IsRequired();

        builder.Property(s => s.Version)
               .IsRowVersion();

        builder.HasOne(s => s.Group)
               .WithMany()
               .HasForeignKey(s => s.GroupId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.FromMember)
               .WithMany()
               .HasForeignKey(s => s.FromMemberId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.ToMember)
               .WithMany()
               .HasForeignKey(s => s.ToMemberId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
