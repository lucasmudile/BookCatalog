using Asp.Versioning;
using BookCatalog.Application.DTOs.Book;
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
public class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<BookViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PagedResult<BookViewModel>>> GetAllAsync(
        [FromQuery] PagedParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await bookService.GetAllAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetBookById")]
    [ProducesResponseType(typeof(BookViewModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<BookViewModel>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await bookService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("author/{authorId:guid}")]
    [ProducesResponseType(typeof(PagedResult<BookViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PagedResult<BookViewModel>>> GetByAuthorIdAsync(
        Guid authorId,
        [FromQuery] PagedParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await bookService.GetByAuthorIdAsync(authorId, parameters);
        return Ok(result);
    }

    [HttpGet("genre/{genreId:guid}")]
    [ProducesResponseType(typeof(PagedResult<BookViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PagedResult<BookViewModel>>> GetByGenreIdAsync(
        Guid genreId,
        [FromQuery] PagedParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await bookService.GetByGenreIdAsync(genreId, parameters);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookViewModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.UnprocessableEntity)]
    public async Task<ActionResult<BookViewModel>> CreateAsync(
        [FromBody] CreateBookDto createDto,
        CancellationToken cancellationToken = default)
    {
        var result = await bookService.AddAsync(createDto, cancellationToken);
        return CreatedAtRoute(
            "GetBookById",
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
        [FromBody] UpdateBookDto updateDto,
        CancellationToken cancellationToken = default)
    {
        await bookService.UpdateAsync(id, updateDto, cancellationToken);
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
        await bookService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<BookViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PagedResult<BookViewModel>>> GetAllByNameAsync(
        [FromQuery] string name,
        [FromQuery] PagedParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await bookService.SearchByNameAsync(name, parameters, cancellationToken);
        return Ok(result);
    }
}