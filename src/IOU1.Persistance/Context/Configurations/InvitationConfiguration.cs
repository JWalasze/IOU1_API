using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("Invitation");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Version)
               .IsRowVersion();

        builder.HasOne(g => g.Group)
               .WithMany()
               .HasForeignKey("GroupId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(g => g.User)
               .WithMany()
               .HasForeignKey("UserId")
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(g => g.Sender)
               .WithMany()
               .HasForeignKey("SenderId")
               .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(i => i.InvitationStatus)
            .HasConversion<string>()
            .IsRequired();
    }
}
