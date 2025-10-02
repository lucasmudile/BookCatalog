using BookCatalog.Application.DTOs.Author;
using BookCatalog.Application.ViewModel;
using BookCatalog.Domain.Abstractions;

namespace BookCatalog.Application.Interfaces;

public interface IAuthorService : IBaseCrudService<CreateAuthorDto, UpdateAuthorDto, AuthorViewModel, Guid>
{
    Task<PagedResult<AuthorViewModel>> SearchByNameAsync(string name, PagedParameters parameters, CancellationToken cancellationToken = default);
    Task<PagedResult<AuthorViewModel>> GetAllWithBooksAsync(PagedParameters parameters, CancellationToken cancellationToken = default);
    Task<AuthorViewModel?> GetByIdWithBooksAsync(Guid id, CancellationToken cancellationToken = default);
}
