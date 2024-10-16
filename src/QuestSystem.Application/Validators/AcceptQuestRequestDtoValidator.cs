using FluentValidation;
using QuestSystem.Application.DTOs;

namespace QuestSystem.Application.Validators
{
    public class AcceptQuestRequestDtoValidator : AbstractValidator<AcceptQuestRequestDto>
    {
        public AcceptQuestRequestDtoValidator()
        {
            RuleFor(x => x.PlayerId)
                .NotEmpty().WithMessage("Идентификатор игрока не может быть пустым.");

            RuleFor(x => x.QuestId)
                .NotEmpty().WithMessage("Идентификатор квеста не может быть пустым.");
        }
    }
}
