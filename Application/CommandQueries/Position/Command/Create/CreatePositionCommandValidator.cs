using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Position.Command.Create
{
    public class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
    {
        public CreatePositionCommandValidator()
        {
            RuleFor(x => x.Title)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x.Length <= 50).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.UniqueName)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x.Length <= 50).WithMessage($"{ExceptionMessage.MaxLenght}:50");
        }
    }
}