namespace Domain.Entities
{
    public partial class Operator : BaseEntity
    {
        public string ExtentionNumber { get; set; } = default!;
        public string QueueNumber { get; set; } = default!;
        public int EmployeeId { get; set; } = default!;
        public bool IsActive { get; set; }
        public virtual User CreatedBy { get; set; } = default!;
        public virtual Employee Employee { get; set; } = default!;
        public virtual Role? ModifiedBy { get; set; } = default!;
        public virtual User? ModifiedByNavigation { get; set; } = default!;
    }
}
