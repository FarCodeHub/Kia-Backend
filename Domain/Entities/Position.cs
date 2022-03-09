using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Position : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string UniqueName { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<UnitPosition> UnitPositions { get; set; } = default!;
        public virtual ICollection<CaseEmployeeChange> CaseEmployeeChanges { get; set; }

    }
}
