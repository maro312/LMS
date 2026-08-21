using Application.Dtos.BorrowingRequest;
using LMS.Domain.Entities;
using LMS.Domain.Enums;
using System;

namespace Application.Mappings;

public static class BorrowingRequestMapping
{
    public static BorrowingRequest ToEntity(this RequestBorrow dto)
    {
        if (dto == null) return null!;

        return new BorrowingRequest
        {
            BookId = dto.BookId,
            BorrowingPeriodDays = dto.BorrowingPeriodDays,
            RequestedAt = dto.RequestedAt,
            Status = BorrowingRequestStatus.Pending
            
        };
    }

    public static BorrowingRequest UpdateEntity(this ReviewRequest dto, BorrowingRequest entity)
    {
        if (dto == null || entity == null) return entity!;

        entity.Status = dto.Status;
        entity.DenyReason = (dto.Status == BorrowingRequestStatus.Denied) ? dto.DenyReason : null;

        return entity;
    }

    public static BorrwingRequestsDto ToDto(this BorrowingRequest entity)
    {
        if (entity == null) return null!;

        return new BorrwingRequestsDto
        {
            Id = entity.Id,
            BookId = entity.BookId,
            Book = entity.Book.ToDto(),
            Requester = entity.Requester.ToDto(),
            Reviewer = entity.Reviewer.ToDto(),
            UserId = entity.UserId,
            Status = entity.Status,
            BorrowingPeriodDays = entity.BorrowingPeriodDays,
            RequestedAt = entity.RequestedAt,
            ReviewedAt = entity.ReviewedAt,
            ReviewedBy = entity.ReviewedBy,
            DenyReason = entity.DenyReason,
            CreatedDate = entity.CreatedDate,
            CreatedBy = entity.CreatedBy,
            UpdatedDate = entity.UpdatedDate,
            ModifiedBy = entity.ModifiedBy
        };
    }
}
