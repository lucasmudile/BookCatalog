namespace BookCatalog.Application.DTOs.Genre;

public record CreateGenreDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
