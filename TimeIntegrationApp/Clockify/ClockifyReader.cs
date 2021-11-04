using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeIntegrationApp.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace TimeIntegrationApp.Clockify
{
    class ClockifyReader : ITaskReader
    {
        readonly IAuthenticationHandler AuthHandler;
        public ClockifyReader(IAuthenticationHandler authHandler)
        {
            AuthHandler = authHandler;
        }
        public List<Project> ReadProjectsByName(string name)
        {
            using (var client = new HttpClient())
            {
                AuthHandler.AddAuthenticationHeaders(client.DefaultRequestHeaders);
                var response = client.GetAsync(ApiUrl.ProjectByName(name)).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                List<Project> projects = JsonConvert.DeserializeObject<List<Project>>(result);
                return projects;
            }
        }
        public List<ClockifyTask> readTasksByName(Project p, string name)
        {
            using (var client = new HttpClient())
            {
                AuthHandler.AddAuthenticationHeaders(client.DefaultRequestHeaders);
                var response = client.GetAsync(ApiUrl.TaskByName(p.Id, name)).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                List<ClockifyTask> tasks = JsonConvert.DeserializeObject<List<ClockifyTask>>(result);
                return tasks;
            }
        }

        public List<TimeEntry> ReadTimeEntries(DateTime from, DateTime to)
        {
            using (var client = new HttpClient())
            {
                AuthHandler.AddAuthenticationHeaders(client.DefaultRequestHeaders);
                string user = getUserId(client);
                var response = client.GetAsync(ApiUrl.TimeEntries(from,to,user)).Result;

                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                List<ClockifyTimeEntry> entries = JsonConvert.DeserializeObject<List<ClockifyTimeEntry>>(result);
                List<TimeEntry> timeEntries = new List<TimeEntry>();
                entries.ForEach(x =>
                {
                    timeEntries.Add(new TimeEntry
                    {

                        Id = x.Id,
                        Description = x.Description,
                        SkipIntegration = x.Tags.Exists(tag => tag.Name == "Skip") || (x.Project != null && Config.ClockifyIgnoreProjects.Contains(x.Project.Name)),
                        TaskId = (x.Task == null) ? null : WorkItem.parseId(x.Task.Name),
                        TaskName = (x.Task == null) ? null : WorkItem.parseId(x.Task.Name),
                        ProjectId = (x.Project == null) ? null : Project.parseId(x.Project.Name),
                        ProjectName = (x.Project == null) ? null : Project.parseId(x.Project.Name),
                        Start = x.TimeInterval.Start,
                        End = x.TimeInterval.End
                    }); ;
                });
                return timeEntries;
            }
        }
        protected string getUserId(HttpClient httpClient)
        {
            var response = httpClient.GetAsync(ApiUrl.User).Result;
            response.EnsureSuccessStatusCode();
            var User = response.Content.ReadAsAsync<User>().Result;
            return User.Id;
        }
        public List<WorkItem> ReadWorkItems(Project p)
        {
            return null;
        }
        public List<Project> ReadProjects()
        {
            return null;
        }
        public Project ReadProject(string id)
        {
            return null;
        }

    }
}

