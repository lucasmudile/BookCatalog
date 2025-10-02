namespace BookCatalog.Domain.Abstractions;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
    public int FirstItemIndex => (Page - 1) * PageSize + 1;
    public int LastItemIndex => Math.Min(Page * PageSize, TotalCount);
}
