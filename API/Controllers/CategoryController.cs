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
public class CategoryController : ControllerBase
{
    private readonly ICategoryAppService _categoryAppService;

    public CategoryController(ICategoryAppService categoryAppService)
    {
        _categoryAppService = categoryAppService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CategoryDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<CategoryDto>> Create([FromBody] CreateUpdateCategoryDto dto)
    {
        return await _categoryAppService.CreateAsync(dto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<CategoryDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<IEnumerable<CategoryDto>>), StatusCodes.Status400BadRequest)]
    public async Task<Result<IEnumerable<CategoryDto>>> GetAll()
    {
        return await _categoryAppService.GetAllAsync();
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Result<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CategoryDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<CategoryDto>> GetById(Guid id)
    {
        return await _categoryAppService.GetByIdAsync(id);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Result<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CategoryDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<CategoryDto>> Update(Guid id, [FromBody] CreateUpdateCategoryDto dto)
    {
        return await _categoryAppService.UpdateAsync(dto, id);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Result<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<CategoryDto>), StatusCodes.Status400BadRequest)]
    public async Task<Result<CategoryDto>> Delete(Guid id)
    {
        return await _categoryAppService.DeleteAsync(id);
    }
}
