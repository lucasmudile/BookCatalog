using BookCatalog.Application.DTOs.Book;
using FluentValidation;

namespace BookCatalog.Application.Validators.Book;

public class UpdateBookDtoValidator : AbstractValidator<UpdateBookDto>
{
    public UpdateBookDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do livro é obrigatório.")
            .NotEqual(Guid.Empty)
            .WithMessage("O ID do livro deve ser um GUID válido.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("O título do livro é obrigatório.")
            .MaximumLength(200)
            .WithMessage("O título não pode ter mais de 200 caracteres.")
            .MinimumLength(1)
            .WithMessage("O título deve ter pelo menos 1 caractere.");

        RuleFor(x => x.Subtitle)
            .MaximumLength(300)
            .WithMessage("O subtítulo não pode ter mais de 300 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Subtitle));

        RuleFor(x => x.Description)
            .MaximumLength(5000)
            .WithMessage("A descrição não pode ter mais de 5000 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.PublishedDate)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("A data de publicação não pode ser no futuro.")
            .GreaterThan(new DateTime(1450, 1, 1))
            .WithMessage("A data de publicação deve ser posterior ao ano 1450 (invenção da imprensa).")
            .When(x => x.PublishedDate.HasValue);

        RuleFor(x => x.PageCount)
            .GreaterThan(0)
            .WithMessage("O número de páginas deve ser maior que zero.")
            .LessThan(50000)
            .WithMessage("O número de páginas deve ser menor que 50.000.")
            .When(x => x.PageCount.HasValue);

        RuleFor(x => x.Publisher)
            .MaximumLength(200)
            .WithMessage("O nome da editora não pode ter mais de 200 caracteres.")
            .MinimumLength(1)
            .WithMessage("O nome da editora deve ter pelo menos 1 caractere.")
            .When(x => !string.IsNullOrEmpty(x.Publisher));

        RuleFor(x => x.AuthorId)
            .NotEmpty()
            .WithMessage("O ID do autor é obrigatório.")
            .NotEqual(Guid.Empty)
            .WithMessage("O ID do autor deve ser um GUID válido.");

        RuleFor(x => x.GenreId)
            .NotEmpty()
            .WithMessage("O ID do gênero é obrigatório.")
            .NotEqual(Guid.Empty)
            .WithMessage("O ID do gênero deve ser um GUID válido.");
    }
}