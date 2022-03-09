using System;

namespace Infrastructure.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int OwnerRoleId { get; set; }
        public int CreatedById { get; set; }

    }
}