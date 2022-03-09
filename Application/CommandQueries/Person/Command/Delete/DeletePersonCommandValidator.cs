using System.Linq;
using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace Application.CommandQueries.Person.Command.Delete
{
    public class DeletePersonCommandValidator : AbstractValidator<DeletePersonCommand>
    {
        public DeletePersonCommandValidator(IRepository repository)
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.Id)
                .Must(x => !repository.GetQuery<Domain.Entities.Employee>().Any(t => t.PersonId == x))
                .WithMessage($"{ExceptionMessage.InUse}");

            RuleFor(x => x.Id)
                .Must(x => !repository.GetQuery<Domain.Entities.Customer>().Any(t => t.PersonId == x))
                .WithMessage($"{ExceptionMessage.InUse}");
        }
    }
}