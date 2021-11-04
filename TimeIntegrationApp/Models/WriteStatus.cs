using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp.Models
{
    class WriteStatus
    {
        public int created { get; set; }
        public int updated { get; set; }
        public int skipped {get;set;}
        public WriteStatus( )
        {
            created = 0;
            updated = 0;
            skipped = 0;
        }
    }
}
