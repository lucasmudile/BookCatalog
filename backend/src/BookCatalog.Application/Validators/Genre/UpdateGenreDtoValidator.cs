using BookCatalog.Application.DTOs.Genre;
using FluentValidation;

namespace BookCatalog.Application.Validators.Genre;

public class UpdateGenreDtoValidator : AbstractValidator<UpdateGenreDto>
{
    public UpdateGenreDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do gênero é obrigatório.")
            .NotEqual(Guid.Empty)
            .WithMessage("O ID do gênero deve ser um GUID válido.");

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