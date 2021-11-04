using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp.Clockify
{
    class ClockifyTask
    {
        public string Id { get; set; }
        public string Name { get;set; }
        public string ProjectId { get; set; }
        public string Status { get; set; }
        public string Estimate { get; set; }

    }
}
