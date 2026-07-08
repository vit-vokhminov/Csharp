using FluentValidation;

namespace TemplateService.Core.Locations;

/// <summary>
/// Валидатор для CreateLocationDto.
/// </summary>
public sealed class CreateLocationDtoValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название локации не может быть пустым.")
            .MaximumLength(200).WithMessage("Название локации не может быть длиннее 200 символов.");

        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Улица не может быть пустой.")
            .MaximumLength(200).WithMessage("Улица не может быть длиннее 200 символов.");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Город не может быть пустым.")
            .MaximumLength(100).WithMessage("Город не может быть длиннее 100 символов.");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Страна не может быть пустой.")
            .MaximumLength(100).WithMessage("Страна не может быть длиннее 100 символов.");

        RuleFor(x => x.ZipCode)
            .MaximumLength(20).WithMessage("Почтовый индекс не может быть длиннее 20 символов.");
    }
}
