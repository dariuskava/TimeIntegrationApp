using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp.Models
{
    class TimeEntry
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool SkipIntegration { get; set; }
        public string TaskId { get; set; }
        public string TaskName { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int TotalDuration { get; set; }
        public int DurationSec()
        {
            return Convert.ToInt32(Math.Truncate((End-Start).TotalSeconds));
        }

    }
}
