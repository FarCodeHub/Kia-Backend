using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Domain.Entities
{
    public partial class Branch : BaseEntity
    {

        public string Title { get; set; } = default!;
        public Geometry? Location { get; set; }
        public string? Address { get; set; } = default!;

        public virtual ICollection<Census> Census { get; set; }

        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<Unit> Units { get; set; } = default!;
    }
}
