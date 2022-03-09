using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Case : BaseEntity
    {
        public int CustomerId { get; set; }
        public int? ConsultantId { get; set; }
        public int? PresentorId { get; set; }
        public int StatusBaseId { get; set; }
        public DateTime? ClosedAt { get; set; }
        public bool IsOpen { get; set; }

        public virtual Employee? Consultant { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual User? ModifiedBy { get; set; }
        public virtual Role OwnerRole { get; set; }
        public virtual Employee? Presentor { get; set; }
        public virtual BaseValue StatusBase { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual ICollection<CaseEmployeeChange>? CaseEmployeeChanges { get; set; }
        public virtual ICollection<Task>? Tasks { get; set; }

    }
}
