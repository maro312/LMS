using Application.Dtos.Lookups;
using LMS.Domain.Lookups;

namespace Application.Mappings;

public static class BookLanguageMapping
{
    /// <summary>
    /// Maps a BookLanguage domain entity to a BookLanguageDto.
    /// </summary>
    public static BookLanguageDto ToDto(this BookLanguage bookLanguage)
    {
        if (bookLanguage == null) return null!;

        return new BookLanguageDto
        {
            Id = bookLanguage.Id,
            Name = bookLanguage.Name,
            Code = bookLanguage.Code,
            Description = bookLanguage.Description,
            IsActive = bookLanguage.IsActive,
            CreatedBy = bookLanguage.CreatedBy,
            CreatedDate = bookLanguage.CreatedDate,
            ModifiedBy = bookLanguage.ModifiedBy,
            UpdatedDate = bookLanguage.UpdatedDate
        };
    }

    /// <summary>
    /// Maps a CreateUpdateBookLanguageDto to a new BookLanguage domain entity.
    /// </summary>
    public static BookLanguage ToEntity(this CreateUpdateBookLanguageDto dto)
    {
        if (dto == null) return null!;

        return new BookLanguage
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive
        };
    }

    /// <summary>
    /// Updates an existing BookLanguage domain entity with values from a CreateUpdateBookLanguageDto.
    /// </summary>
    public static BookLanguage UpdateEntity(this CreateUpdateBookLanguageDto dto, BookLanguage bookLanguage)
    {
        if (dto == null || bookLanguage == null) return bookLanguage!;

        bookLanguage.Name = dto.Name;
        bookLanguage.Description = dto.Description;
        bookLanguage.IsActive = dto.IsActive;

        return bookLanguage;
    }
}
