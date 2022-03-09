using System.Collections.Generic;
using Infrastructure.Interfaces;

namespace Domain.Entities
{
    public partial class Unit : BaseEntity, IHierarchical
    {

        public int BranchId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string LevelCode { get; set; } = default!;
        public int? ParentId { get; set; }


        public virtual Branch Branch { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Unit? Parent { get; set; } = default!;
        public virtual ICollection<Unit> InverseParent { get; set; } = default!;
        public virtual ICollection<Census> Census { get; set; }

        public virtual ICollection<UnitPosition> UnitPositions { get; set; } = default!;
    }
}
