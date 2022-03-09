using System.Collections.Generic;
using Infrastructure.Interfaces;

namespace Domain.Entities
{
    public partial class Permission : BaseEntity, IHierarchical
    {
        public int? ParentId { get; set; }
        public string LevelCode { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string UniqueName { get; set; } = default!;


        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual Permission? Parent { get; set; } = default!;
        public virtual ICollection<Permission> InverseParent { get; set; } = default!;
        public virtual ICollection<MenuItem> MenuItems { get; set; } = default!;
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = default!;
    }
}
