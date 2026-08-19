using Application.Dtos.Users;
using LMS.Domain.Entities;

namespace Application.Mappings;

public static class UserMapping
{
    /// <summary>
    /// Maps a User domain entity to a UserDto.
    /// </summary>
    public static UserDto ToDto(this User user)
    {
        if (user == null) return null!;

        return new UserDto
        {
            Id = user.Id,
            IdentityUserId = user.IdentityUserId,
            IdentityUser = user.IdentityUser,
            FullName = user.FullName,
            IsActive = user.IsActive,
            CreatedDate = user.CreatedDate,
            CreatedBy = user.CreatedBy,
            UpdatedDate = user.UpdatedDate,
            ModifiedBy = user.ModifiedBy
        };
    }

    /// <summary>
    /// Maps a CreateUpdateUserDto to a new User domain entity.
    /// </summary>
    public static User ToEntity(this CreateUpdateUserDto dto)
    {
        if (dto == null) return null!;

        return new User
        {
            Id = Guid.NewGuid(),
            IdentityUserId = dto.IdentityUserId,
            FullName = dto.FullName,
            IsActive = dto.IsActive
        };
    }

    /// <summary>
    /// Updates an existing User domain entity with values from a CreateUpdateUserDto.
    /// </summary>
    public static User UpdateEntity(this CreateUpdateUserDto dto, User user)
    {
        if (dto == null || user == null) return user!;

        user.FullName = dto.FullName;
        user.IsActive = dto.IsActive;
        user.IdentityUserId = dto.IdentityUserId;

        return user;
    }
}
