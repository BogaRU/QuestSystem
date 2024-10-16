using FluentValidation;
using QuestSystem.Application.DTOs;

namespace QuestSystem.Application.Validators
{
    public class QuestProgressUpdateDtoValidator : AbstractValidator<QuestProgressUpdateDto>
    {
        public QuestProgressUpdateDtoValidator()
        {
            RuleFor(x => x.CollectedItemIds)
                .Must(collectedItemIds => collectedItemIds == null || collectedItemIds.All(id => id != Guid.Empty))
                .WithMessage("Список собранных предметов содержит некорректные идентификаторы.");

            RuleFor(x => x.DefeatedMonsters)
                .GreaterThanOrEqualTo(0).WithMessage("Побеждённые монстры не могут быть отрицательными.");

            RuleFor(x => x.VisitedLocations)
                .GreaterThanOrEqualTo(0).WithMessage("Посещённые локации не могут быть отрицательными.");
        }
    }
}
