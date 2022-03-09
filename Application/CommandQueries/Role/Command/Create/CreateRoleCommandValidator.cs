using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Role.Command.Create
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator()
        {
            RuleFor(x => x.Title)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x.Length <= 50).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.UniqueName)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x.Length <= 50).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.Description)
                .Must(x => x.Length <= 250).WithMessage($"{ExceptionMessage.MaxLenght}:250");

            RuleFor(x => x.PermissionsIdList)
                .Must(x => x != null && x.Count > 0).WithMessage($"{ExceptionMessage.IsRequired}");

        }
    }
}