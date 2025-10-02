using BookCatalog.Domain.Abstractions;

namespace BookCatalog.Domain.Entities;

public class Author : Entity<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }
    public string? Biography { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}