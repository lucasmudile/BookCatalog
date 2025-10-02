using BookCatalog.Application.DTOs.Genre;
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

public class GenreServiceTests : TestBase
{
    private readonly Mock<IRepository<Genre, Guid>> _mockRepository;
    private readonly Mock<IValidator<CreateGenreDto>> _mockCreateValidator;
    private readonly Mock<IValidator<UpdateGenreDto>> _mockUpdateValidator;
    private readonly GenreService _genreService;

    public GenreServiceTests()
    {
        _mockRepository = new Mock<IRepository<Genre, Guid>>();
        _mockCreateValidator = new Mock<IValidator<CreateGenreDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdateGenreDto>>();
        _genreService = new GenreService(
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
        var genres = new List<Genre>
        {
            new() { Id = Guid.NewGuid(), Name = "Romance Africano", Description = "Literatura romântica de origem africana" },
            new() { Id = Guid.NewGuid(), Name = "Ficção Histórica", Description = "Narrativas baseadas em eventos históricos" }
        };
        var pagedResult = new PagedResult<Genre>
        {
            Items = genres,
            TotalCount = 2,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetAllAsync(parameters))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _genreService.GetAllAsync(parameters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);

        var firstGenre = result.Items.First();
        firstGenre.Name.Should().Be("Romance Africano");
        firstGenre.Description.Should().Be("Literatura romântica de origem africana");

        _mockRepository.Verify(r => r.GetAllAsync(parameters), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyPagedResult()
    {
        // Arrange
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var pagedResult = new PagedResult<Genre>
        {
            Items = new List<Genre>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetAllAsync(parameters))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _genreService.GetAllAsync(parameters);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    #endregion

    #region GetByIdAsync Tests

    [Fact]
    public async Task GetByIdAsync_WhenGenreExists_ShouldReturnGenreViewModel()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var genre = new Genre
        {
            Id = genreId,
            Name = "Literatura Angolana",
            Description = "Gênero que abrange obras literárias de autores angolanos ou sobre temas relacionados com Angola"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(genreId))
            .ReturnsAsync(genre);

        // Act
        var result = await _genreService.GetByIdAsync(genreId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(genreId);
        result.Name.Should().Be("Literatura Angolana");
        result.Description.Should().Be("Gênero que abrange obras literárias de autores angolanos ou sobre temas relacionados com Angola");

        _mockRepository.Verify(r => r.GetByIdAsync(genreId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WhenGenreNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        _mockRepository.Setup(r => r.GetByIdAsync(genreId))
            .ReturnsAsync((Genre)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _genreService.GetByIdAsync(genreId));

        exception.Message.Should().Contain($"Gênero com ID {genreId} não encontrado");
        _mockRepository.Verify(r => r.GetByIdAsync(genreId), Times.Once);
    }

    #endregion

    #region GetAllWithBooksAsync Tests

    [Fact]
    public async Task GetAllWithBooksAsync_WhenCalled_ShouldReturnGenresWithBooks()
    {
        // Arrange
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        var genres = new List<Genre>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Realismo Mágico",
                Description = "Gênero que mistura elementos realistas com fantásticos",
                Books = new List<Book>
                {
                    new() { Id = Guid.NewGuid(), Title = "Terra Sonâmbula" },
                    new() { Id = Guid.NewGuid(), Title = "A Varanda do Frangipani" }
                }
            }
        };
        var pagedResult = new PagedResult<Genre>
        {
            Items = genres,
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetAllAsync(parameters, It.IsAny<Expression<Func<Genre, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _genreService.GetAllWithBooksAsync(parameters, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Books.Should().HaveCount(2);
        result.Items.First().Books.Should().Contain(b => b.Title == "Terra Sonâmbula");
        result.Items.First().Books.Should().Contain(b => b.Title == "A Varanda do Frangipani");

        _mockRepository.Verify(r => r.GetAllAsync(parameters, It.IsAny<Expression<Func<Genre, object>>>()), Times.Once);
    }

    #endregion

    #region AddAsync Tests

    [Fact]
    public async Task AddAsync_WhenValidDto_ShouldCreateGenre()
    {
        // Arrange
        var createDto = new CreateGenreDto
        {
            Name = "Poesia Contemporânea",
            Description = "Gênero que engloba obras poéticas da era moderna e contemporânea"
        };
        var cancellationToken = CancellationToken.None;
        var validationResult = new ValidationResult();

        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, cancellationToken))
            .ReturnsAsync(validationResult);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Genre>(), cancellationToken))
            .ReturnsAsync((Genre genre, CancellationToken ct) => genre);

        // Act
        var result = await _genreService.AddAsync(createDto, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Poesia Contemporânea");
        result.Description.Should().Be("Gênero que engloba obras poéticas da era moderna e contemporânea");

        _mockCreateValidator.Verify(v => v.ValidateAsync(createDto, cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Genre>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var createDto = new CreateGenreDto
        {
            Name = "",
            Description = "Desc"
        };
        var cancellationToken = CancellationToken.None;
        var validationFailures = new List<ValidationFailure>
        {
            new("Name", "O nome do gênero é obrigatório"),
            new("Description", "A descrição deve ter pelo menos 10 caracteres")
        };
        var validationResult = new ValidationResult(validationFailures);

        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, cancellationToken))
            .ReturnsAsync(validationResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _genreService.AddAsync(createDto, cancellationToken));

        exception.Errors.Should().HaveCount(2);
        exception.Errors.Should().Contain(e => e.ErrorMessage == "O nome do gênero é obrigatório");
        exception.Errors.Should().Contain(e => e.ErrorMessage == "A descrição deve ter pelo menos 10 caracteres");

        _mockCreateValidator.Verify(v => v.ValidateAsync(createDto, cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Genre>(), cancellationToken), Times.Never);
    }

    #endregion

    #region UpdateAsync Tests

    [Fact]
    public async Task UpdateAsync_WhenValidDto_ShouldUpdateGenre()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var updateDto = new UpdateGenreDto
        {
            Id = genreId,
            Name = "Literatura de Resistência",
            Description = "Gênero que aborda temas de luta e resistência política e social"
        };
        var cancellationToken = CancellationToken.None;
        var existingGenre = new Genre
        {
            Id = genreId,
            Name = "Nome Antigo",
            Description = "Descrição antiga"
        };
        var validationResult = new ValidationResult();

        _mockRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(existingGenre);
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, cancellationToken))
            .ReturnsAsync(validationResult);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Genre>(), cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _genreService.UpdateAsync(genreId, updateDto, cancellationToken);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()), Times.Once);
        _mockUpdateValidator.Verify(v => v.ValidateAsync(updateDto, cancellationToken), Times.Once);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Genre>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenIdMismatch_ShouldThrowBadRequestException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var differentId = Guid.NewGuid();
        var updateDto = new UpdateGenreDto
        {
            Id = differentId,
            Name = "Drama Contemporâneo"
        };
        var cancellationToken = CancellationToken.None;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(
            () => _genreService.UpdateAsync(genreId, updateDto, cancellationToken));

