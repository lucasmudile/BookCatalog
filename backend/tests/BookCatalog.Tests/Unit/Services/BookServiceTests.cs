using BookCatalog.Application.DTOs.Book;
using BookCatalog.Application.Services;
using BookCatalog.Application.ViewModel;
using BookCatalog.Domain.Abstractions;
using BookCatalog.Domain.Entities;
using BookCatalog.Shared.Exceptions;
using BookCatalog.Tests.Unit.Helpers;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System.Linq.Expressions;

namespace BookCatalog.Tests.Unit.Services;

public class BookServiceTests : TestBase
{
    private readonly Mock<IRepository<Book, Guid>> _mockRepository;
    private readonly Mock<IValidator<CreateBookDto>> _mockCreateValidator;
    private readonly Mock<IValidator<UpdateBookDto>> _mockUpdateValidator;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _mockRepository = new Mock<IRepository<Book, Guid>>();
        _mockCreateValidator = new Mock<IValidator<CreateBookDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdateBookDto>>();
        _bookService = new BookService(
            _mockRepository.Object,
            _mockCreateValidator.Object,
            _mockUpdateValidator.Object);
    }

    #region GetAllAsync Tests

    [Fact]
    public async Task GetAllAsync_WhenCalled_ShouldReturnPagedResult()
    {
        // Arrange
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var books = new List<Book>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Os Transparentes",
                Author = new Author { FirstName = "Ondjaki", LastName = "Ndalu" },
                Genre = new Genre { Name = "Romance" }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "O Vendedor de Passados",
                Author = new Author { FirstName = "José Eduardo", LastName = "Agualusa" },
                Genre = new Genre { Name = "Ficção" }
            }
        };
        var pagedResult = new PagedResult<Book>
        {
            Items = books,
            TotalCount = 2,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetAllAsync(parameters, It.IsAny<Expression<Func<Book, object>>>(),
                It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _bookService.GetAllAsync(parameters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);

        var firstBook = result.Items.First();
        firstBook.Title.Should().Be("Os Transparentes");

        _mockRepository.Verify(r => r.GetAllAsync(parameters, It.IsAny<Expression<Func<Book, object>>>(),
            It.IsAny<Expression<Func<Book, object>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyPagedResult()
    {
        // Arrange
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var pagedResult = new PagedResult<Book>
        {
            Items = new List<Book>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetAllAsync(parameters, It.IsAny<Expression<Func<Book, object>>>(),
                It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _bookService.GetAllAsync(parameters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WhenBookExists_ShouldReturnBookViewModel()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var book = new Book
        {
            Id = bookId,
            Title = "Mayombe",
            Subtitle = "Um Romance da Guerra",
            Description = "Romance sobre a luta pela independência de Angola",
            PublishedDate = new DateTime(1980, 1, 1),
            ISBN = "978-972-1-04567-8",
            PageCount = 312,
            Publisher = "Dom Quixote",
            Author = new Author { FirstName = "Pepetela", LastName = "Mayombe" },
            Genre = new Genre { Name = "Romance Histórico" }
        };

        _mockRepository.Setup(r => r.GetByIdAsync(bookId, It.IsAny<Expression<Func<Book, object>>>(),
                It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync(book);

        // Act
        var result = await _bookService.GetByIdAsync(bookId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(bookId);
        result.Title.Should().Be("Mayombe");
        result.Subtitle.Should().Be("Um Romance da Guerra");
        result.Description.Should().Be("Romance sobre a luta pela independência de Angola");
        result.PublishedDate.Should().Be(new DateTime(1980, 1, 1));
        result.ISBN.Should().Be("978-972-1-04567-8");
        result.PageCount.Should().Be(312);
        result.Publisher.Should().Be("Dom Quixote");

        _mockRepository.Verify(r => r.GetByIdAsync(bookId, It.IsAny<Expression<Func<Book, object>>>(),
            It.IsAny<Expression<Func<Book, object>>>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenBookNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(bookId, It.IsAny<Expression<Func<Book, object>>>(),
                It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync((Book)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _bookService.GetByIdAsync(bookId));

        exception.Message.Should().Contain($"Livro com ID {bookId} não encontrado");
        _mockRepository.Verify(r => r.GetByIdAsync(bookId, It.IsAny<Expression<Func<Book, object>>>(),
            It.IsAny<Expression<Func<Book, object>>>()), Times.Once);
    }

    #endregion

    #region GetByAuthorIdAsync Tests

    [Fact]
    public async Task GetByAuthorIdAsync_WhenAuthorHasBooks_ShouldReturnBooks()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var books = new List<Book>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Nação Crioula",
                AuthorId = authorId,
                Author = new Author { FirstName = "José Eduardo", LastName = "Agualusa" },
                Genre = new Genre { Name = "Romance" }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "A Conjuração",
                AuthorId = authorId,
                Author = new Author { FirstName = "José Eduardo", LastName = "Agualusa" },
                Genre = new Genre { Name = "Romance Histórico" }
            }
        };
        var pagedResult = new PagedResult<Book>
        {
            Items = books,
            TotalCount = 2,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.FindAsync(parameters, It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Expression<Func<Book, object>>>(), It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _bookService.GetByAuthorIdAsync(authorId, parameters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Should().AllSatisfy(book => book.AuthorId.Should().Be(authorId));
        result.Items.Should().Contain(b => b.Title == "Nação Crioula");
        result.Items.Should().Contain(b => b.Title == "A Conjuração");

        _mockRepository.Verify(r => r.FindAsync(parameters, It.IsAny<Expression<Func<Book, bool>>>(),
            It.IsAny<Expression<Func<Book, object>>>(), It.IsAny<Expression<Func<Book, object>>>()), Times.Once);
    }

    [Fact]
    public async Task GetByAuthorIdAsync_WhenAuthorHasNoBooks_ShouldThrowNotFoundException()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };

        _mockRepository.Setup(r => r.FindAsync(parameters, It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Expression<Func<Book, object>>>(), It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync((PagedResult<Book>)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _bookService.GetByAuthorIdAsync(authorId, parameters));

        exception.Message.Should().Contain($"Nenhum livro encontrado para o autor com ID {authorId}");
    }

    #endregion

    #region GetByGenreIdAsync Tests

    [Fact]
    public async Task GetByGenreIdAsync_WhenGenreHasBooks_ShouldReturnBooks()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var books = new List<Book>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Terra Sonâmbula",
                GenreId = genreId,
                Author = new Author { FirstName = "Mia", LastName = "Couto" },
                Genre = new Genre { Name = "Realismo Mágico" }
            }
        };
        var pagedResult = new PagedResult<Book>
        {
            Items = books,
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.FindAsync(parameters, It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Expression<Func<Book, object>>>(), It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _bookService.GetByGenreIdAsync(genreId, parameters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().GenreId.Should().Be(genreId);
        result.Items.First().Title.Should().Be("Terra Sonâmbula");

        _mockRepository.Verify(r => r.FindAsync(parameters, It.IsAny<Expression<Func<Book, bool>>>(),
            It.IsAny<Expression<Func<Book, object>>>(), It.IsAny<Expression<Func<Book, object>>>()), Times.Once);
    }

    [Fact]
    public async Task GetByGenreIdAsync_WhenGenreHasNoBooks_ShouldThrowNotFoundException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };

        _mockRepository.Setup(r => r.FindAsync(parameters, It.IsAny<Expression<Func<Book, bool>>>(),
                It.IsAny<Expression<Func<Book, object>>>(), It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync((PagedResult<Book>)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _bookService.GetByGenreIdAsync(genreId, parameters));

        exception.Message.Should().Contain($"Nenhum livro encontrado para o género com ID {genreId}");
    }

    #endregion

    #region AddAsync Tests

    [Fact]
    public async Task AddAsync_WhenValidDto_ShouldCreateBook()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var genreId = Guid.NewGuid();
        var createDto = new CreateBookDto
        {
            Title = "As Aventuras de Ngunga",
            Subtitle = "Um Romance de Pepetela",
            Description = "História de um jovem guerrilheiro durante a luta pela independência",
            PublishedDate = new DateTime(1973, 1, 1),
            ISBN = "978-972-1-04123-6",
            PageCount = 128,
            Publisher = "União dos Escritores Angolanos",
            AuthorId = authorId,
            GenreId = genreId
        };
        var cancellationToken = CancellationToken.None;
        var validationResult = new ValidationResult();

        var savedBook = new Book
        {
            Id = Guid.NewGuid(),
            Title = createDto.Title,
            Subtitle = createDto.Subtitle,
            Description = createDto.Description,
            PublishedDate = createDto.PublishedDate,
            ISBN = createDto.ISBN,
            PageCount = createDto.PageCount,
            Publisher = createDto.Publisher,
            AuthorId = authorId,
            GenreId = genreId,
            Author = new Author { FirstName = "Pepetela", LastName = "Mayombe" },
            Genre = new Genre { Name = "Romance" }
        };

        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, cancellationToken))
            .ReturnsAsync(validationResult);
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Book>(), cancellationToken))
            .ReturnsAsync((Book book, CancellationToken ct) => book);
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Expression<Func<Book, object>>>(),
                It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync(savedBook);

        // Act
        var result = await _bookService.AddAsync(createDto, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("As Aventuras de Ngunga");
        result.Subtitle.Should().Be("Um Romance de Pepetela");
        result.Description.Should().Be("História de um jovem guerrilheiro durante a luta pela independência");
        result.PublishedDate.Should().Be(new DateTime(1973, 1, 1));
        result.ISBN.Should().Be("978-972-1-04123-6");
        result.PageCount.Should().Be(128);
        result.Publisher.Should().Be("União dos Escritores Angolanos");
        result.AuthorId.Should().Be(authorId);
        result.GenreId.Should().Be(genreId);
        result.AuthorName.Should().Be("Pepetela Mayombe");
        result.GenreName.Should().Be("Romance");

        _mockCreateValidator.Verify(v => v.ValidateAsync(createDto, cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Book>(), cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<Expression<Func<Book, object>>>(),
            It.IsAny<Expression<Func<Book, object>>>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var createDto = new CreateBookDto
        {
            Title = "",
            AuthorId = Guid.Empty,
            GenreId = Guid.Empty
        };
        var cancellationToken = CancellationToken.None;
        var validationFailures = new List<ValidationFailure>
        {
            new("Title", "O título do livro é obrigatório"),
            new("AuthorId", "O ID do autor é obrigatório"),
            new("GenreId", "O ID do gênero é obrigatório")
        };
        var validationResult = new ValidationResult(validationFailures);

        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, cancellationToken))
            .ReturnsAsync(validationResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _bookService.AddAsync(createDto, cancellationToken));

        exception.Errors.Should().HaveCount(3);
        exception.Errors.Should().Contain(e => e.ErrorMessage == "O título do livro é obrigatório");
        exception.Errors.Should().Contain(e => e.ErrorMessage == "O ID do autor é obrigatório");
        exception.Errors.Should().Contain(e => e.ErrorMessage == "O ID do gênero é obrigatório");

        _mockCreateValidator.Verify(v => v.ValidateAsync(createDto, cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Book>(), cancellationToken), Times.Never);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_WhenValidDto_ShouldUpdateBook()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var authorId = Guid.NewGuid();
        var genreId = Guid.NewGuid();
        var updateDto = new UpdateBookDto
        {
            Id = bookId,
            Title = "A Geração da Utopia",
            Subtitle = "Romance Angolano",
            Description = "Romance sobre diferentes gerações de angolanos",
            PublishedDate = new DateTime(1992, 1, 1),
            ISBN = "978-972-1-04789-4",
            PageCount = 364,
            Publisher = "Dom Quixote",
            AuthorId = authorId,
            GenreId = genreId
        };
        var cancellationToken = CancellationToken.None;
        var existingBook = new Book
        {
            Id = bookId,
            Title = "Título Antigo",
            AuthorId = authorId,
            GenreId = genreId
        };
        var validationResult = new ValidationResult();

        _mockRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(existingBook);
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, cancellationToken))
            .ReturnsAsync(validationResult);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Book>(), cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _bookService.UpdateAsync(bookId, updateDto, cancellationToken);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Book, bool>>>()), Times.Once);
        _mockUpdateValidator.Verify(v => v.ValidateAsync(updateDto, cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Book>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenIdMismatch_ShouldThrowBadRequestException()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var differentId = Guid.NewGuid();
        var updateDto = new UpdateBookDto
        {
            Id = differentId,
            Title = "Yaka"
        };
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => _bookService.UpdateAsync(bookId, updateDto, cancellationToken));

        exception.Message.Should().Be("Os Id são diferentes ");
    }

    [Fact]
    public async Task UpdateAsync_WhenBookNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var updateDto = new UpdateBookDto
        {
            Id = bookId,
            Title = "Lueji"
        };
        var cancellationToken = CancellationToken.None;

        _mockRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync((Book)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _bookService.UpdateAsync(bookId, updateDto, cancellationToken));

        exception.Message.Should().Contain($"Livro com ID {bookId} não encontrado");
    }

    [Fact]
    public async Task UpdateAsync_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var updateDto = new UpdateBookDto
        {
            Id = bookId,
            Title = "",
            AuthorId = Guid.Empty
        };
        var cancellationToken = CancellationToken.None;
        var existingBook = new Book { Id = bookId };
        var validationFailures = new List<ValidationFailure>
        {
            new("Title", "O título do livro é obrigatório"),
            new("AuthorId", "O ID do autor é obrigatório")
        };
        var validationResult = new ValidationResult(validationFailures);

        _mockRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(existingBook);
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, cancellationToken))
            .ReturnsAsync(validationResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _bookService.UpdateAsync(bookId, updateDto, cancellationToken));

        exception.Errors.Should().HaveCount(2);
        exception.Errors.Should().Contain(e => e.ErrorMessage == "O título do livro é obrigatório");
        exception.Errors.Should().Contain(e => e.ErrorMessage == "O ID do autor é obrigatório");
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_WhenBookExists_ShouldReturnTrue()
    {
        // Arrange
        var bookId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _mockRepository.Setup(r => r.DeleteAsync(bookId, cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _bookService.DeleteAsync(bookId, cancellationToken);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(bookId, cancellationToken), Times.Once);
    }

    #endregion

    #region SearchByNameAsync Tests

    [Fact]
    public async Task SearchByNameAsync_WhenTitleProvided_ShouldReturnMatchingBooks()
    {
        // Arrange
        var title = "transparentes";
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        var books = new List<Book>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Os Transparentes",
                Author = new Author { FirstName = "Ondjaki", LastName = "Ndalu" },
                Genre = new Genre { Name = "Romance" }
            }
        };
        var pagedResult = new PagedResult<Book>
        {
            Items = books,
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetByFilterAsync(
                It.IsAny<Expression<Func<Book, bool>>>(),
                parameters,
                It.IsAny<Expression<Func<Book, object>>>(),
                It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _bookService.SearchByNameAsync(title, parameters, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Title.Should().Be("Os Transparentes");

        _mockRepository.Verify(r => r.GetByFilterAsync(
            It.IsAny<Expression<Func<Book, bool>>>(),
            parameters,
            It.IsAny<Expression<Func<Book, object>>>(),
            It.IsAny<Expression<Func<Book, object>>>()), Times.Once);
    }

    [Fact]
    public async Task SearchByNameAsync_WhenNoMatch_ShouldReturnEmptyResult()
    {
        // Arrange
        var title = "inexistente";
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        var pagedResult = new PagedResult<Book>
        {
            Items = new List<Book>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetByFilterAsync(
                It.IsAny<Expression<Func<Book, bool>>>(),
                parameters,
                It.IsAny<Expression<Func<Book, object>>>(),
                It.IsAny<Expression<Func<Book, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _bookService.SearchByNameAsync(title, parameters, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    #endregion
}