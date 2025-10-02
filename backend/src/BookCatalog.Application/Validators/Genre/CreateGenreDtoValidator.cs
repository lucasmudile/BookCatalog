using BookCatalog.Application.DTOs.Genre;
using FluentValidation;

namespace BookCatalog.Application.Validators.Genre;

public class CreateGenreDtoValidator : AbstractValidator<CreateGenreDto>
{
    public CreateGenreDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("O nome do gênero é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome do gênero não pode ter mais de 100 caracteres.")
            .MinimumLength(2)
            .WithMessage("O nome do gênero deve ter pelo menos 2 caracteres.");
            
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("A descrição do gênero não pode ter mais de 1000 caracteres.")
            .MinimumLength(10)
            .WithMessage("A descrição deve ter pelo menos 10 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}