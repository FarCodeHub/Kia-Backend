using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Project.Command.Create
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.Title)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x.Length <= 50).WithMessage($"{ExceptionMessage.MaxLenght}:50");

            RuleFor(x => x.Lat)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.Lng)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

            RuleFor(x => x.PriorityBaseId)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

        }
    }
}