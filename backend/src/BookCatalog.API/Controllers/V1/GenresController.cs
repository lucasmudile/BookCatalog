using Asp.Versioning;
using BookCatalog.Application.DTOs.Genre;
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
public class GenresController(IGenreSerivce genreService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<GenreViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PagedResult<GenreViewModel>>> GetAllAsync(
        [FromQuery] PagedParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await genreService.GetAllAsync(parameters);
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetGenreById")]
    [ProducesResponseType(typeof(GenreViewModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<GenreViewModel>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await genreService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("with-books")]
    [ProducesResponseType(typeof(PagedResult<GenreViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PagedResult<GenreViewModel>>> GetAllWithBooksAsync(
        [FromQuery] PagedParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await genreService.GetAllWithBooksAsync(parameters, cancellationToken);
        return Ok(result);
    }


    [HttpPost]
    [ProducesResponseType(typeof(GenreViewModel), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.UnprocessableEntity)]
    public async Task<ActionResult<GenreViewModel>> CreateAsync(
        [FromBody] CreateGenreDto createDto,
        CancellationToken cancellationToken = default)
    {
        var result = await genreService.AddAsync(createDto, cancellationToken);
        return CreatedAtRoute(
            "GetGenreById",
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
        [FromBody] UpdateGenreDto updateDto,
        CancellationToken cancellationToken = default)
    {
        await genreService.UpdateAsync(id, updateDto, cancellationToken);
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
        await genreService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<GenreViewModel>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ProblemDetails), (int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<PagedResult<GenreViewModel>>> GetAllByNameAsync(
        [FromQuery] string name,
        [FromQuery] PagedParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var result = await genreService.SearchByNameAsync(name, parameters, cancellationToken);
        return Ok(result);
    }
}