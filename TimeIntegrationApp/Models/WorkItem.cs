using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp.Models
{
    public class WorkItem: IComparable<WorkItem>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ClientName { get; set; }
        public string ClientId { get; set; }
        public string IntegrationName()
        {
            return IntegrationName(Id, Name);
        }
        public static string IntegrationName(string id, string name)
        {
            name = name.Replace("\t", " ");
            while (name.Contains("  "))
                name = name.Replace("  ", " ");

            name = name.Trim();
            return $"{name}^{id}";
        }
        public static string parseId(string IntegrationName)
        {
            return IntegrationName.Split('^').Last();
        }
        public static string parseName(string IntegrationName)
        {
            return IntegrationName.Split('^').First();
        }
        public int CompareTo(WorkItem workItem)
        {
            if (workItem == null)
                return 1;
            else
                if (this.ProjectId == workItem.ProjectId)
                return this.Id.CompareTo(workItem.Id);
            else
                return this.ProjectId.CompareTo(workItem.ProjectId);
        }
    }
}
