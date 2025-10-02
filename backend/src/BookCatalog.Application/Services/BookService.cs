using BookCatalog.Application.DTOs.Book;
using BookCatalog.Application.Interfaces;
using BookCatalog.Application.ViewModel;
using BookCatalog.Domain.Abstractions;
using BookCatalog.Domain.Entities;
using BookCatalog.Shared.Exceptions;
using FluentValidation;
using Mapster;

namespace BookCatalog.Application.Services;

public class BookService(
    IRepository<Book, Guid> repository,
    IValidator<CreateBookDto> createValidator,
    IValidator<UpdateBookDto> updateValidator) : IBookService
{
    public async Task<PagedResult<BookViewModel>> GetAllAsync(PagedParameters parameters)
    {
        var pagedBooks = await repository.GetAllAsync(parameters, b => b.Author, b => b.Genre);

        return pagedBooks.Adapt<PagedResult<BookViewModel>>();
    }

    public async Task<BookViewModel?> GetByIdAsync(Guid id)
    {
        var book = await repository.GetByIdAsync(id, b => b.Author, b => b.Genre);

        if (book is null)
            throw new NotFoundException($"Livro com ID {id} não encontrado.");

        return book.Adapt<BookViewModel>();
    }


    public async Task<PagedResult<BookViewModel>> GetByAuthorIdAsync(Guid authorId, PagedParameters parameters)
    {
        var books = await repository.FindAsync(parameters, b => b.AuthorId == authorId, b => b.Author, b => b.Genre);

        if (books is null)
            throw new NotFoundException($"Nenhum livro encontrado para o autor com ID {authorId}.");

        return books.Adapt<PagedResult<BookViewModel>>();
    }

    public async Task<PagedResult<BookViewModel>> GetByGenreIdAsync(Guid genreId, PagedParameters parameters)
    {
        var books = await repository.FindAsync(parameters, b => b.GenreId == genreId, b => b.Author, b => b.Genre);

        if (books is null)
            throw new NotFoundException($"Nenhum livro encontrado para o género com ID {genreId}.");

        return books.Adapt<PagedResult<BookViewModel>>(); 
    }

    public async Task<BookViewModel> AddAsync(CreateBookDto createDto, CancellationToken cancellationToken = default)
    {
        var validationResult = await createValidator.ValidateAsync(createDto, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var book = createDto.Adapt<Book>();
        await repository.AddAsync(book, cancellationToken);

        var savedBook = await repository.GetByIdAsync(book.Id, b => b.Author, b => b.Genre);

        return savedBook.Adapt<BookViewModel>() with
        {
            AuthorName = $"{savedBook?.Author?.FirstName} {savedBook?.Author?.LastName}".Trim(),
            GenreName = savedBook?.Genre?.Name ?? string.Empty
        };
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var book = await repository.DeleteAsync(id, cancellationToken);
        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateBookDto updateDto, CancellationToken cancellationToken = default)
    {
        if (id != updateDto.Id)
            throw new BadRequestException($"Os Id são diferentes ");

        var book = await repository.SingleOrDefaultAsync(b => b.Id == id);
        if (book is null)
            throw new NotFoundException($"Livro com ID {id} não encontrado.");

        var validationResult = await updateValidator.ValidateAsync(updateDto, cancellationToken);
        
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var updatedBook = updateDto.Adapt(book);
        var result = await repository.UpdateAsync(updatedBook, cancellationToken);
        return result;
    }
    public async Task<PagedResult<BookViewModel>> SearchByNameAsync(string name, PagedParameters parameters, CancellationToken cancellationToken)
    {
        var normalizedName = name.ToLower();
        var books = await repository
            .GetByFilterAsync(b => b.Title
                                .ToLower()
                                .Contains(normalizedName), parameters,  b => b.Author, b => b.Genre);

        return books.Adapt<PagedResult<BookViewModel>>();
    }
}