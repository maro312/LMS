using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations;

/// <summary>
/// Entity Framework Core configuration for <see cref="BorrowingRequest"/> entity.
/// </summary>
public class BorrowingRequestConfiguration : IEntityTypeConfiguration<BorrowingRequest>
{
    public void Configure(EntityTypeBuilder<BorrowingRequest> builder)
    {
        builder.ToTable("BorrowingRequests");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.BookTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.BorrowingPeriodDays)
            .IsRequired();

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(r => r.RequestedAt)
            .IsRequired();

        builder.Property(r => r.DenyReason)
            .HasMaxLength(500);

        builder.Property(r => r.CreatedBy)
            .HasMaxLength(100);

        builder.Property(r => r.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(r => r.Book)
            .WithMany()
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Requester)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
