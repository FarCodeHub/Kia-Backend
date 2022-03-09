using FluentValidation;
using Infrastructure.Enums;

namespace Application.CommandQueries.Question.Command.Update
{
    public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidator()
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