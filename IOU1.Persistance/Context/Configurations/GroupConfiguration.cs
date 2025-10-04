using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configurations;

public class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("CommunityGroup");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Description)
               .IsRequired();

        builder.HasOne(g => g.Owner)
               .WithMany(u => u.OwnedGroups)
               .HasForeignKey("CreatedById")
               .OnDelete(DeleteBehavior.Cascade);
    }
}
