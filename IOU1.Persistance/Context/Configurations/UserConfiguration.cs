using IOU1.Domain.Entities;
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
               .HasConversion(v => Convert.FromBase64String(v), v => Convert.ToBase64String(v))
               .HasMaxLength(User.HashBytesMaxLength)
               .IsRequired();

        builder.Property(u => u.PasswordSalt)
               .HasColumnName("PasswordSalt")
               .HasConversion(v => Convert.FromBase64String(v), v => Convert.ToBase64String(v))
               .HasMaxLength(User.SaltBytesMaxLength)
               .IsRequired();

        builder.Property(u => u.CreatedAt)
               .HasColumnName("AddDate")
               .IsRequired();

        builder.Property(u => u.IsDeleted)
               .HasColumnName("IsDeleted")
               .IsRequired();

        builder.Property(u => u.Version)
               .IsRowVersion();

        builder.HasMany(u => u.OwnedGroups)
               .WithOne(g => g.Owner);

        builder.HasMany(u => u.MemberGroups)
               .WithOne(gm => gm.User);

        builder.OwnsOne(u => u.Email, eb =>
        {
            eb.Property(e => e.EmailAddress)
              .HasColumnName("Email")
              .IsRequired();
        });
    }
}
