using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeIntegrationApp.Models;

namespace TimeIntegrationApp
{
    interface ITaskReader
    {
        List<WorkItem> ReadWorkItems(Project p);
        List<Project> ReadProjects();
        Project ReadProject(string id);
        (bool, List<TimeIntegrationApp.Models.TimeEntry>) ReadTimeEntries(DateTime from, DateTime to, log4net.ILog log);
    }
}
