# Time Integration App
Program designed to simplify time entry to Upland PSA (Tenrox) timesheets. 
It exports available tasks from Upland PSA to [Clockify](https://clockify.me) and imports time registrations to Upland PSA timesheets.

## Configuration

Program configuration is done by editing supplied .config file.
Available options:
* UplandBaseUrl - Url to your Upland PSA. *https://ORGANISATION_NAME.tenrox.net/TEnterprise*
* UplandApiVersion - for most api calls appended to an Url. *v2*
* UplandUser - your username. *name@organisation.com*
* UplandPassword - password. **Used only for development purposes. Never save password for daily use. Use command line switch instead**
* UplandOrgName - your organization name.
* UplandDaysBack - how many days look backwards. Integration requests will include tasks and projects where ending date is after *now - UplandDaysBack*.
* UplandUserId - unique id for the user. 
* UplandNoteType - note type for timesheet entries
* ProjectsToIntegrate - unique ids of the projects to integrate. Separated by comma. E.g. *138,147*
* ClockifyBaseUrl - Url to clockify. *https://api.clockify.me/api/v1*
* ClockifyApiKey - Api key for clockify. **Used only for development purposes. Never save key for daily use. Use command line switch instead**
* ClockifyWorkspace - Clockify workspace id used for integration


## Usage 

```
  TimeIntegrationApp [options] [command]

Options:
  --version         Show version information
  -?, -h, --help    Show help and usage information

Commands:
  tasks       Sync tasks
  projects    List projects
  time        Update Time Entries
```

### Tasks 
Command for syncronizing tasks from Upland to Clockify
```
Usage:
  TimeIntegrationApp tasks [options]

Options:
  -p, --project <project>         Project id to sync. If not specified all configured projects are syncronized
  -u, --user1 <username>		  UserName for Upland
  -id, --uid1 <id>	              Upland unique user ID
  -p1, --password1 <password1>    Password for Upland
  -p2, --password2 <password2>    Password for Clockify
  -?, -h, --help                  Show help and usage information
```

### Projects
Command to list available projects 

```
Usage:
  TimeIntegrationApp projects [options]

Options:
  -a, --all                       List all projects. If not specificed only projects from configuration file are listed
  -u, --user1 <username>		  UserName for Upland
  -id, --uid1 <id>	              Upland unique user ID
  -p1, --password1 <password1>    Password for Upland
  -p2, --password2 <password2>    Password for Clockify
  -?, -h, --help                  Show help and usage information
```

### Time

Command for time entry syncronization from Clockify to Upland

```
Usage:
  TimeIntegrationApp time [options]

Options:
  -s, --start <start>             Time entries from date
  -e, --end <end>                 Time entries till date
  -u, --user1 <username>		  UserName for Upland
  -id, --uid1 <id>	              Upland unique user ID
  -p1, --password1 <password1>    Password for Upland
  -p2, --password2 <password2>    Password for Clockify

  -?, -h, --help                  Show help and usage information
```

## Examples
### Sync of all the projects and tasks to Clockify
```
.\TimeIntegrationApp.exe tasks --user1 <UplandUsername> --uid1 <UplandUserId> --password1 <UplandPassword> --password2 <ApiKey>
```
### Sync of tasks for single project 
```
.\TimeIntegrationApp.exe tasks --user1 <UplandUsername> --uid1 <UplandUserId> --password1 <UplandPassword> --password2 <ApiKey> --project 414
```
### Sync time entries for a period
```
.\TimeIntegrationApp.exe time --user1 <UplandUsername> --uid1 <UplandUserId> --password1 <UplandPassword> --password2 <ApiKey> -s '2021-03-08' -e '2021-03-14'
```