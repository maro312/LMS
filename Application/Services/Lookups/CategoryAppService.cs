using Application.Contracts.Lookups;
using Application.Dtos.Lookups;
using Application.Mappings;
using LMS.Core.Results;
using LMS.Domain.Lookups;
using LMS.Domain.Repositories;
using LMS.Infrastructure.Services;

namespace Application.Services.Lookups;

public class CategoryAppService : ICategoryAppService
{
    private readonly IGenericRepository<Category, Guid> _categoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public CategoryAppService(IGenericRepository<Category, Guid> categoryRepository,
        ICurrentUserService currentUserService)
    {
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<CategoryDto>> CreateAsync(CreateUpdateCategoryDto input)
    {
        if (input == null)
        {
            return Result<CategoryDto>.BadRequest("Category input cannot be null.");
        }

        var category = input.ToEntity();
        category.CreatedBy = _currentUserService.UserId;
        category.CreatedDate = DateTime.UtcNow;
        await _categoryRepository.AddAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return Result<CategoryDto>.Created(category.ToDto());
    }

    public async Task<Result<CategoryDto>> GetByIdAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return Result<CategoryDto>.NotFound($"Category with ID '{id}' was not found.");
        }

        return Result<CategoryDto>.Success(category.ToDto());
    }

    public async Task<Result<IEnumerable<CategoryDto>>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var dtos = categories.Select(c => c.ToDto()).ToList();

        return Result<IEnumerable<CategoryDto>>.Success(dtos);
    }

    public async Task<Result<CategoryDto>> UpdateAsync(CreateUpdateCategoryDto input, Guid id)
    {
        if (input == null)
        {
            return Result<CategoryDto>.BadRequest("Category input cannot be null.");
        }

        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return Result<CategoryDto>.NotFound($"Category with ID '{id}' was not found.");
        }

        input.UpdateEntity(category);

        await _categoryRepository.UpdateAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return Result<CategoryDto>.Success(category.ToDto());
    }

    public async Task<Result<CategoryDto>> DeleteAsync(Guid id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return Result<CategoryDto>.NotFound($"Category with ID '{id}' was not found.");
        }

        await _categoryRepository.DeleteAsync(category);
        await _categoryRepository.SaveChangesAsync();

        return Result<CategoryDto>.Success(category.ToDto());
    }
}

