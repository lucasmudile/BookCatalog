using Asp.Versioning;
using BookCatalog.Application.DTOs.Author;
using BookCatalog.Application.Interfaces;
using BookCatalog.Application.ViewModel;
using BookCatalog.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookCatalog.API.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class AuthorsController(IAuthorService authorService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<AuthorViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PagedResult<AuthorViewModel>>> GetAllAsync(
        [FromQuery] PagedParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await authorService.GetAllAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetAuthorById")]
    [ProducesResponseType(typeof(AuthorViewModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<AuthorViewModel>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await authorService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("with-books")]
    [ProducesResponseType(typeof(PagedResult<AuthorViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PagedResult<AuthorViewModel>>> GetAllWithBooksAsync(
        [FromQuery] PagedParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await authorService.GetAllWithBooksAsync(parameters, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}/with-books")]
    [ProducesResponseType(typeof(AuthorViewModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<AuthorViewModel>> GetByIdWithBooksAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await authorService.GetByIdWithBooksAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AuthorViewModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.UnprocessableEntity)]
    public async Task<ActionResult<AuthorViewModel>> CreateAsync(
        [FromBody] CreateAuthorDto createDto,
        CancellationToken cancellationToken = default)
    {
        var result = await authorService.AddAsync(createDto, cancellationToken);
        return CreatedAtRoute(
            "GetAuthorById",
            new { version = "1.0", id = result.Id },
            result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.UnprocessableEntity)]
    public async Task<IActionResult> UpdateAsync(
        Guid id,
        [FromBody] UpdateAuthorDto updateDto,
        CancellationToken cancellationToken = default)
    {
        await authorService.UpdateAsync(id, updateDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await authorService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<AuthorViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PagedResult<AuthorViewModel>>> GetAllByNameAsync(
        [FromQuery] string name,
        [FromQuery] PagedParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await authorService.SearchByNameAsync(name, parameters, cancellationToken);
        return Ok(result);
    }
}