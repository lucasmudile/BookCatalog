using BookCatalog.Domain.Abstractions;

namespace BookCatalog.Domain.Entities;

public class Genre : Entity<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