        exception.Message.Should().Be("Os Id são diferentes ");
    }

    [Fact]
    public async Task UpdateAsync_WhenGenreNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var updateDto = new UpdateGenreDto
        {
            Id = genreId,
            Name = "Ficção Científica Africana"
        };
        var cancellationToken = CancellationToken.None;

        _mockRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync((Genre)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(
            () => _genreService.UpdateAsync(genreId, updateDto, cancellationToken));

        exception.Message.Should().Contain($"Gênero com ID {genreId} não encontrado");
    }

    [Fact]
    public async Task UpdateAsync_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var updateDto = new UpdateGenreDto
        {
            Id = genreId,
            Name = "",
            Description = "Desc"
        };
        var cancellationToken = CancellationToken.None;
        var existingGenre = new Genre { Id = genreId };
        var validationFailures = new List<ValidationFailure>
        {
            new("Name", "O nome do gênero é obrigatório"),
            new("Description", "A descrição deve ter pelo menos 10 caracteres")
        };
        var validationResult = new ValidationResult(validationFailures);

        _mockRepository.Setup(r => r.SingleOrDefaultAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(existingGenre);
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, cancellationToken))
            .ReturnsAsync(validationResult);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(
            () => _genreService.UpdateAsync(genreId, updateDto, cancellationToken));

        exception.Errors.Should().HaveCount(2);
        exception.Errors.Should().Contain(e => e.ErrorMessage == "O nome do gênero é obrigatório");
        exception.Errors.Should().Contain(e => e.ErrorMessage == "A descrição deve ter pelo menos 10 caracteres");
    }

    #endregion

    #region DeleteAsync Tests

    [Fact]
    public async Task DeleteAsync_WhenGenreExists_ShouldReturnTrue()
    {
        // Arrange
        var genreId = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        _mockRepository.Setup(r => r.DeleteAsync(genreId, cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _genreService.DeleteAsync(genreId, cancellationToken);

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(genreId, cancellationToken), Times.Once);
    }

    #endregion

    #region SearchByNameAsync Tests

    [Fact]
    public async Task SearchByNameAsync_WhenNameProvided_ShouldReturnMatchingGenres()
    {
        // Arrange
        var name = "africano";
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        var genres = new List<Genre>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Romance Africano",
                Description = "Literatura romântica de origem africana",
                Books = new List<Book>
                {
                    new() { Id = Guid.NewGuid(), Title = "O Vendedor de Passados" }
                }
            }
        };
        var pagedResult = new PagedResult<Genre>
        {
            Items = genres,
            TotalCount = 1,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetByFilterAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                parameters,
                It.IsAny<Expression<Func<Genre, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _genreService.SearchByNameAsync(name, parameters, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(1);
        result.Items.First().Name.Should().Be("Romance Africano");
        result.Items.First().Description.Should().Be("Literatura romântica de origem africana");
        result.Items.First().Books.Should().HaveCount(1);
        result.Items.First().Books.First().Title.Should().Be("O Vendedor de Passados");

        _mockRepository.Verify(r => r.GetByFilterAsync(
            It.IsAny<Expression<Func<Genre, bool>>>(),
            parameters,
            It.IsAny<Expression<Func<Genre, object>>>()), Times.Once);
    }

    [Fact]
    public async Task SearchByNameAsync_WhenNoMatch_ShouldReturnEmptyResult()
    {
        // Arrange
        var name = "inexistente";
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        var pagedResult = new PagedResult<Genre>
        {
            Items = new List<Genre>(),
            TotalCount = 0,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetByFilterAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                parameters,
                It.IsAny<Expression<Func<Genre, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _genreService.SearchByNameAsync(name, parameters, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);

        _mockRepository.Verify(r => r.GetByFilterAsync(
            It.IsAny<Expression<Func<Genre, bool>>>(),
            parameters,
            It.IsAny<Expression<Func<Genre, object>>>()), Times.Once);
    }

    [Fact]
    public async Task SearchByNameAsync_WhenNameIsEmpty_ShouldReturnAllGenres()
    {
        // Arrange
        var name = "";
        var parameters = new PagedParameters { PageNumber = 1, PageSize = 10 };
        var cancellationToken = CancellationToken.None;
        var genres = new List<Genre>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Literatura Lusófona",
                Description = "Gênero que abrange literaturas de países de língua portuguesa",
                Books = new List<Book>()
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Crônica Contemporânea",
                Description = "Narrativas curtas sobre o cotidiano moderno",
                Books = new List<Book>()
            }
        };
        var pagedResult = new PagedResult<Genre>
        {
            Items = genres,
            TotalCount = 2,
            Page = 1,
            PageSize = 10
        };

        _mockRepository.Setup(r => r.GetByFilterAsync(
                It.IsAny<Expression<Func<Genre, bool>>>(),
                parameters,
                It.IsAny<Expression<Func<Genre, object>>>()))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await _genreService.SearchByNameAsync(name, parameters, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.Items.Should().Contain(g => g.Name == "Literatura Lusófona");
        result.Items.Should().Contain(g => g.Name == "Crônica Contemporânea");
    }

    #endregion
}