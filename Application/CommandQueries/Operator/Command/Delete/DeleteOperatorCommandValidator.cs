using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Operator.Command.Delete
{
    public class DeleteOperatorCommandValidator : AbstractValidator<DeleteOperatorCommand>
    {
        public DeleteOperatorCommandValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

        }
    }
}