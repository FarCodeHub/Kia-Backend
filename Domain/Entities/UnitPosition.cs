using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class UnitPosition : BaseEntity
    {

        public int UnitId { get; set; } = default!;
        public int PositionId { get; set; } = default!;


        public virtual Position Position { get; set; } = default!;
        public virtual Unit Unit { get; set; } = default!;
        public virtual ICollection<Employee> Employees { get; set; } = default!;
    }
}
