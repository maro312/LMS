using Application.Contracts.Books;
using Application.Dtos.Books;
using Application.Mappings;
using LMS.Core.Results;
using LMS.Domain.Entities;
using LMS.Domain.Repositories;
using LMS.Infrastructure.Services;
using Application.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Books;

public class BookAppService : IBookAppService
{
    private readonly IGenericRepository<Book, Guid> _bookRepository;
    private readonly ICurrentUserService _currentUserService;
    public BookAppService(IGenericRepository<Book, Guid> bookRepository,
        ICurrentUserService currentUserService)
    {
        _bookRepository = bookRepository;
        _currentUserService = currentUserService;
    }

    public async Task<Result<BookDto>> CreateAsync(CreateUpdateBookDto input)
    {
        if (input == null)
        {
            return Result<BookDto>.BadRequest("Book input cannot be null.");
        }

        var book = input.ToEntity();
        book.CreatedBy = _currentUserService.UserId;
        book.CreatedDate = DateTime.Now;

        if (input.BookPhotoFile != null)
        {
            book.BookPhotoUrl = await FileHelper.SaveFileAsync(input.BookPhotoFile, "books");
        }

        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();
        return Result<BookDto>.Created(book.ToDto());
    }

    public async Task<Result<BookDto>> GetByIdAsync(Guid id)
    {
        var book = await _bookRepository.GetAllQuerable()
            .Include(b => b.Category)
            .Include(b => b.BookLanguage)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (book == null)
        {
            return Result<BookDto>.NotFound($"Book with ID '{id}' was not found.");
        }

        return Result<BookDto>.Success(book.ToDto());
    }

    public async Task<Result<IEnumerable<BookDto>>> GetAllAsync()
    {
        var books = await _bookRepository.GetAllQuerable()
            .Include(b => b.Category)
            .Include(b => b.BookLanguage)
            .ToListAsync();
        var dtos = books.Select(b => b.ToDto()).ToList();

        return Result<IEnumerable<BookDto>>.Success(dtos);
    }

    public async Task<Result<PagedResult<BookDto>>> GetAllPaginatedAsync(int pageNumber, int pageSize)
    {
        var query = _bookRepository.GetAllQuerable()
            .Include(b => b.Category)
            .Include(b => b.BookLanguage);

        var totalCount = await query.CountAsync();
        var books = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var dtos = books.Select(b => b.ToDto()).ToList();
        
        var pagedResult = new PagedResult<BookDto>(dtos, pageNumber, pageSize, totalCount);
        return Result<PagedResult<BookDto>>.Success(pagedResult);
    }

    public async Task<Result<PagedResult<BookDto>>> SearchAsync(string keyword, int pageNumber, int pageSize)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            return await GetAllPaginatedAsync(pageNumber, pageSize);
        }

        var lowerKeyword = keyword.ToLower();
        var query = _bookRepository.GetAllQuerable()
            .Include(b => b.Category)
            .Include(b => b.BookLanguage)
            .Where(b => 
                b.Title.ToLower().Contains(lowerKeyword) || 
                b.Author.ToLower().Contains(lowerKeyword) || 
                (b.Isbn != null && b.Isbn.ToLower().Contains(lowerKeyword)));

        var totalCount = await query.CountAsync();
        var books = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var dtos = books.Select(b => b.ToDto()).ToList();
        
        var pagedResult = new PagedResult<BookDto>(dtos, pageNumber, pageSize, totalCount);
        return Result<PagedResult<BookDto>>.Success(pagedResult);
    }

    public async Task<Result<IEnumerable<BookDto>>> FilterAsync(Guid? categoryId, bool? isAvailable)
    {
        var books = await _bookRepository.GetAllQuerable()
            .Include(b => b.Category)
            .Include(b => b.BookLanguage)
            .Where(b => 
                (!categoryId.HasValue || b.CategoryId == categoryId.Value) &&
                (!isAvailable.HasValue || b.IsAvailable == isAvailable.Value))
            .ToListAsync();

        var dtos = books.Select(b => b.ToDto()).ToList();
        return Result<IEnumerable<BookDto>>.Success(dtos);
    }

    public async Task<Result<BookDto>> UpdateAsync(CreateUpdateBookDto input, Guid id)
    {
        if (input == null)
        {
            return Result<BookDto>.BadRequest("Book input cannot be null.");
        }

        var book = await _bookRepository.GetAllQuerable()
            .Include(b => b.Category)
            .Include(b => b.BookLanguage)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (book == null)
        {
            return Result<BookDto>.NotFound($"Book with ID '{id}' was not found.");
        }

        input.UpdateEntity(book);

        if (input.BookPhotoFile != null)
        {
            book.BookPhotoUrl = await FileHelper.SaveFileAsync(input.BookPhotoFile, "books");
        }

        book.UpdatedDate = DateTime.Now;
        book.ModifiedBy = _currentUserService.UserId;

        await _bookRepository.UpdateAsync(book);
        await _bookRepository.SaveChangesAsync();

        return Result<BookDto>.Success(book.ToDto());
    }

    public async Task<Result<BookDto>> DeleteAsync(Guid id)
    {
        var book = await _bookRepository.GetAllQuerable()
            .Include(b => b.Category)
            .Include(b => b.BookLanguage)
            .FirstOrDefaultAsync(b => b.Id == id);
        if (book == null)
        {
            return Result<BookDto>.NotFound($"Book with ID '{id}' was not found.");
        }

        await _bookRepository.DeleteAsync(book);
        await _bookRepository.SaveChangesAsync();

        return Result<BookDto>.Success(book.ToDto());
    }

}
