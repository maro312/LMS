using Application.Contracts.Lookups;
using Application.Dtos.Lookups;
using LMS.Core.Results;
using LMS.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Route("api/lookup/[controller]")]
[Authorize(Roles = UserRoles.Admin)]
public class BookLanguageController : ControllerBase
{
    private readonly IBookLanguageAppService _bookLanguageAppService;

    public BookLanguageController(IBookLanguageAppService bookLanguageAppService)
    {
        _bookLanguageAppService = bookLanguageAppService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<BookLanguageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<BookLanguageDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<BookLanguageDto>> Create([FromBody] CreateUpdateBookLanguageDto dto)
    {
        return await _bookLanguageAppService.CreateAsync(dto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<BookLanguageDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<IEnumerable<BookLanguageDto>>), StatusCodes.Status400BadRequest)]
    public async Task<Result<IEnumerable<BookLanguageDto>>> GetAll()
    {
        return await _bookLanguageAppService.GetAllAsync();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<BookLanguageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<BookLanguageDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<BookLanguageDto>> GetById(Guid id)
    {
        return await _bookLanguageAppService.GetByIdAsync(id);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result<BookLanguageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<BookLanguageDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<BookLanguageDto>> Update(Guid id, [FromBody] CreateUpdateBookLanguageDto dto)
    {
        return await _bookLanguageAppService.UpdateAsync(dto, id);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result<BookLanguageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<BookLanguageDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<BookLanguageDto>> Delete(Guid id)
    {
        return await _bookLanguageAppService.DeleteAsync(id);
    }
}
