using Application.Dtos.Books;
using LMS.Domain.Entities;
using System;

namespace Application.Mappings;

public static class BookMapping
{
    public static BookDto ToDto(this Book book)
    {
        if (book == null) return null!;

        return new BookDto
        {
            Id = book.Id,
            Isbn = book.Isbn,
            Title = book.Title,
            Author = book.Author,
            CategoryId = book.CategoryId,
            IsAvailable = book.IsAvailable,
            TotalCopies = book.TotalCopies,
            AvailableCopies = book.AvailableCopies,
            BookPhotoUrl = book.BookPhotoUrl,
            PublisherName = book.PublisherName,
            PublishDate = book.PublishDate,
            BookLanguageId = book.BookLanguageId,
            PageNumber = book.PageNumber,
            description = book.description,
            CreatedDate = book.CreatedDate,
            CreatedBy = book.CreatedBy,
            UpdatedDate = book.UpdatedDate,
            ModifiedBy = book.ModifiedBy
        };
    }

    public static Book ToEntity(this CreateUpdateBookDto dto)
    {
        if (dto == null) return null!;

        return new Book
        {
            Id = Guid.NewGuid(),
            Isbn = dto.Isbn,
            Title = dto.Title,
            Author = dto.Author,
            CategoryId = dto.CategoryId,
            IsAvailable = dto.IsAvailable,
            TotalCopies = dto.TotalCopies,
            AvailableCopies = dto.AvailableCopies,
            PublisherName = dto.PublisherName,
            PublishDate = dto.PublishDate,
            BookLanguageId = dto.BookLanguageId,
            PageNumber = dto.PageNumber,
            description = dto.description
        };
    }

    public static Book UpdateEntity(this CreateUpdateBookDto dto, Book book)
    {
        if (dto == null || book == null) return book!;

        book.Isbn = dto.Isbn;
        book.Title = dto.Title;
        book.Author = dto.Author;
        book.CategoryId = dto.CategoryId;
        book.IsAvailable = dto.IsAvailable;
        book.TotalCopies = dto.TotalCopies;
        book.AvailableCopies = dto.AvailableCopies;
        book.PublisherName = dto.PublisherName;
        book.PublishDate = dto.PublishDate;
        book.BookLanguageId = dto.BookLanguageId;
        book.PageNumber = dto.PageNumber;
        book.description = dto.description;

        return book;
    }
}
