using IOU1.Domain.Entities.Notifications;
using IOU1.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IOU1.Persistance.Context.Configurations;

public sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("AppNotification");

        builder.HasKey(mb => mb.Id);

        builder.Property(mb => mb.CreatedAt)
               .IsRequired();

        builder.Property(mb => mb.Payload);

        builder.Property(mb => mb.Type)
               .IsRequired()
               .HasConversion(
                    v => v.ToString(),
                    v => Enum.Parse<NotificationType>(v));

        builder.Property(mb => mb.Type)
               .IsRequired()
               .HasColumnName("NotificationType");

        builder.Property(mb => mb.Version)
               .IsRowVersion();

        builder.HasOne(mb => mb.User)
               .WithMany()
               .HasForeignKey(mb => mb.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
