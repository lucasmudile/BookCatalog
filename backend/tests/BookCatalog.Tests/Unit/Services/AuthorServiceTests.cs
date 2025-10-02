using BookCatalog.Application.DTOs.Author;
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

public class AuthorServiceTests : TestBase
{
    private readonly Mock<IRepository<Author, Guid>> _mockRepository;
    private readonly Mock<IValidator<CreateAuthorDto>> _mockCreateValidator;
    private readonly Mock<IValidator<UpdateAuthorDto>> _mockUpdateValidator;
    private readonly AuthorService _authorService;

    public AuthorServiceTests()
    {
        _mockRepository = new Mock<IRepository<Author, Guid>>();
        _mockCreateValidator = new Mock<IValidator<CreateAuthorDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdateAuthorDto>>();
        _authorService = new AuthorService(
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
        var authors = new List<Author>
        {
            new() { Id = Guid.NewGuid(), FirstName = "Helton", LastName = "Evambi" },
            new() { Id = Guid.NewGuid(), FirstName = "Armando", LastName = "Kakunda" }
        };
        var pagedResult = new PagedResult<Author>
        {
            Items = authors,
            TotalCount = 2,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetAllAsync(parameters))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _authorService.GetAllAsync(parameters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);

        var firstAuthor = result.Items.First();
        firstAuthor.FirstName.Should().Be("Helton");
        firstAuthor.LastName.Should().Be("Evambi");

        _mockRepository.Verify(r => r.GetAllAsync(parameters), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyPagedResult()
    {
        // Arrange
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var pagedResult = new PagedResult<Author>
        {
            Items = new List<Author>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetAllAsync(parameters))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _authorService.GetAllAsync(parameters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WhenAuthorExists_ShouldReturnAuthorViewModel()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var author = new Author
        {
            Id = authorId,
            FirstName = "Esperança",
            LastName = "Benguela",
            DateOfBirth = new DateTime(1985, 3, 15),
            Biography = "Escritora angolana especializada em literatura contemporânea"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(authorId))
            .ReturnsAsync(author);

        // Act
        var result = await _authorService.GetByIdAsync(authorId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(authorId);
        result.FirstName.Should().Be("Esperança");
        result.LastName.Should().Be("Benguela");
        result.DateOfBirth.Should().Be(new DateTime(1985, 3, 15));
        result.Biography.Should().Be("Escritora angolana especializada em literatura contemporânea");

        _mockRepository.Verify(r => r.GetByIdAsync(authorId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAuthorNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(authorId))
            .ReturnsAsync((Author)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _authorService.GetByIdAsync(authorId));

        exception.Message.Should().Contain($"Author com ID {authorId} não encontrado");
        _mockRepository.Verify(r => r.GetByIdAsync(authorId), Times.Once);
    }

    #endregion

    #region GetAllWithBooksAsync Tests

    [Fact]
    public async Task GetAllWithBooksAsync_WhenCalled_ShouldReturnAuthorsWithBooks()
    {
        // Arrange
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        var authors = new List<Author>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Ondjaki",
                LastName = "Ndalu",
                Books = new List<Book>
                {
                    new() { Id = Guid.NewGuid(), Title = "Os Transparentes" }
                }
            }
        };
        var pagedResult = new PagedResult<Author>
        {
            Items = authors,
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetAllAsync(parameters, It.IsAny<Expression<Func<Author, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _authorService.GetAllWithBooksAsync(parameters, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Books.Should().HaveCount(1);
        result.Items.First().Books.First().Title.Should().Be("Os Transparentes");

        _mockRepository.Verify(r => r.GetAllAsync(parameters, It.IsAny<Expression<Func<Author, object>>>()), Times.Once);
    }

    #endregion

    #region GetByIdWithBooksAsync Tests

    [Fact]
    public async Task GetByIdWithBooksAsync_WhenAuthorExists_ShouldReturnAuthorWithBooks()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        var author = new Author
        {
            Id = authorId,
            FirstName = "José Eduardo",
            LastName = "Agualusa",
            Books = new List<Book>
            {
                new() { Id = Guid.NewGuid(), Title = "O Vendedor de Passados" },
                new() { Id = Guid.NewGuid(), Title = "Nação Crioula" }
            }
        };

        _mockRepository.Setup(r => r.GetByIdAsync(authorId, It.IsAny<Expression<Func<Author, object>>>()))
            .ReturnsAsync(author);

        // Act
        var result = await _authorService.GetByIdWithBooksAsync(authorId, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(authorId);
        result.FirstName.Should().Be("José Eduardo");
        result.LastName.Should().Be("Agualusa");
        result.Books.Should().HaveCount(2);
        result.Books.Should().Contain(b => b.Title == "O Vendedor de Passados");
        result.Books.Should().Contain(b => b.Title == "Nação Crioula");

        _mockRepository.Verify(r => r.GetByIdAsync(authorId, It.IsAny<Expression<Func<Author, object>>>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdWithBooksAsync_WhenAuthorNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        _mockRepository.Setup(r => r.GetByIdAsync(authorId, It.IsAny<Expression<Func<Author, object>>>()))
            .ReturnsAsync((Author)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _authorService.GetByIdWithBooksAsync(authorId, cancellationToken));

        exception.Message.Should().Contain($"Author com ID {authorId} não encontrado");
    }

    #endregion

    #region AddAsync Tests

    [Fact]
    public async Task AddAsync_WhenValidDto_ShouldCreateAuthor()
    {
        // Arrange
        var createDto = new CreateAuthorDto
        {
            FirstName = "Pepetela",
            LastName = "Mayombe",
            DateOfBirth = new DateTime(1941, 10, 29),
            Biography = "Escritor angolano, considerado um dos maiores nomes da literatura africana de língua portuguesa"
        };
        var cancellationToken = CancellationToken.None;
        var validationResult = new ValidationResult();

        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, cancellationToken))
            .ReturnsAsync(validationResult);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Author>(), cancellationToken))
            .ReturnsAsync((Author author, CancellationToken ct) => author);

        // Act
        var result = await _authorService.AddAsync(createDto, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.FirstName.Should().Be("Pepetela");
        result.LastName.Should().Be("Mayombe");
        result.DateOfBirth.Should().Be(new DateTime(1941, 10, 29));
        result.Biography.Should().Be("Escritor angolano, considerado um dos maiores nomes da literatura africana de língua portuguesa");

        _mockCreateValidator.Verify(v => v.ValidateAsync(createDto, cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Author>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var createDto = new CreateAuthorDto
        {
            FirstName = "",
            LastName = "Kakunda",
            Biography = "Biografia inválida"
        };
        var cancellationToken = CancellationToken.None;
        var validationFailures = new List<ValidationFailure>
        {
            new("FirstName", "O primeiro nome é obrigatório")
        };
        var validationResult = new ValidationResult(validationFailures);

        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, cancellationToken))
            .ReturnsAsync(validationResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _authorService.AddAsync(createDto, cancellationToken));

        exception.Errors.Should().HaveCount(1);
        exception.Errors.First().ErrorMessage.Should().Be("O primeiro nome é obrigatório");
        _mockCreateValidator.Verify(v => v.ValidateAsync(createDto, cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Author>(), cancellationToken), Times.Never);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_WhenValidDto_ShouldUpdateAuthor()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var updateDto = new UpdateAuthorDto
        {
            Id = authorId,
            FirstName = "Luandino",
            LastName = "Vieira",
            DateOfBirth = new DateTime(1935, 5, 4),
            Biography = "Escritor angolano, Prêmio Camões 2006"
        };
        var cancellationToken = CancellationToken.None;
        var existingAuthor = new Author
        {
            Id = authorId,
            FirstName = "António",
            LastName = "Vieira"
        };
        var validationResult = new ValidationResult();

        _mockRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync(existingAuthor);
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, cancellationToken))
            .ReturnsAsync(validationResult);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Author>(), cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _authorService.UpdateAsync(authorId, updateDto, cancellationToken);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Author, bool>>>()), Times.Once);
        _mockUpdateValidator.Verify(v => v.ValidateAsync(updateDto, cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Author>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenIdMismatch_ShouldThrowBadRequestException()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var differentId = Guid.NewGuid();
        var updateDto = new UpdateAuthorDto
        {
            Id = differentId,
            FirstName = "Manuel",
            LastName = "Rui"
        };
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => _authorService.UpdateAsync(authorId, updateDto, cancellationToken));

        exception.Message.Should().Be("Os Id são diferentes ");
    }

    [Fact]
    public async Task UpdateAsync_WhenAuthorNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var updateDto = new UpdateAuthorDto
        {
            Id = authorId,
            FirstName = "Uanhenga",
            LastName = "Xitu"
        };
        var cancellationToken = CancellationToken.None;

        _mockRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync((Author)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _authorService.UpdateAsync(authorId, updateDto, cancellationToken));

        exception.Message.Should().Contain($"Author com ID {authorId} não encontrado");
    }

    [Fact]
    public async Task UpdateAsync_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var updateDto = new UpdateAuthorDto
        {
            Id = authorId,
            FirstName = "",
            LastName = "Benguela"
        };
        var cancellationToken = CancellationToken.None;
        var existingAuthor = new Author { Id = authorId };
        var validationFailures = new List<ValidationFailure>
        {
            new("FirstName", "O primeiro nome é obrigatório")
        };
        var validationResult = new ValidationResult(validationFailures);

        _mockRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync(existingAuthor);
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, cancellationToken))
            .ReturnsAsync(validationResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _authorService.UpdateAsync(authorId, updateDto, cancellationToken));

