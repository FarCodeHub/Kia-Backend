using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.User.Command.Delete
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.Id)
                .Must(x => false).WithMessage($"{ExceptionMessage.InUse}");
        }
    }
}