using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations;

/// <summary>
/// Entity Framework Core configuration for <see cref="User"/> entity.
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.IdentityUserId)
            .IsRequired();

        builder.HasIndex(u => u.IdentityUserId)
            .IsUnique();

        builder.HasOne(u => u.IdentityUser)
            .WithOne()
            .HasForeignKey<User>(u => u.IdentityUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.IsActive)
            .HasDefaultValue(true);

        builder.Property(u => u.CreatedBy)
            .HasMaxLength(100);

        builder.Property(u => u.ModifiedBy)
            .HasMaxLength(100);
    }
}
