using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Unit.Command.Delete
{
    public class DeleteUnitCommandValidator : AbstractValidator<DeleteUnitCommand>
    {
        public DeleteUnitCommandValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");
        }
    }
}