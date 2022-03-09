using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Employee : BaseEntity
    {
        public int PersonId { get; set; } = default!;
        public int UnitPositionId { get; set; } = default!;
        public string EmployeeCode { get; set; } = default!;
        public DateTime EmploymentDate { get; set; } = default!;
        public DateTime? LeaveDate { get; set; }

        public virtual ICollection<Task> Tasks { get; set; } = default!;
        public virtual ICollection<Commission> Commissions { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Person Person { get; set; } = default!;
        public virtual UnitPosition UnitPosition { get; set; } = default!;
        public ICollection<Communication> Communications { get; set; }
        public virtual Operator Operator { get; set; } = default!;
        public virtual ICollection<Case> CasConsultants { get; set; }
        public virtual ICollection<Case> CasPresentors { get; set; }
        public virtual ICollection<CaseEmployeeChange> CaseEmployeeChanges { get; set; }
        public virtual ICollection<Census> Census { get; set; }

    }
}
