using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Question.Command.Create
{
    public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator()
        {
            RuleFor(x => x.Title)
                .Must(x => x != null).WithMessage($"{ExceptionMessage.IsRequired}")
                .Must(x => x.Length <= 50).WithMessage($"{ExceptionMessage.MaxLenght}:50");
            RuleFor(x => x.AnswerOptionBaseTypeId)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");
            RuleFor(x => x.QuestionTypBaseId)
                .Must(x => x > 0).WithMessage($"{ExceptionMessage.IsRequired}");

        }
    }
}