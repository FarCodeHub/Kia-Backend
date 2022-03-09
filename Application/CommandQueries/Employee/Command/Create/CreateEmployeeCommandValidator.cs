using FluentValidation;
using Infrastructure.Enums;
using Infrastructure.Interfaces;

namespace Application.CommandQueries.Employee.Command.Create
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator(IRepository repository)
        {
            //RuleFor(x => x.ExtentionNumber)
            //    .Must(x => !string.IsNullOrEmpty(x) && repository.GetQuery<Domain.Entities.Operator>()
            //        .Any(t => t.IsActive && t.ExtentionNumber == x))
            //    .WithMessage($"{ExceptionMessage.InUse}");

            RuleFor(x => x.Phone1)
                .Must(x => !string.IsNullOrEmpty(x))
                    .WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.FirstName)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.LastName)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.NationalCode)
                .Must(x => !string.IsNullOrEmpty(x))
                .WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.ExtentionNumber)
                .Must(x => string.IsNullOrEmpty(x) != (!string.IsNullOrEmpty(x) && x.Length < 6))
                .WithMessage($"{ExceptionMessage.MaxLenght}:6");


            RuleFor(x => x.BirthPlaceId)
                .Must(x => x > 0)
                .WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.UnitPositionId)
                .Must(x => x > 0)
                .WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.ZipCodeId)
                .Must(x => x > 0)
                .WithMessage($"{ExceptionMessage.IsRequired}");

        }
    }
}   