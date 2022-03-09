namespace Domain.Entities
{
    public partial class SessionSurvey : BaseEntity
    {
        public int TaskId { get; set; } = default!;
        public int QuestionId { get; set; } = default!;
        public int? AnswerId { get; set; } = default!;
        public string? Answer { get; set; }


        public virtual BaseValue AnswerNavigation { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Question Question { get; set; } = default!;
        public virtual Task Task { get; set; } = default!;
    }
}
