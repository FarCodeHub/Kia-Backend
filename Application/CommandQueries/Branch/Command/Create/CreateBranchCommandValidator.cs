using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Branch.Command.Create
{
    public class CreateBranchCommandValidator : AbstractValidator<CreateBranchCommand>
    {
        public CreateBranchCommandValidator()
        {
            RuleFor(x => x.Title)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x is { Length: < 50 }).WithMessage($"{ExceptionMessage.MaxLenght}:50");
        }
    }
}