using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Permission.Command.Update
{
    public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
    {
        public UpdatePermissionCommandValidator()
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