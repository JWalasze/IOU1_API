using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class MemberBalanceConfiguration : IEntityTypeConfiguration<MemberBalance>
{
    public void Configure(EntityTypeBuilder<MemberBalance> builder)
    {
        builder.ToTable("MemberBalance");

        builder.HasKey(mb => mb.Id);

        builder.Property(mb => mb.Amount)
               .IsRequired();

        builder.Property(mb => mb.UpdatedAt)
               .IsRequired();

        builder.Property(mb => mb.Version)
               .IsRowVersion();

        builder.HasOne(mb => mb.Member)
               .WithMany()
               .HasForeignKey(mb => mb.MemberId)
               .OnDelete(DeleteBehavior.Restrict);
        //TODO: poczytać o DeleteBehaviour jeszcze trochę

        builder.HasOne(mb => mb.CounterpartyMember)
               .WithMany()
               .HasForeignKey(mb => mb.CounterpartyMemberId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(mb => mb.Group)
               .WithMany()
               .HasForeignKey(mb => mb.GroupId);
    }
}
