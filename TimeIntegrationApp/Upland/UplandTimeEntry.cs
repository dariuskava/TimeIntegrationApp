using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp.Upland
{
    class UplandTimeEntryReturn
    {
        public UplandTimeEntry TimeEntry { get; set; }
    }
    class UplandTimeEntry
    {
        public int UniqueId { get; set; }
        public string EntryDate { get;set; }
        public double RegularTime { get; set; }
        public string UserName { get; set; }
        public string TaskName { get; set; }
    
    }
}


