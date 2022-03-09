using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public partial class Census : BaseEntity
    {
        public int TotalTasks { get; set; }
        public int TotalCommunications { get; set; }
        public int TotalCases { get; set; }
        public int TotalCommmissions { get; set; }
        public int TotalContracts { get; set; }
        public int OpenCases { get; set; }
        public int ClosedCases { get; set; }
        public int PaidCommmissions { get; set; }
        public int UnPaidCommmissions { get; set; }
        public int IncomingCommunications { get; set; }
        public int OutgoingCommunications { get; set; }
        public int NotDoneTasks { get; set; }
        public int DoneTasks { get; set; }
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public int UnitId { get; set; }
        public int BranchId { get; set; }

        public virtual Branch Branch { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual User User { get; set; }
    }
}
