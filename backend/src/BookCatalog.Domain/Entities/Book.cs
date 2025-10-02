using BookCatalog.Domain.Abstractions;

namespace BookCatalog.Domain.Entities;

public class Book : Entity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string? Subtitle { get; set; }
    public string? Description { get; set; }
    public DateTime? PublishedDate { get; set; }
    public string? ISBN { get; set; }
    public int? PageCount { get; set; }
    public string? Publisher { get; set; }

    public Guid AuthorId { get; set; }
    public Author Author { get; set; } = default!;
    public Guid GenreId { get; set; }
    public Genre Genre { get; set; } = default!;
}
