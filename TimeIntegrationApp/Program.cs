using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;

namespace TimeIntegrationApp
{
    public static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Program));
        static int Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            var cmd = new RootCommand
            {                
                new Command("tasks", "Sync tasks")
                {
                    new Option<string>(new[] {"--project", "-pr"},description: "Project id to sync"),
                    new Option<string> (new []{"--password1","-p1"},description: $"Password for {Config.Reader}"),
                    new Option<string> (new[]{"--user1","-u"},description: $"Username for {Config.Reader}"),
                    new Option<int> (new[]{"--uid1","-id"},description: $"User ID for {Config.Reader}"),
                    new Option<string> (new []{"--password2","-p2"},description: $"Password for {Config.Writer}")
                }.WithHandler(nameof(SyncTasks)),
                new Command("projects","List projects")
                {
                    new Option<bool>(new []{"--all","-a" },description: "List all projects"),
                    new Option<string> (new []{"--password1","-p1"},description: $"Password for {Config.Reader}"),
                    new Option<string> (new[]{"--user1","-u"},description: $"Username for {Config.Reader}"),
                    new Option<int> (new[]{"--uid1","-id"},description: $"User ID for {Config.Reader}"),
                    new Option<string> (new []{"--password2","-p2"},description: $"Password for {Config.Writer}")
                }.WithHandler(nameof(ListProjects)),
                new Command("time","Update Time Entries")
                {
                    new Option<DateTime>(new []{"--start","-s" },description: "Time entries from date"),
                    new Option<DateTime>(new []{"--end","-e" },description: "Time entries till date"),                  
                    new Option<string> (new []{"--password1","-p1"},description: $"Password for {Config.Reader}"),
                    new Option<string> (new[]{"--user1","-u"},description: $"Username for {Config.Reader}"),
                    new Option<int> (new[]{"--uid1","-id"},description: $"User ID for {Config.Reader}"),
                    new Option<string> (new []{"--password2","-p2"},description: $"Password for {Config.Writer}")
                }.WithHandler(nameof(TimeEntries)),

            };
            cmd.TreatUnmatchedTokensAsErrors = true;
            return cmd.Invoke(args);
        }

        static int SyncTasks(string Password1,string Password2, string Project, string user1, int uid1, IConsole console)
        {
            new Services.Integrator(Password1,user1,uid1,Password2,log).SyncTasks(Project);
            return 0;
        }
        static int ListProjects(string Password1, string Password2, bool all, string user1, int uid1,IConsole console)
        {
            new Services.Integrator(Password1, user1, uid1, Password2, log).ListProjects(all);
            return 0;
        }
        static int TimeEntries(string Password1, string Password2, DateTime start, DateTime end,string user1, int uid1)
        {
            new Services.Integrator(Password1, user1,uid1,Password2, log).TimeEntries(start, end);
            return 0;
        }

        private static Command WithHandler(this Command command, string name)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Static;
            var method = typeof(Program).GetMethod(name, flags);

            var handler = CommandHandler.Create(method);
            command.Handler = handler;
            return command;
        }
    }

}
