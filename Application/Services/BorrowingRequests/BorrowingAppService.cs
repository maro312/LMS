using Application.Contracts.BorrowingRequest;
using Application.Contracts.Users;
using Application.Dtos.BorrowingRequest;
using Application.Mappings;
using LMS.Core.Results;
using LMS.Domain.Entities;
using LMS.Domain.Repositories;
using LMS.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.BorrowingRequests;

public class BorrowingAppService : IBorrowingAppService
{
    private readonly IGenericRepository<BorrowingRequest, Guid> _repository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserAppService _userAppService;

    public BorrowingAppService(IGenericRepository<BorrowingRequest, Guid> repository,
        ICurrentUserService currentUserService,
        IUserAppService userAppService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
        _userAppService = userAppService;
    }

    public async Task<Result<BorrwingRequestsDto>> GetByIdAsync(Guid id)
    {
        try
        {
            var query = _repository.GetAllQuerable();
            BorrowingRequest entity = await query.Where(x => x.Id == id)
                .Include(x => x.Book)
                .Include(x => x.Requester)
                .Include(x => x.Reviewer)
                .FirstOrDefaultAsync();
            if (entity == null)
                return Result<BorrwingRequestsDto>.NotFound("Borrowing request not found.");

            return Result<BorrwingRequestsDto>.Success(entity.ToDto());
        }
        catch (Exception ex)
        {
            return Result<BorrwingRequestsDto>.BadRequest(ex.Message);
        }
    }

    public async Task<Result<List<BorrwingRequestsDto>>> GetAllAsync()
    {
        try
        {
            var query =  _repository.GetAllQuerable();
            List<BorrowingRequest> entities = await query
                .Include(x => x.Book)
                .Include(x => x.Requester)
                .Include(x => x.Reviewer)
                .ToListAsync();
            var dtos = entities.Select(e => e.ToDto()).ToList();
            return Result<List<BorrwingRequestsDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Result<List<BorrwingRequestsDto>>.BadRequest(ex.Message);
        }
    }

    public async Task<Result<string>> RequestBookBorrow(RequestBorrow dto)
    {
        try
        {
            var entity = dto.ToEntity();
            if (Guid.TryParse(_currentUserService.UserId, out var IdentityuserId))
            {
                var userDto = await _userAppService.GetByIdentityIdAsync(IdentityuserId);
                if (userDto == null)
                {
                    return Result<string>.NotFound("User not found.");
                }
                var userId = userDto.Id;
                entity.UserId = userId;
            }
            else
            {
                throw new Exception("Invalid user ID.");
            }
            entity.CreatedBy = _currentUserService.UserId;
            entity.CreatedDate = DateTime.UtcNow;
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return Result<string>.Success("Borrow Requested Sucessfully");
        }
        catch (Exception ex) 
        { 
            return Result<string>.BadRequest(ex.Message);
        }
    }

    public async Task<Result<string>> ReviewBorrowingRequest(ReviewRequest dto, Guid requestId)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(requestId);
            if (entity == null)
                return Result<string>.NotFound("Borrowing request not found.");

            dto.UpdateEntity(entity);
            
            entity.ModifiedBy = _currentUserService.UserId;
            entity.UpdatedDate = DateTime.UtcNow;
            entity.ReviewedAt = DateTime.UtcNow;

            if (Guid.TryParse(_currentUserService.UserId, out var IdentityuserId))
            {
                var userDto = await _userAppService.GetByIdentityIdAsync(IdentityuserId);
                if (userDto == null)
                {
                    return Result<string>.NotFound("User not found.");
                }
                var userId = userDto.Id;
                entity.UserId = userId;
            }
            else
            {
                throw new Exception("Invalid user ID.");
            }

            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();

            return Result<string>.Success($"Borrowing request {dto.Status.ToString().ToLower()} successfully.");
        }
        catch (Exception ex)
        {
            return Result<string>.BadRequest(ex.Message);
        }
    }
}
