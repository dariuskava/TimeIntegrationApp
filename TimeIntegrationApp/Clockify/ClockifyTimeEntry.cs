using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeIntegrationApp.Models;

namespace TimeIntegrationApp.Clockify
{
    class ClockifyTimeEntry
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public List<ClockifyTag> Tags { get; set; }
        public WorkItem Task { get; set; }
        public Project Project { get; set; }
        public ClockifyInterval TimeInterval { get; set; }
    }
    class ClockifyTag
    {
        public String Name { get; set; }
    }
    class ClockifyInterval
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public String Duration { get; set; }
    }
}
