using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp.Upland
{
    class UplandTimeRequest
    {
        public List<UplandNote> Notes { get; set; }
        public List<UplandKeyValue> KeyValues { get; set; }
    }
    class UplandNote
    {
        public int UniqueId { get; set; }
        public string Description { get; set; }
        public string NoteType { get; set; }
        public bool IsPublic { get; set; }
    }
    class UplandKeyValue
    {
        public bool IsAttribute { get; set; }
        public string Property { get; set; }
        public dynamic Value { get; set; }
    }

}
