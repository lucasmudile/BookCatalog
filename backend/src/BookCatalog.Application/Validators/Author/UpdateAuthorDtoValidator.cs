using BookCatalog.Application.DTOs.Author;
using FluentValidation;

namespace BookCatalog.Application.Validators.Author;

public class UpdateAuthorDtoValidator : AbstractValidator<UpdateAuthorDto>
{
    public UpdateAuthorDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("O ID do autor é obrigatório.")
            .NotEqual(Guid.Empty)
            .WithMessage("O ID do autor deve ser um GUID válido.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("O primeiro nome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O primeiro nome não pode ter mais de 100 caracteres.")
            .Matches("^[a-zA-ZÀ-ÿ\\s'-]+$")
            .WithMessage("O primeiro nome deve conter apenas letras, espaços, hífens e apostrofes.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("O sobrenome é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O sobrenome não pode ter mais de 100 caracteres.")
            .Matches("^[a-zA-ZÀ-ÿ\\s'-]+$")
            .WithMessage("O sobrenome deve conter apenas letras, espaços, hífens e apostrofes.");

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now)
            .WithMessage("A data de nascimento deve ser anterior à data atual.")
            .GreaterThan(new DateTime(1800, 1, 1))
            .WithMessage("A data de nascimento deve ser posterior ao ano 1800.")
            .When(x => x.DateOfBirth.HasValue);

        RuleFor(x => x.DateOfDeath)
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("A data de falecimento não pode ser no futuro.")
            .GreaterThan(x => x.DateOfBirth)
            .WithMessage("A data de falecimento deve ser posterior à data de nascimento.")
            .When(x => x.DateOfDeath.HasValue);

        RuleFor(x => x.Biography)
            .MaximumLength(2000)
            .WithMessage("A biografia não pode ter mais de 2000 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Biography));
    }
}
