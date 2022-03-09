using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.BaseValue.Command.Create
{
    public class CreateBaseValueCommandValidator : AbstractValidator<CreateBaseValueCommand>
    {
        public CreateBaseValueCommandValidator()
        {
            RuleFor(x => x.Title)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x is { Length: < 250 }).WithMessage($"{ExceptionMessage.MaxLenght}:250");

            RuleFor(x => x.UniqueName)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x is { Length: < 50 }).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.Value)
                .Must(x => x == null != x is { Length: < 50 }).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.BaseValueTypeId)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");
        }
    }
}