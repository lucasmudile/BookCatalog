using BookCatalog.Domain.Abstractions;

namespace BookCatalog.Application.Interfaces;

public interface IBaseCrudService<TCreateDto, TUpdateDto, TViewModel, TId>
{
    Task<PagedResult<TViewModel>> GetAllAsync(PagedParameters parameters);
    Task<TViewModel?> GetByIdAsync(TId id);
    Task<TViewModel> AddAsync(TCreateDto createDto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TId id, TUpdateDto updateDto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(TId id, CancellationToken cancellationToken = default);
}