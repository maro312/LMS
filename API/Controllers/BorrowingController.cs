using Application.Contracts.BorrowingRequest;
using Application.Dtos.BorrowingRequest;
using LMS.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BorrowingController : ControllerBase
{
    private readonly IBorrowingAppService _borrowingAppService;

    public BorrowingController(IBorrowingAppService borrowingAppService)
    {
        _borrowingAppService = borrowingAppService;
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _borrowingAppService.GetAllAsync();
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _borrowingAppService.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result);
    }

    [HttpPost("request")]
    [Authorize]
    public async Task<IActionResult> RequestBorrow([FromBody] RequestBorrow dto)
    {
        var result = await _borrowingAppService.RequestBookBorrow(dto);
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result);
    }

    [HttpPost("{requestId}/review")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> ReviewRequest([FromRoute] Guid requestId, [FromBody] ReviewRequest dto)
    {
        var result = await _borrowingAppService.ReviewBorrowingRequest(dto, requestId);
        if (result.IsSuccess)
            return Ok(result);
            
        return BadRequest(result);
    }
}
