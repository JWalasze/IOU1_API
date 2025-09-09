using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configurations;

public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.ToTable("IOU1GroupMember");

        builder.HasKey(gm => gm.Id);

        builder.HasOne(gm => gm.Group)
               .WithMany()
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gm => gm.User)
               .WithMany()
               .HasForeignKey("UserId")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