        exception.Errors.Should().HaveCount(1);
        exception.Errors.First().ErrorMessage.Should().Be("O primeiro nome é obrigatório");
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_WhenAuthorExists_ShouldReturnTrue()
    {
        // Arrange
        var authorId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _mockRepository.Setup(r => r.DeleteAsync(authorId, cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _authorService.DeleteAsync(authorId, cancellationToken);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(authorId, cancellationToken), Times.Once);
    }

    #endregion

    #region SearchByNameAsync Tests

    [Fact]
    public async Task SearchByNameAsync_WhenNameProvided_ShouldReturnMatchingAuthors()
    {
        // Arrange
        var name = "agualusa";
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        var authors = new List<Author>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "José Eduardo",
                LastName = "Agualusa",
                Books = new List<Book>()
            }
        };
        var pagedResult = new PagedResult<Author>
        {
            Items = authors,
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetByFilterAsync(
                It.IsAny<Expression<Func<Author, bool>>>(),
                parameters,
                It.IsAny<Expression<Func<Author, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _authorService.SearchByNameAsync(name, parameters, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().FirstName.Should().Be("José Eduardo");
        result.Items.First().LastName.Should().Be("Agualusa");

        _mockRepository.Verify(r => r.GetByFilterAsync(
            It.IsAny<Expression<Func<Author, bool>>>(),
            parameters,
            It.IsAny<Expression<Func<Author, object>>>()), Times.Once);
    }

    [Fact]
    public async Task SearchByNameAsync_WhenNoMatch_ShouldReturnEmptyResult()
    {
        // Arrange
        var name = "inexistente";
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        var pagedResult = new PagedResult<Author>
        {
            Items = new List<Author>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetByFilterAsync(
                It.IsAny<Expression<Func<Author, bool>>>(),
                parameters,
                It.IsAny<Expression<Func<Author, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _authorService.SearchByNameAsync(name, parameters, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    #endregion
}