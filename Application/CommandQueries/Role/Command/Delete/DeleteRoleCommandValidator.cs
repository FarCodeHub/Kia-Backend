using System.Linq;
using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace Application.CommandQueries.Role.Command.Delete
{
    public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
    {
        public DeleteRoleCommandValidator(IRepository repository)
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.Id)
                .Must(x => !repository.GetQuery<Domain.Entities.RolePermission>().Any(t => t.RoleId == x))
                .WithMessage($"{ExceptionMessage.InUse}");

            RuleFor(x => x.Id)
                .Must(x => !repository.GetQuery<Domain.Entities.UserRole>().Any(t => t.RoleId == x))
                .WithMessage($"{ExceptionMessage.InUse}");
        }
    }
}