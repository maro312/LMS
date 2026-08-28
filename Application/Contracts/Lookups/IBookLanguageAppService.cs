using Application.Dtos.Lookups;
using Core.Contracts.Cruds;
using LMS.Core.Results;

namespace Application.Contracts.Lookups;

public interface IBookLanguageAppService : ICrudAppService<CreateUpdateBookLanguageDto, Result<BookLanguageDto>, Guid, Result<IEnumerable<BookLanguageDto>>>
{
}
