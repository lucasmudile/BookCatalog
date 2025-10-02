namespace BookCatalog.Domain.Abstractions;

public interface IEntity<T> : IEntity
{
    T Id { get; set; }
}

public interface IEntity
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
}