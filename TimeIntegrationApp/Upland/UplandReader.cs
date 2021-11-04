using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeIntegrationApp.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TimeIntegrationApp.Upland
{
    class UplandReader : ITaskReader
    {
        readonly IAuthenticationHandler AuthHandler;
        public UplandReader(IAuthenticationHandler authHandler)
        {
            AuthHandler = authHandler;
        }
        public List<WorkItem> ReadWorkItems(Project project)
        {
            using (var client = new HttpClient())
            {
                AuthHandler.AddAuthenticationHeaders(client.DefaultRequestHeaders);                
                DateTime dt = DateTime.Now;
                dt = dt.AddDays(-1 * Config.UplandDaysBack);
                string url = ApiUrl.TasksAfter(project,dt);
                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                List < UplandTask > tasks = JsonConvert.DeserializeObject<List<UplandTask>>(result);

                response = client.GetAsync(ApiUrl.Clients).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
                List<UplandClient> clients = JsonConvert.DeserializeObject<List<UplandClient>>(result);

                List<WorkItem> workItems = new List<WorkItem>();
                foreach (var task in tasks)
                {
                    WorkItem w = new WorkItem
                    {
                        Id = task.UniqueID.ToString(),
                        Name = task.Name,
                        Start = task.StartDate,
                        End = task.EndDate,
                        ProjectId = task.ProjectId.ToString()
                    };
                    w.ProjectName = project.Name;
                    w.ClientId = project.ClientId;
                    var c = clients.Find(x => x.UniqueID.ToString() == project.ClientId);
                    w.ClientName = c.Name;
                    workItems.Add(w);
                }
                return workItems;
            }
        }
        public List<Project> ReadProjects()
        {
            using (var client = new HttpClient())
            {
                AuthHandler.AddAuthenticationHeaders(client.DefaultRequestHeaders);
                DateTime dt = DateTime.Now;
                dt = dt.AddDays(-1 * Config.UplandDaysBack);
                int lastId = 0;
                List<Project> projects = new List<Project>();
                List<UplandProject> uplandProjects;
                do
                {
                    var response = client.GetAsync(ApiUrl.ProjectsAfter(dt, lastId)).Result;
                    response.EnsureSuccessStatusCode();
                    var result = response.Content.ReadAsStringAsync().Result;
                    uplandProjects = JsonConvert.DeserializeObject<List<UplandProject>>(result);
                    uplandProjects.ForEach(x =>
                        {
                            projects.Add(new Project
                            {
                                Id = x.UniqueID.ToString(),
                                Name = x.Name,
                                ClientId = x.ClientId.ToString()
                            });
                        });
                    if (uplandProjects.Any())
                        lastId = uplandProjects.Last().UniqueID;
                }
                while (uplandProjects.Count() == 500);
                return projects;
            }
        }
        public Project ReadProject(string id)
        {
            using (var client = new HttpClient())
            {
                AuthHandler.AddAuthenticationHeaders(client.DefaultRequestHeaders);
                var response = client.GetAsync(ApiUrl.ProjectById(id)).Result;
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsStringAsync().Result;
                UplandProject uplandProject = JsonConvert.DeserializeObject<UplandProject>(result);
                Project project = new Project
                {
                    Id = uplandProject.UniqueID.ToString(),
                    Name = uplandProject.Name,
                    ClientId = uplandProject.ClientId.ToString()
                };
                return project;
            };
            
        }
        public List<TimeIntegrationApp.Models.TimeEntry> ReadTimeEntries(DateTime from, DateTime to)
        {
            return null;
        }

    }
}

