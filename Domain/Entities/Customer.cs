using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Customer : BaseEntity
    {
        public int PersonId { get; set; } = default!;
        public bool IsBlocked { get; set; }

        public virtual  Person Person { get; set; }
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual ICollection<Communication> Communications { get; set; } = default!;
        public virtual ICollection<Task> Tasks { get; set; } = default!;
        public virtual ICollection<Case> Cases { get; set; }

    }
}
