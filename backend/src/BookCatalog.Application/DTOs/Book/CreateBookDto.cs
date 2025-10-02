namespace BookCatalog.Application.DTOs.Book;

public record CreateBookDto
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? Description { get; set; }
    public DateTime? PublishedDate { get; set; }
    public string? ISBN { get; set; }
    public int? PageCount { get; set; }
    public string? Publisher { get; set; }
    public Guid AuthorId { get; set; }
    public Guid GenreId { get; set; }
}
