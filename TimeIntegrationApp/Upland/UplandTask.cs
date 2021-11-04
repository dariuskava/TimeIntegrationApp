using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeIntegrationApp.Models;

namespace TimeIntegrationApp.Upland
{
    public class UplandTask:WorkItem
    {
        public string ID { get; set; }
        public int UniqueID { get; set; }

        public string TaskType { get; set; }
        public string TaskRate { get; set; }
        public string Description { get; set; }
        public int AccessType { get; set; }
        public int IsBillable { get; set; }
        public int IsPayable { get; set; }
        public int IsFunded { get; set; }
        public int IsRandD { get; set; }
        public int Capitalized { get; set; }
        public int PortfolioId { get; set; }
        public int IsETCEnabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int IsSuspended { get; set; }
        public int IsOverhead { get; set; }
        public string State { get; set; }
        public string Priority { get; set; }
        public int IsMilestoneBilling { get; set; }
        public int WorktypeId { get; set; }
        public string TimeEntryNotesOption { get; set; }
    }
}
