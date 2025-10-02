namespace BookCatalog.Application.ViewModel;

public record AuthorViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfDeath { get; set; }
    public string? Biography { get; set; }
    public ICollection<BookViewModel> Books { get; set; } = new List<BookViewModel>();
}
