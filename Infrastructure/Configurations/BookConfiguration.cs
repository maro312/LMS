using LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations;

/// <summary>
/// Entity Framework Core configuration for <see cref="Book"/> entity.
/// </summary>
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(b => b.Author)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(b => b.Isbn)
            .HasMaxLength(20);

        builder.Property(b => b.IsAvailable)
            .HasDefaultValue(true);

        builder.Property(b => b.TotalCopies)
            .IsRequired();

        builder.Property(b => b.AvailableCopies)
            .IsRequired();

        builder.Property(b => b.CreatedBy)
            .HasMaxLength(100);

        builder.Property(b => b.ModifiedBy)
            .HasMaxLength(100);

        builder.HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(b => b.BookLanguage)
            .WithMany()
            .HasForeignKey(b => b.BookLanguageId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
