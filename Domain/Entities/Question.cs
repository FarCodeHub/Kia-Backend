using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Question : BaseEntity
    {
        public int QuestionTypBaseId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int AnswerTypeBaseId { get; set; } = default!;
        public int? AnswerOptionBaseTypeId { get; set; }


        public virtual BaseValueType? AnswerOptionBaseType { get; set; } = default!;
        public virtual BaseValue AnswerTypeBase { get; set; } = default!;
        public virtual BaseValue QuestionTypBase { get; set; } = default!;
        public virtual ICollection<SessionSurvey> SessionSurveys { get; set; } = default!;
    }
}
