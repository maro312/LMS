using Application.Dtos.BorrowingRequest;
using Core.Contracts.Cruds;
using LMS.Core.Results;
using LMS.Domain.Enums;

namespace Application.Contracts.BorrowingRequest;

public interface IBorrowingAppService :IGetByIdAppService<Result<BorrwingRequestsDto>, Guid>,
    IGetListAppService<Result<List<BorrwingRequestsDto>>>
{
    public Task<Result<BorrwingRequestsDto>> RequestBookBorrow(RequestBorrow dto);
    public Task<Result<string>> ReviewBorrowingRequest(ReviewRequest dto, Guid requestId);
    Task<Result<List<BorrwingRequestsDto>>> GetAllAsync(BorrowingRequestStatus? status = null);
}
