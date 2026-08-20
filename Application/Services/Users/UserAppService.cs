using Application.Contracts.Users;
using Application.Dtos.Users;
using Application.Mappings;
using LMS.Core.Results;
using LMS.Domain.Entities;
using LMS.Domain.Repositories;

namespace Application.Services.Users;

public class UserAppService : IUserAppService
{
    private readonly IGenericRepository<User, Guid> _userRepository;

    public UserAppService(IGenericRepository<User, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserDto>> CreateAsync(CreateUpdateUserDto input)
    {
        if (input == null)
        {
            return Result<UserDto>.BadRequest("User input cannot be null.");
        }

        var user = input.ToEntity();
        user.CreatedBy = input.IdentityUserId.ToString();
        user.CreatedDate = DateTime.Now;
        await _userRepository.AddAsync(user);
        return Result<UserDto>.Created(user.ToDto());
    }

    public async Task<Result<UserDto>> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return Result<UserDto>.NotFound($"User with ID '{id}' was not found.");
        }

        return Result<UserDto>.Success(user.ToDto());
    }

    public async Task<Result<IEnumerable<UserDto>>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        var dtos = users.Select(u => u.ToDto()).ToList();

        return Result<IEnumerable<UserDto>>.Success(dtos);
    }

    public async Task<Result<UserDto>> UpdateAsync(CreateUpdateUserDto input, Guid id)
    {
        if (input == null)
        {
            return Result<UserDto>.BadRequest("User input cannot be null.");
        }

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return Result<UserDto>.NotFound($"User with ID '{id}' was not found.");
        }

        input.UpdateEntity(user);

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return Result<UserDto>.Success(user.ToDto());
    }

    public async Task<Result<UserDto>> DeleteAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return Result<UserDto>.NotFound($"User with ID '{id}' was not found.");
        }

        await _userRepository.DeleteAsync(user);
        await _userRepository.SaveChangesAsync();

        return Result<UserDto>.Success(user.ToDto());
    }
}
