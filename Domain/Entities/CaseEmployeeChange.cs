namespace Domain.Entities
{
    public partial class CaseEmployeeChange : BaseEntity
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeePositionId { get; set; }
        public string Reason { get; set; }


        public virtual Case Case { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Position EmployeePosition { get; set; }
        public virtual User ModifiedBy { get; set; }
        public virtual Role OwnerRole { get; set; }
    }
}
