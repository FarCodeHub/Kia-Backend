using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Advertisement : BaseEntity
    {
        public string HostName { get; set; }
        public string Title { get; set; }
        public int AdvertisementTypeBaseId { get; set; }
        public int AdvertisementSourceBaseId { get; set; }
        public int Reputation { get; set; }
        public int HeadLineNumberBaseId { get; set; }
        public int FeedbackNumber { get; set; }
        public int Expense { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string? Descriptions { get; set; }


        public virtual BaseValue AdvertisementSourceBase { get; set; }
        public virtual BaseValue AdvertisementTypeBase { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual BaseValue HeadLineNumberBase { get; set; }
        public virtual User ModifiedBy { get; set; }
        public virtual Role OwnerRole { get; set; }
        public virtual ICollection<Communication> Communications { get; set; }
        public virtual ICollection<OutgoingAdvertismentMessage> OutgoingAdvertismentMessages { get; set; }
    }
}
