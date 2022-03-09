using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Permission.Command.Delete
{
    public class DeletePermissionCommandValidator : AbstractValidator<DeletePermissionCommand>
    {
        public DeletePermissionCommandValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");
        }
    }
}