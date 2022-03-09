using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Operator.Command.Create
{
    public class CreateOperatorCommandValidator : AbstractValidator<CreateOperatorCommand>
    {
        public CreateOperatorCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.ExtentionNumber)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");
        }
    }
}