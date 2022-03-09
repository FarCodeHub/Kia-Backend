using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Operator.Command.Deactive
{
    public class DeactiveOperatorCommandValidator : AbstractValidator<DeactiveOperatorCommand>
    {
        public DeactiveOperatorCommandValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

        }
    }
}