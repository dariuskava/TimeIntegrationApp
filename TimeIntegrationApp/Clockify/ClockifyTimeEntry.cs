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
        private DateTime? _start;
        private DateTime? _end;

        public DateTime? Start
        {
            get => _start;
            set
            {
                _start = value ?? throw new EmptyDateException(nameof(Start));
            }
        }

        public DateTime? End 
        { 
            get => _end;
            set
            {
                _end = value ?? throw new EmptyDateException(nameof(End), "Stop the Clockify counter and try again.");
            }
        }
        public String Duration { get; set; }
    }
}
