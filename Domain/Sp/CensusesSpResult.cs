// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.reverse
{
    public partial class CensusesSpResult
    {
        public int TotalTasks { get; set; }
        public int TotalCommunications { get; set; }
        public int TotalCases { get; set; }
        public int TotalCommmissionsAmount { get; set; }
        public int TotalCommmissionsCount { get; set; }
        public int TotalContracts { get; set; }
        public int OpenCases { get; set; }
        public int ClosedCases { get; set; }
        public int PaidCommmissionsAmount { get; set; }
        public int UnPaidCommmissionsAmount { get; set; }
        public int PaidCommmissionsCount { get; set; }
        public int UnPaidCommmissionsCount { get; set; }
        public int IncomingCommunications { get; set; }
        public int OutgoingCommunications { get; set; }
        public int NotDoneTasks { get; set; }
        public int DoneTasks { get; set; }
    }
}
