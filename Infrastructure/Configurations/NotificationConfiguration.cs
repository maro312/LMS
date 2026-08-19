using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations;

/// <summary>
/// Entity Framework Core configuration for <see cref="Notification"/> entity.
/// </summary>
public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.RecipientRole)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(n => n.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(n => n.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(n => n.Message)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(n => n.IsRead)
            .HasDefaultValue(false);

        builder.Property(n => n.CreatedBy)
            .HasMaxLength(100);

        builder.Property(n => n.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(n => n.RecipientUser)
            .WithMany()
            .HasForeignKey(n => n.RecipientUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
