using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configurations;

public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.ToTable("IOU1GroupMembers");

        builder.HasKey(gm => gm.Id);

        builder.HasOne(gm => gm.Group)
               .WithMany(g => g.Members)
               .HasForeignKey("IOU1Group")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gm => gm.User)
               .WithMany()
               .OnDelete(DeleteBehavior.Cascade);
    }
}
