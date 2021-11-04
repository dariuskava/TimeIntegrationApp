using System;
using System.Collections;
using System.Configuration;

namespace TimeIntegrationApp
{
    static class Config
    {
        public static string UplandBaseUrl
        {
            get { return ConfigurationManager.AppSettings["UplandBaseUrl"]; }
        }
        public static string UplandOrgName
        {
            get { return ConfigurationManager.AppSettings["UplandOrgName"]; }
        }
        public static string UplandApiVersion
        {
            get { return ConfigurationManager.AppSettings["UplandApiVersion"]; }
        }
        public static int UplandDaysBack
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["UplandDaysBack"]); }          
        }
        public static string UplandNoteType
        {
            get { return ConfigurationManager.AppSettings["UplandNoteType"]; }
        }
        public static string ProjectsToIntegrate
        {
            get { return ConfigurationManager.AppSettings["ProjectsToIntegrate"]; }
        }
        public static string Reader
        {
            //get { return ConfigurationManager.AppSettings["Reader"]; }
            get { return "Upland"; }
        }
        public static string Writer
        {
            //get { return ConfigurationManager.AppSettings["Writer"]; }
            get { return "Clockify"; }
        }
        public static string ClockifyBaseUrl
        {
            get { return ConfigurationManager.AppSettings["ClockifyBaseUrl"]; }
        }
        public static string ClockifyWorkspace
        {
            get { return ConfigurationManager.AppSettings["ClockifyWorkspace"]; }
        }

        public static string ClockifyIgnoreProjects
        {
            get { return ConfigurationManager.AppSettings["ClockifyIgnoreProjects"]; }
        }
    }
}
