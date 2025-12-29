using IOU1.Domain.Entities;
using IOU1.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("AppUser");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
               .IsRequired();

        builder.Property(u => u.LastName)
               .IsRequired();

        builder.Property(u => u.Login)
               .IsRequired();

        builder.Property(u => u.PasswordHash)
               .HasColumnName("PasswordHash")
               .IsRequired();

        builder.Property(u => u.PasswordSalt)
               .HasColumnName("PasswordSalt")
               .IsRequired();

        builder.Property(u => u.CreatedAt)
               .HasColumnName("AddDate")
               .IsRequired();

        builder.HasMany(u => u.OwnedGroups)
               .WithOne(g => g.Owner);

        builder.HasMany(u => u.MemberGroups)
               .WithOne(gm => gm.User);

        builder.Property(u => u.Email)
               .HasConversion(
                   v => v.EmailAddress,
                   v => new Email(v))
               .HasColumnName("Email")
               .IsRequired();
    }
}
