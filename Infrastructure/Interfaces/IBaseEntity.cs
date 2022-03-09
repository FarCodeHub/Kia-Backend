using System;

namespace Infrastructure.Interfaces
{
    public interface IBaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int OwnerRoleId { get; set; }
        public int CreatedById { get; set; }
    }
}