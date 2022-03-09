using Application.CommandQueries.BaseValue.Command.Delete;
using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.BaseValueType.Command.Delete
{
    public class DeleteBaseValueTypeCommandValidator : AbstractValidator<DeleteBaseValueCommand>
    {
        public DeleteBaseValueTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");
        }
    }
}