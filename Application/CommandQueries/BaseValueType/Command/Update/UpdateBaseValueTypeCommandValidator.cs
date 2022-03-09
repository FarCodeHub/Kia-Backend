using Application.CommandQueries.BaseValueType.Command.Create;
using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.BaseValueType.Command.Update
{
    public class UpdateBaseValueTypeCommandValidator : AbstractValidator<CreateBaseValueTypeCommand>
    {
        public UpdateBaseValueTypeCommandValidator()
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