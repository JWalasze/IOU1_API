using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.ToTable("GroupMember");

        builder.HasKey(gm => gm.Id);

        builder.HasOne(gm => gm.Group)
               .WithMany(g => g.Members)
               .HasForeignKey("GroupId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gm => gm.User)
               .WithMany(u => u.MemberGroups)
               .HasForeignKey("MemberId")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
