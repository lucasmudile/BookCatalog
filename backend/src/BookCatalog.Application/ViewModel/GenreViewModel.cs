namespace BookCatalog.Application.ViewModel;

public record GenreViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ICollection<BookViewModel> Books { get; set; } = new List<BookViewModel>();
}
