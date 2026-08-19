using Application.Dtos.Users;
using Core.Contracts.Cruds;
using LMS.Core.Results;

namespace Application.Contracts.Users;

public interface IUserAppService : ICrudAppService<CreateUpdateUserDto, Result<UserDto>, Guid, Result<IEnumerable<UserDto>>>
{
}
