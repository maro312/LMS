using Application.Dtos.Lookups;
using LMS.Domain.Lookups;

namespace Application.Mappings;

public static class CategoryMapping
{
    /// <summary>
    /// Maps a Category domain entity to a CategoryDto.
    /// </summary>
    public static CategoryDto ToDto(this Category category)
    {
        if (category == null) return null!;

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Code = category.Code,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedBy = category.CreatedBy,
            CreatedDate = category.CreatedDate,
            ModifiedBy = category.ModifiedBy,
            UpdatedDate = category.UpdatedDate
        };
    }

    /// <summary>
    /// Maps a CreateUpdateCategoryDto to a new Category domain entity.
    /// </summary>
    public static Category ToEntity(this CreateUpdateCategoryDto dto)
    {
        if (dto == null) return null!;

        return new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            IsActive = dto.IsActive
        };
    }

    /// <summary>
    /// Updates an existing Category domain entity with values from a CreateUpdateCategoryDto.
    /// </summary>
    public static Category UpdateEntity(this CreateUpdateCategoryDto dto, Category category)
    {
        if (dto == null || category == null) return category!;

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.IsActive = dto.IsActive;

        return category;
    }
}
