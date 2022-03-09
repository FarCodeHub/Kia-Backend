using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.BaseValueType.Command.Create
{
    public class CreateBaseValueTypeCommandValidator : AbstractValidator<CreateBaseValueTypeCommand>
    {
        public CreateBaseValueTypeCommandValidator()
        {
            RuleFor(x => x.Title)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x is { Length: < 50 }).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.UniqueName)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x is { Length: < 50 }).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.GroupName)
                .Must(x => x == null != x is { Length: < 50 }).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.SubSystem)
                .Must(x => x == null != x is { Length: < 50 }).WithMessage($"{ExceptionMessage.MaxLenght}");
        }
    }
}