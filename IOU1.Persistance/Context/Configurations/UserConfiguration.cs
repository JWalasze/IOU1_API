using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Context.Configurations;

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

        builder.Property(u => u.HashedPassword)
               .HasColumnName("Password")
               .IsRequired();

        builder.Property(u => u.CreatedAt)
               .HasColumnName("AddDate")
               .IsRequired();

        builder.HasMany(u => u.OwnedGroups)
               .WithOne(g => g.Owner);

        builder.HasMany(u => u.MemberGroups)
               .WithOne(gm => gm.User);

        builder.ComplexProperty(u => u.Email, u =>
        {
            u.Property(e => e.EmailAddress)
             .HasColumnName("Email")
             .IsRequired();
        });
    }
}
