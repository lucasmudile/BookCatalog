using BookCatalog.Application.DTOs.Genre;
using BookCatalog.Application.ViewModel;
using BookCatalog.Domain.Abstractions;

namespace BookCatalog.Application.Interfaces;

public interface IGenreSerivce : IBaseCrudService<CreateGenreDto, UpdateGenreDto, GenreViewModel, Guid>
{
    Task<PagedResult<GenreViewModel>> SearchByNameAsync(string name, PagedParameters parameters, CancellationToken cancellationToken = default);
    Task<PagedResult<GenreViewModel>> GetAllWithBooksAsync(PagedParameters parameters, CancellationToken cancellationToken = default);
}
