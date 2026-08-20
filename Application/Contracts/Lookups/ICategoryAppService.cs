using Application.Dtos.Lookups;
using Core.Contracts.Cruds;
using LMS.Core.Results;

namespace Application.Contracts.Lookups;

public interface ICategoryAppService : ICrudAppService<CreateUpdateCategoryDto, Result<CategoryDto>, Guid, Result<IEnumerable<CategoryDto>>>
{
}

