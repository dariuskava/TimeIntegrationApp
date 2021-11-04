using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeIntegrationApp.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string IntegrationName()
        {
            return IntegrationName(Id, Name);
        }
        public static string IntegrationName(string id, string name)
        {
            //reduce spaces. due to the clockify api doing the same behind the scenes
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
    }
}
