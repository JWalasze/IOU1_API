using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

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
               .HasForeignKey("CreatedById");

        builder.HasOne(g => g.Currency)
               .WithMany()
               .HasForeignKey(g => g.CurrencyKey);

        builder.HasMany(g => g.Members)
               .WithOne(m => m.Group)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
