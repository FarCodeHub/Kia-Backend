using System;

namespace Domain.Entities
{
    public partial class Commission : BaseEntity
    {

        public int ContractId { get; set; } = default!;
        public long Amount { get; set; } = default!;
        public int EmployeeId { get; set; } = default!;
        public bool IsPaid { get; set; } = default!;
        public DateTime? PaidAt { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Contract Contract { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
    }
}
