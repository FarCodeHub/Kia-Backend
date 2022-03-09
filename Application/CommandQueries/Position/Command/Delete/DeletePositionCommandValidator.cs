using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Position.Command.Delete
{
    public class DeletePositionCommandValidator : AbstractValidator<DeletePositionCommand>
    {
        public DeletePositionCommandValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

        }
    }
}