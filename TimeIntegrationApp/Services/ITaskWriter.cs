using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp
{
    interface ITaskWriter
    {
        TimeIntegrationApp.Models.WriteStatus WriteWorkItems(List<TimeIntegrationApp.Models.WorkItem> workitems);
        TimeIntegrationApp.Models.WriteStatus WriteTimeEntries(List<TimeIntegrationApp.Models.TimeEntry> timeEntries);
    }

}
