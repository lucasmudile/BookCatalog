using BookCatalog.Application.DTOs.Author;
using BookCatalog.Application.Interfaces;
using BookCatalog.Application.ViewModel;
using BookCatalog.Domain.Abstractions;
using BookCatalog.Domain.Entities;
using BookCatalog.Shared.Exceptions;
using FluentValidation;
using Mapster;

namespace BookCatalog.Application.Services;

public class AuthorService(
    IRepository<Author, Guid> repository,
    IValidator<CreateAuthorDto> createValidator,
    IValidator<UpdateAuthorDto> updateValidator) : IAuthorService
{
    public async Task<PagedResult<AuthorViewModel>> GetAllAsync(PagedParameters parameters)
    {
        var authors = await repository.GetAllAsync(parameters);
        return authors.Adapt<PagedResult<AuthorViewModel>>();
    }

    public async Task<AuthorViewModel?> GetByIdAsync(Guid id)
    {
        var author = await repository.GetByIdAsync(id);
        if (author is null)
            throw new NotFoundException($"Author com ID {id} não encontrado.");

        return author.Adapt<AuthorViewModel>();
    }



    public async Task<PagedResult<AuthorViewModel>> GetAllWithBooksAsync(PagedParameters parameters, CancellationToken cancellationToken = default)
    {
        var authors = await repository.GetAllAsync(parameters, a => a.Books);

        return authors.Adapt<PagedResult<AuthorViewModel>>();
    }

    public async Task<AuthorViewModel?> GetByIdWithBooksAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var author = await repository.GetByIdAsync(id, a => a.Books);

        if (author is null)
            throw new NotFoundException($"Author com ID {id} não encontrado.");

        return author.Adapt<AuthorViewModel>();
    }

    public async Task<AuthorViewModel> AddAsync(CreateAuthorDto createDto, CancellationToken cancellationToken = default)
    {
        var validationResult = await createValidator.ValidateAsync(createDto, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var author = createDto.Adapt<Author>();
        await repository.AddAsync(author, cancellationToken);

        return author.Adapt<AuthorViewModel>();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var author = await repository.DeleteAsync(id, cancellationToken);
        return true;
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateAuthorDto updateDto, CancellationToken cancellationToken = default)
    {
        if (id != updateDto.Id)
            throw new BadRequestException($"Os Id são diferentes ");

        var author = await repository.SingleOrDefaultAsync(a => a.Id == id);
        if (author is null)
            throw new NotFoundException($"Author com ID {id} não encontrado.");

        var validationResult = await updateValidator.ValidateAsync(updateDto, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var updatedAuthor = updateDto.Adapt(author);
        var result = await repository.UpdateAsync(updatedAuthor, cancellationToken);

        return result;
    }
    public async Task<PagedResult<AuthorViewModel>> SearchByNameAsync(string name, PagedParameters parameters, CancellationToken cancellationToken = default)
    {
        var normalizedName = name.ToLower();
        var authors = await repository
            .GetByFilterAsync(a =>
                a.FirstName.ToLower().Contains(normalizedName) ||
                a.LastName.ToLower().Contains(normalizedName),
                parameters, a => a.Books);

        return authors.Adapt<PagedResult<AuthorViewModel>>();
    }
}