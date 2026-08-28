using Application.Contracts.Lookups;
using Application.Dtos.Lookups;
using Application.Mappings;
using LMS.Core.Results;
using LMS.Domain.Lookups;
using LMS.Domain.Repositories;
using LMS.Infrastructure.Services;

namespace Application.Services.Lookups;

public class BookLanguageAppService : IBookLanguageAppService
{
    private readonly IGenericRepository<BookLanguage, Guid> _bookLanguageRepository;
    private readonly ICurrentUserService _currentUserService;

    public BookLanguageAppService(IGenericRepository<BookLanguage, Guid> bookLanguageRepository,
        ICurrentUserService currentUserService)
    {
        _bookLanguageRepository = bookLanguageRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BookLanguageDto>> CreateAsync(CreateUpdateBookLanguageDto input)
    {
        if (input == null)
        {
            return Result<BookLanguageDto>.BadRequest("BookLanguage input cannot be null.");
        }

        var bookLanguage = input.ToEntity();
        bookLanguage.CreatedBy = _currentUserService.UserId;
        bookLanguage.CreatedDate = DateTime.UtcNow;
        await _bookLanguageRepository.AddAsync(bookLanguage);
        await _bookLanguageRepository.SaveChangesAsync();

        return Result<BookLanguageDto>.Created(bookLanguage.ToDto());
    }

    public async Task<Result<BookLanguageDto>> GetByIdAsync(Guid id)
    {
        var bookLanguage = await _bookLanguageRepository.GetByIdAsync(id);
        if (bookLanguage == null)
        {
            return Result<BookLanguageDto>.NotFound($"BookLanguage with ID '{id}' was not found.");
        }

        return Result<BookLanguageDto>.Success(bookLanguage.ToDto());
    }

    public async Task<Result<IEnumerable<BookLanguageDto>>> GetAllAsync()
    {
        var bookLanguages = await _bookLanguageRepository.GetAllAsync();
        var dtos = bookLanguages.Select(c => c.ToDto()).ToList();

        return Result<IEnumerable<BookLanguageDto>>.Success(dtos);
    }

    public async Task<Result<BookLanguageDto>> UpdateAsync(CreateUpdateBookLanguageDto input, Guid id)
    {
        if (input == null)
        {
            return Result<BookLanguageDto>.BadRequest("BookLanguage input cannot be null.");
        }

        var bookLanguage = await _bookLanguageRepository.GetByIdAsync(id);
        if (bookLanguage == null)
        {
            return Result<BookLanguageDto>.NotFound($"BookLanguage with ID '{id}' was not found.");
        }

        input.UpdateEntity(bookLanguage);

        await _bookLanguageRepository.UpdateAsync(bookLanguage);
        await _bookLanguageRepository.SaveChangesAsync();

        return Result<BookLanguageDto>.Success(bookLanguage.ToDto());
    }

    public async Task<Result<BookLanguageDto>> DeleteAsync(Guid id)
    {
        var bookLanguage = await _bookLanguageRepository.GetByIdAsync(id);
        if (bookLanguage == null)
        {
            return Result<BookLanguageDto>.NotFound($"BookLanguage with ID '{id}' was not found.");
        }

        await _bookLanguageRepository.DeleteAsync(bookLanguage);
        await _bookLanguageRepository.SaveChangesAsync();

        return Result<BookLanguageDto>.Success(bookLanguage.ToDto());
    }
}
