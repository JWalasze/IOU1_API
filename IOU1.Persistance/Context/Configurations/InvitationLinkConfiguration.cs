using IOU1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public class InvitationLinkConfiguration : IEntityTypeConfiguration<InvitationLink>
{
    public void Configure(EntityTypeBuilder<InvitationLink> builder)
    {
        builder.ToTable("InvitationLink");

        builder.HasKey(il => il.Id);

        builder.HasOne(il => il.Group)
               .WithMany()
               .IsRequired();

        builder.Property(il => il.AddDate)
               .IsRequired();

        builder.ComplexProperty(cp => cp.InvitationKey, cp =>
        {
            cp.Property(ik => ik.Key)
              .HasColumnName("InvitationKey")
              .IsRequired();
        });

        builder.ComplexProperty(cp => cp.ExpirationDate, cp =>
        {
            cp.Property(ed => ed.ExpirationDate)
              .HasColumnName("ExpirationDate")
              .IsRequired();
        });
    }
}
