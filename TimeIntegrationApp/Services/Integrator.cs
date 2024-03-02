using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeIntegrationApp.Models;
namespace TimeIntegrationApp.Services
{

    public class Integrator
    {
        private readonly log4net.ILog log;
        private readonly ITaskReader TaskReader,TimeReader;
        private readonly ITaskWriter TaskWriter,TimeWriter;
        private readonly IAuthenticationHandler ClockifyAuthHandler;
        private readonly Upland.AuthenticationHandler UplandAuthHandler;

        public Integrator(string secret1, string userName1, int Uid, string secret2, log4net.ILog logger)
        {
            log = logger;
            switch (Config.Reader)
            {
                case "Upland":
                    UplandAuthHandler = new Upland.AuthenticationHandler(secret1, log);
                    UplandAuthHandler.SetUser(userName1, Uid);
                    TaskReader = new Upland.UplandReader(UplandAuthHandler);
                    TimeWriter = new Upland.UplandWriter(UplandAuthHandler,log);
                    break;
            }
            switch(Config.Writer)
            {
                case "Clockify":
                    ClockifyAuthHandler = new Clockify.AuthenticationHandler(secret2, log);
                    TaskWriter = new Clockify.ClockifyWriter(ClockifyAuthHandler,log);
                    TimeReader = new Clockify.ClockifyReader(ClockifyAuthHandler);
                    break;
            }
        }

        public void SyncTasks (string projectId="")
        {
            List<Project> projects;
            if (String.IsNullOrEmpty(projectId))
            {
                projects = ReadProjectList();
            }
            else
            {
                projects = new List<Project>();
                Project p = TaskReader.ReadProject(projectId);
                projects.Add(p);
            }
            log.Info($"Syncronizing {projects.Count} projects");
            foreach (var p in projects)
            {
                syncOneProjectTasks(p);
            }
            
        }

        public void ListProjects(bool All=false)
        {
            var projects = ReadProjectList(All);
            foreach (var p in projects)
            {
                log.Info(p.IntegrationName());
            }
        }

        public void TimeEntries(DateTime from,DateTime to)
        {
            log.Info($"Time entry integration from {from:yyyy-MM-dd} to {to:yyyy-MM-dd}");

            for (DateTime date = from; date <= to; date = date.AddDays(1.0))
            {
                log.Info($"{date:yyyy-MM-dd}");
                (bool successfulReading, var entries) = TimeReader.ReadTimeEntries(date,date, log);
                if (successfulReading)
                {
                    log.Info($"Read {entries.Count} entries");
                    WriteStatus s = TimeWriter.WriteTimeEntries(entries);
                    log.Info($"Created {s.created} entries");
                } else
                {
                    break;
                }
            }

        }

        private List<Project> ReadProjectList(bool all=false)
        {
            List<Project> projects;
            Project p;
            if (String.IsNullOrEmpty(Config.ProjectsToIntegrate) || all)          
            {
                projects = TaskReader.ReadProjects();
            }
            else
            {
                projects = new List<Project>();
                var projectsToIntegrate = Config.ProjectsToIntegrate.Split(',');
                foreach (var projId in projectsToIntegrate)
                {
                    p = TaskReader.ReadProject(projId);
                    projects.Add(p);
                }
            }

            return projects;
        }
        private void syncOneProjectTasks(Project project)
        {
            var tasks = TaskReader.ReadWorkItems(project);
            log.Info($"Read {tasks.Count} tasks from project {project.Id} {project.Name}");
            var s = TaskWriter.WriteWorkItems(tasks);
            log.Info($"Created: {s.created}, Updated: {s.updated}, Skipped: {s.skipped}");
        }

    }
}
