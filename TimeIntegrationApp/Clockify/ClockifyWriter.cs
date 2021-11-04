using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Linq;
using TimeIntegrationApp.Models;


namespace TimeIntegrationApp.Clockify
{
    class ClockifyWriter:ITaskWriter
    {
        HttpClient client;
        readonly ClockifyReader Reader;
        readonly log4net.ILog Log;
        readonly IAuthenticationHandler AuthHandler;
        public ClockifyWriter(IAuthenticationHandler authHandler,log4net.ILog logger)
        {
            AuthHandler = authHandler;
            Reader = new ClockifyReader(AuthHandler);
            Log = logger;
        }

        public WriteStatus WriteWorkItems(List<TimeIntegrationApp.Models.WorkItem> workitems)
        {
            WriteStatus s = new WriteStatus();
            Project p = null;
            using (client = new HttpClient())
            {
                AuthHandler.AddAuthenticationHeaders(client.DefaultRequestHeaders);
                workitems.Sort();
                foreach (var v in workitems)
                {
                    if (p == null || p.Name != Project.IntegrationName(v.ProjectId, v.ProjectName))
                    {
                        p = FindOrCreateProject(v.ProjectId, v.ProjectName);
                        Log.Info($"Using project {p.Name}");
                    }
                    ClockifyTask t = FindOrCreateTask(p,v.Id, v.Name,s);
                    //TODO: close task
                }              
            }
            return s;
        }
        private Project FindOrCreateProject(string originalId, string name)
        {          
            var projList = Reader.ReadProjectsByName(Project.IntegrationName(originalId, name));
            if (projList.Count() == 0)
            {
                Project p = new Project
                {
                    Name = Project.IntegrationName(originalId,name)
                };
                var json = JsonSerializer.ToJson(p);
                var c = new StringContent(json,Encoding.UTF8,"application/json");
                var response = client.PostAsync(ApiUrl.Projects, c).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var Stringresp = response.Content.ReadAsStringAsync().Result;
                    Log.Error(Stringresp);
                }
                response.EnsureSuccessStatusCode();
                p = response.Content.ReadAsAsync<Project>().Result;
                Log.Info($"Created project {p.Id} {p.Name}");
                return p;
            }
            else
            {
                return projList.First();
            }
        }
        private ClockifyTask FindOrCreateTask(Project p,string originalId, string name,WriteStatus s)
        {
            var taskList = Reader.readTasksByName(p, WorkItem.IntegrationName(originalId, name));
            System.Threading.Thread.Sleep(100);
            if (taskList.Count() == 0)
            {

                ClockifyTask t = new ClockifyTask
                {
                    Name = WorkItem.IntegrationName(originalId,name)
                };
                var json = JsonSerializer.ToJson(t);
                var c = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(ApiUrl.Tasks(p.Id), c).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var Stringresp = response.Content.ReadAsStringAsync().Result;
                    Log.Error(Stringresp);
                }
                response.EnsureSuccessStatusCode();
                t = response.Content.ReadAsAsync<ClockifyTask>().Result;
                Log.Info($"Created task {t.Name}");
                s.created++;
                return t;
            }
            else
            {
                s.skipped++;
                return taskList.First();
            }

        }
        public TimeIntegrationApp.Models.WriteStatus WriteTimeEntries(List<TimeIntegrationApp.Models.TimeEntry> timeEntries)
        {
            return null;
        }

    }
}
