using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp.Upland
{
    class UplandTimesheetInfo
    {
        public int UniqueId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<UplandTimeEntry> TimeEntries {get;set;}
    }
}
