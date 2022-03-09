using System.Linq;
using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace Application.CommandQueries.Employee.Command.Delete
{
    public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
    {
        public DeleteEmployeeCommandValidator(IRepository repository)
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.Id)
                .Must(x => !repository.GetQuery<Domain.Entities.User>().Any(t => t.Person.Employee.Id == x))
                .WithMessage($"{ExceptionMessage.InUse}");
        }
    }
}