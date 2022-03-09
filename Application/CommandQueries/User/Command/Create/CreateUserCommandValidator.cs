using System.Linq;
using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.CommandQueries.User.Command.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IRepository repository)
        {
            RuleFor(x => x.Password)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x is { Length: < 50 }).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.Username)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x.Length <= 50).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.Username)
                .Must(x => !(repository.GetQuery<Domain.Entities.User>().Any(t => t.Username == x)))
                .WithMessage($"{ExceptionMessage.Doublicate}")
             ;

            RuleFor(x => x.RolesIdList)
                .Must(x => x != null && x.Count > 0).WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.PersonId)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");
        }
    }
}