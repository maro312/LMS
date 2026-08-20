using Application.Contracts.Books;
using Application.Dtos.Books;
using LMS.Core.Results;
using LMS.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookController : ControllerBase
{
    private readonly IBookAppService _bookAppService;

    public BookController(IBookAppService bookAppService)
    {
        _bookAppService = bookAppService;
    }
    [Authorize(Roles = UserRoles.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(Result<BookDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<BookDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<BookDto>> Create([FromForm] CreateUpdateBookDto dto)
    {
        return await _bookAppService.CreateAsync(dto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<BookDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<IEnumerable<BookDto>>), StatusCodes.Status400BadRequest)]
    public async Task<Result<IEnumerable<BookDto>>> GetAll()
    {
        return await _bookAppService.GetAllAsync();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<BookDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<BookDto>> GetById(Guid id)
    {
        return await _bookAppService.GetByIdAsync(id);
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<BookDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<BookDto>> Update(Guid id, [FromForm] CreateUpdateBookDto dto)
    {
        return await _bookAppService.UpdateAsync(dto, id);
    }

    [Authorize(Roles = UserRoles.Admin)]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result<BookDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<BookDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<BookDto>> Delete(Guid id)
    {
        return await _bookAppService.DeleteAsync(id);
    }
}
