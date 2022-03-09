using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Communication : BaseEntity
    {
        public int? TypeBaseId { get; set; }
        public int? AdvertismentId { get; set; }
        public string? VoipUniqueNumber { get; set; }
        public string? CustomerConnectedNumber { get; set; }
        public string? SmsUniqueNumber { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }

        public virtual Advertisement? Advertisment { get; set; } = default!;
        public virtual User CreatedBy { get; set; } = default!;
        public virtual Customer Customer { get; set; } = default!;
        public virtual User? ModifiedBy { get; set; } = default!;
        public virtual Employee Employee { get; set; } = default!;
        public virtual Role OwnerRole { get; set; } = default!;
        public virtual BaseValue? TypeBase { get; set; } = default!;
        public virtual ICollection<Task> Tasks { get; set; } = default!;
    }
}
