using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeIntegrationApp.Models;

namespace TimeIntegrationApp.Upland
{
    public static class ApiUrl
    {

        public static string Token { get { return $"{Config.UplandBaseUrl}/api/token"; } }
        public static string Clients { get { return $"{Config.UplandBaseUrl}/api/{Config.UplandApiVersion}/Clients";  } }
        public static string Projects { get { return $"{Config.UplandBaseUrl}/api/{Config.UplandApiVersion}/Projects";  } }
        public static string Tasks { get { return $"{Config.UplandBaseUrl}/api/{Config.UplandApiVersion}/Tasks";  } } 
        public static string TimeEntries { get { return $"{Config.UplandBaseUrl}/api/{Config.UplandApiVersion}/TimeEntries"; } }
        public static string OneTimeEntry(int UniqueId) { 
            return $"{TimeEntries}/{UniqueId}";  
        }
        public static string TimeSheetInfo(DateTime RefDate,int Uid)
        {
            return $"{Config.UplandBaseUrl}/api/Timesheets/?UserId={Uid}&anyDate={RefDate:MM-dd-yyyy}";
        }
        public static string TimeEntryWithNote(int TimeSheetId)
        {
            return $"{Config.UplandBaseUrl}/api/Timesheets/{TimeSheetId}/?property=TIMEENTRYLITE";
        }
        public static string TasksAfter(Project project, DateTime dt)
        {
            string filter = $"?$filter=EndDate gt datetime'{dt:yyyy-MM-dd}' and ProjectId eq {Convert.ToInt32(project.Id)}";
            return Tasks + filter;
        }
        public static string ProjectsAfter(DateTime dt,int UniqueId = 0)
        {
            string filter = $"?$filter=EndDate gt datetime'{dt:yyyy-MM-dd}'";
            if (UniqueId > 0)
                filter = filter + $" and UniqueId gt {UniqueId}";
            return Projects + filter;
        }
        public static string ProjectById(string id)
        {
            return $"{Projects}/{id}";
        }
    }
}
