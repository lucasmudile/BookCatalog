using BookCatalog.Application.DTOs.Book;
using BookCatalog.Application.ViewModel;
using BookCatalog.Domain.Abstractions;

namespace BookCatalog.Application.Interfaces;

public interface IBookService : IBaseCrudService<CreateBookDto, UpdateBookDto, BookViewModel, Guid>
{
    Task<PagedResult<BookViewModel>> SearchByNameAsync(string name, PagedParameters parameters, CancellationToken cancellationToken = default);
    Task<PagedResult<BookViewModel>> GetByAuthorIdAsync(Guid authorId, PagedParameters parameters);
    Task<PagedResult<BookViewModel>> GetByGenreIdAsync(Guid genreId, PagedParameters parameters);
}
