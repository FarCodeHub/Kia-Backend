using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace Application.CommandQueries.BaseValue.Command.Delete
{
    public class DeleteBaseValueCommandValidator : AbstractValidator<DeleteBaseValueCommand>
    {
        public DeleteBaseValueCommandValidator(IRepository repository)
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.Id)
                .Must(x => false /*!repository.GetQuery<Domain.Entities.Person>().Any(t => t.GenderBaseId == x)*/)
                .WithMessage($"{ExceptionMessage.InUse}");
        }
    }
}