using FluentValidation;
using QuestSystem.Application.DTOs;

namespace QuestSystem.Application.Validators
{
    public class CreatePlayerDtoValidator : AbstractValidator<CreatePlayerDto>
    {
        public CreatePlayerDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Имя игрока не может быть пустым.")
                .MaximumLength(100).WithMessage("Имя игрока не может превышать 100 символов.");

            RuleFor(x => x.Level)
                .InclusiveBetween(1, 100).WithMessage("Уровень игрока должен быть между 1 и 100.");
        }
    }
}
