using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TimeIntegrationApp.Clockify
{
    public static class ApiUrl
    {

        public static string Projects { get { return $"{Config.ClockifyBaseUrl}/workspaces/{Config.ClockifyWorkspace}/projects"; } }
        public static string Tasks(string projectId)
        {
            return $"{Config.ClockifyBaseUrl}/workspaces/{Config.ClockifyWorkspace}/projects/{projectId}/tasks";
        }
        public static string ProjectByName(string name)
        {
            return $"{Projects}?name={Uri.EscapeDataString(name)}";
        }
        public static string TaskByName(string project, string name)
        {
            return $"{Tasks(project)}?name={Uri.EscapeDataString(name)}";
        }
        public static string TimeEntries(string user)
        {
            return $"{Config.ClockifyBaseUrl}/workspaces/{Config.ClockifyWorkspace}/user/{user}/time-entries?hydrated=true";
        }
        public static string TimeEntries(DateTime from, DateTime to, string user)
        {
            return $"{TimeEntries(user)}&start={from:yyyy-MM-ddT00:00:00Z}&end={to:yyyy-MM-ddT23:59:59.999Z}";
        }
        public static string User { get { return $"{Config.ClockifyBaseUrl}/user"; } }
    }
}
