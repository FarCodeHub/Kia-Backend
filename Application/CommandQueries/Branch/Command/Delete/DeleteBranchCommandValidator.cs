using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Branch.Command.Delete
{
    public class DeleteBranchCommandValidator : AbstractValidator<DeleteBranchCommand>
    {
        public DeleteBranchCommandValidator()
        {
            RuleFor(x => x.Id)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");
        }
    }
}