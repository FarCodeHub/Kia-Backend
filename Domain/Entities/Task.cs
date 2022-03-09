using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Task : BaseEntity
    {
        public int? ParentId { get; set; }
        public int? CommunicationId { get; set; }
        public int TypeBaseId { get; set; }

        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime DuoAt { get; set; }
        public DateTime? EndAt { get; set; }
        public int Status { get; set; }
        public string? StatusTitle { get; set; }
        public int? ResultBaseId { get; set; }
        public int CaseId { get; set; }
        public string? Descriptions { get; set; }
        public virtual Communication? Communication { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual Customer Customer { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Employee Employee { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Task? Parent { get; set; } = default!;
        public virtual Project? Project { get; set; } = default!;
        public virtual BaseValue? ResultBase { get; set; } = default!;
        public virtual BaseValue TypeBase { get; set; } = default!;
        public virtual ICollection<Contract> Contracts { get; set; } = default!;
        public virtual ICollection<Task> InverseParent { get; set; } = default!;
        public virtual ICollection<SessionSurvey> SessionSurveys { get; set; } = default!;
        public virtual Case Case { get; set; }

    }
}
