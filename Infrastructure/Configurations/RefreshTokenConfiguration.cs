using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(rt => rt.Token)
            .IsUnique();

        builder.Property(rt => rt.JwtId)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(rt => rt.IdentityUser)
            .WithMany()
            .HasForeignKey(rt => rt.IdentityUserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
