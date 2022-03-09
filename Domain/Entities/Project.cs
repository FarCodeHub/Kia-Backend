using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace Domain.Entities
{
    public partial class Project : BaseEntity,ICloneable
    {
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string FileUrl { get; set; }
        [System.Runtime.Serialization.DataMember]
        public Geometry Location { get; set; }
        public string Address { get; set; }
        public int PriorityBaseId { get; set; }

        public virtual ICollection<Project> InverseParent { get; set; } = default!;
        public virtual Project? Parent { get; set; }
        public virtual User CreatedBy { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual BaseValue PriorityBase { get; set; } = default!;
        public virtual ICollection<Task> Tasks { get; set; } = default!;
        public virtual ICollection<ContractProject> ContractProjects { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
