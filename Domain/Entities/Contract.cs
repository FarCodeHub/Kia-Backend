using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Contract : BaseEntity
    {
        public int TaskId { get; set; } = default!;
        public string? Descriptions { get; set; }
        public int CaseId { get; set; }

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Task Task { get; set; } = default!;
        public virtual ICollection<Commission> Commissions { get; set; } = default!;
        public virtual Case Case { get; set; } = default!;
        public virtual ICollection<ContractAttachment> ContractAttachments { get; set; } = default!;
        public virtual ICollection<ContractProject> ContractProjects { get; set; }

    }
}
