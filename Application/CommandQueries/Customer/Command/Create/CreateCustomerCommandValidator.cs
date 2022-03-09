using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Customer.Command.Create
{
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.Phone1)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}");

        }
    }
}