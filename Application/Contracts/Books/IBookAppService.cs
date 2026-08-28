using Application.Dtos.Books;
using Core.Contracts.Cruds;
using LMS.Core.Results;

namespace Application.Contracts.Books;

public interface IBookAppService : ICrudAppService<CreateUpdateBookDto, Result<BookDto>, Guid, Result<IEnumerable<BookDto>>>
{
    Task<Result<PagedResult<BookDto>>> GetAllPaginatedAsync(int pageNumber, int pageSize);
    Task<Result<IEnumerable<BookDto>>> SearchAsync(string keyword);
    Task<Result<IEnumerable<BookDto>>> FilterAsync(Guid? categoryId, bool? isAvailable);
}
