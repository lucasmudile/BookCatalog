using BookCatalog.Domain.Abstractions;
using BookCatalog.Domain.Entities;

namespace BookCatalog.Tests.Unit.Helpers;

public static class TestHelpers
{
    public static class Authors
    {
        public static Author CreateValidAuthor(string firstName = "Pepetela", string lastName = "Mayombe")
        {
            return new Author
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = new DateTime(1941, 10, 29),
                Biography = "Escritor angolano, considerado um dos maiores nomes da literatura africana de língua portuguesa",
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
        }

        public static Author CreateAuthorWithBooks(int bookCount = 2)
        {
            var author = CreateValidAuthor("José Eduardo", "Agualusa");
            var books = new List<Book>();

            for (int i = 0; i < bookCount; i++)
            {
                books.Add(Books.CreateValidBook($"Livro {i + 1}", author.Id, Genres.CreateValidGenre().Id));
            }

            author.Books = books;
            return author;
        }

        public static List<Author> CreateAuthorList(int count = 3)
        {
            var authors = new List<Author>();
            var firstNames = new[] { "Ondjaki", "Luandino", "Uanhenga", "Manuel", "Ana Paula" };
            var lastNames = new[] { "Ndalu", "Vieira", "Xitu", "Rui", "Tavares" };

            for (int i = 0; i < count && i < firstNames.Length; i++)
            {
                authors.Add(CreateValidAuthor(firstNames[i], lastNames[i]));
            }

            return authors;
        }
    }

    public static class Books
    {
        public static Book CreateValidBook(string title = "Os Transparentes", Guid? authorId = null, Guid? genreId = null)
        {
            return new Book
            {
                Id = Guid.NewGuid(),
                Title = title,
                Subtitle = "Um romance contemporâneo",
                Description = "Uma narrativa envolvente sobre a realidade angolana",
                PublishedDate = new DateTime(2012, 8, 15),
                ISBN = "978-972-1-04567-8",
                PageCount = 245,
                Publisher = "Caminho",
                AuthorId = authorId ?? Guid.NewGuid(),
                GenreId = genreId ?? Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
        }

        public static Book CreateBookWithRelations()
        {
            var author = Authors.CreateValidAuthor("Mia", "Couto");
            var genre = Genres.CreateValidGenre("Realismo Mágico");

            var book = CreateValidBook("Terra Sonâmbula", author.Id, genre.Id);
            book.Author = author;
            book.Genre = genre;

            return book;
        }

        public static List<Book> CreateBookList(int count = 3)
        {
            var books = new List<Book>();
            var titles = new[]
            {
                "Mayombe",
                "A Geração da Utopia",
                "O Cão e os Calús",
                "Yaka",
                "Lueji"
            };

            for (int i = 0; i < count && i < titles.Length; i++)
            {
                books.Add(CreateValidBook(titles[i]));
            }

            return books;
        }
    }

    public static class Genres
    {
        public static Genre CreateValidGenre(string name = "Romance Africano")
        {
            return new Genre
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = "Gênero literário que aborda narrativas da literatura africana de expressão portuguesa",
                CreatedAt = DateTime.UtcNow,
                LastModified = DateTime.UtcNow
            };
        }

        public static Genre CreateGenreWithBooks(int bookCount = 2)
        {
            var genre = CreateValidGenre("Literatura de Resistência");
            var books = new List<Book>();

            for (int i = 0; i < bookCount; i++)
            {
                books.Add(Books.CreateValidBook($"Livro de Resistência {i + 1}", Guid.NewGuid(), genre.Id));
            }

            genre.Books = books;
            return genre;
        }

        public static List<Genre> CreateGenreList(int count = 3)
        {
            var genres = new List<Genre>();
            var names = new[]
            {
                "Ficção Histórica",
                "Poesia Contemporânea",
                "Literatura Lusófona",
                "Crônica Social",
                "Drama Moderno"
            };

            for (int i = 0; i < count && i < names.Length; i++)
            {
                genres.Add(CreateValidGenre(names[i]));
            }

            return genres;
        }
    }

    public static class PagedResults
    {
        public static PagedResult<T> CreatePagedResult<T>(List<T> items, PagedParameters parameters)
        {
            return new PagedResult<T>
            {
                Items = items,
                TotalCount = items.Count,
                Page = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }

        public static PagedResult<T> CreateEmptyPagedResult<T>(PagedParameters parameters)
        {
            return new PagedResult<T>
            {
                Items = new List<T>(),
                TotalCount = 0,
                Page = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
    }

    public static class Parameters
    {
        public static PagedParameters CreateDefaultParameters()
        {
            return new PagedParameters
            {
                PageNumber = 1,
                PageSize = 10
            };
        }

        public static PagedParameters CreateParameters(int pageNumber, int pageSize)
        {
            return new PagedParameters
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}