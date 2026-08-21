using Application.Dtos.BorrowingRequest;
using Core.Contracts.Cruds;
using LMS.Core.Results;

namespace Application.Contracts.BorrowingRequest;

public interface IBorrowingAppService :IGetByIdAppService<Result<BorrwingRequestsDto>, Guid>,
    IGetListAppService<Result<List<BorrwingRequestsDto>>>
{
    public Task<Result<string>> RequestBookBorrow(RequestBorrow dto);
    public Task<Result<string>> ReviewBorrowingRequest(ReviewRequest dto, Guid requestId);
}
