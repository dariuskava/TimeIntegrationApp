using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Linq;
using TimeIntegrationApp.Models;
using Newtonsoft.Json;
using System;

namespace TimeIntegrationApp.Upland
{
    class UplandWriter : ITaskWriter
    {
        readonly string dateFormat = "MM/dd/yyyy";
        readonly log4net.ILog Log;
        Dictionary<DateTime, int> TimeSheetIds;
        readonly IAuthenticationHandler AuthHandler;
        public UplandWriter(IAuthenticationHandler authHandler, log4net.ILog logger)
        {
            AuthHandler = authHandler;
            Log = logger;
            TimeSheetIds = new Dictionary<DateTime, int>();
        }

        public WriteStatus WriteWorkItems(List<TimeIntegrationApp.Models.WorkItem> workitems)
        {
            return null;
        }
        public TimeIntegrationApp.Models.WriteStatus WriteTimeEntries(List<TimeIntegrationApp.Models.TimeEntry> timeEntries)
        {
            List<UplandTimeEntry> CreatedEntries = new List<UplandTimeEntry>();
            WriteStatus status = new WriteStatus();
            var PreparedEntries = PrepareEntries(timeEntries);
            ValidateEntries(PreparedEntries);
            try
            {
                foreach (var t in PreparedEntries)
                {
                    if (!TimeSheetIds.ContainsKey(t.Start.Date))
                    {
                        GetTimesheetInfo(t.Start);
                        Log.Info($"Got timesheet id {TimeSheetIds[t.Start.Date]} for {t.Start.Date}");
                    }
                    CreatedEntries.Add(UplandCreateOneTimeEntry(t, TimeSheetIds[t.Start.Date]));
                    status.created++;
                }
            }
            catch (HttpRequestException er)
            {
                Log.Error(er);
                foreach(var e in CreatedEntries)
                {
                    DeleteTimeEntry(e);
                }
                status = new WriteStatus();
            } 
            return status;
        }
        private void DeleteTimeEntry(UplandTimeEntry Entry)
        {
            using (var httpClient = new HttpClient())
            {
                AuthHandler.AddAuthenticationHeaders(httpClient.DefaultRequestHeaders);
                var response = httpClient.DeleteAsync(ApiUrl.OneTimeEntry(Entry.UniqueId)).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                if (!response.IsSuccessStatusCode)
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        Log.Error(result);
                    }
                }
                response.EnsureSuccessStatusCode();
            }
        }
        private void  GetTimesheetInfo(DateTime RefDate)
        {
            using (var httpClient = new HttpClient())
            {
                AuthHandler.AddAuthenticationHeaders(httpClient.DefaultRequestHeaders);
                var response = httpClient.GetAsync(ApiUrl.TimeSheetInfo(RefDate,((AuthenticationHandler)AuthHandler).Uid)).Result;
                response.EnsureSuccessStatusCode();
                var result = response.Content.ReadAsStringAsync().Result;
                UplandTimesheetInfo timesheet = JsonConvert.DeserializeObject<UplandTimesheetInfo>(result);
                DateTime start = DateTime.ParseExact(timesheet.StartDate, dateFormat, null, System.Globalization.DateTimeStyles.None).Date;
                DateTime end = DateTime.ParseExact(timesheet.EndDate, dateFormat, null,System.Globalization.DateTimeStyles.None).Date;
                for (DateTime date = start; date <= end; date = date.AddDays(1.0))
                {
                    TimeSheetIds[date.Date] = timesheet.UniqueId;
                }
            }
        }
        private UplandTimeEntry UplandCreateOneTimeEntry(TimeEntry timeEntry, int TimeSheetId)
        {
            using (var httpClient = new HttpClient())
            {
                AuthHandler.AddAuthenticationHeaders(httpClient.DefaultRequestHeaders);
                var entryRequest = new UplandTimeRequest
                {
                    Notes = new List<UplandNote>(),
                    KeyValues = new List<UplandKeyValue>()
                };
                entryRequest.Notes.Add(new UplandNote
                {
                    UniqueId = -1,
                    Description = timeEntry.Description,
                    NoteType = Config.UplandNoteType,
                    IsPublic = true
                });
                entryRequest.KeyValues.Add(new UplandKeyValue
                {
                    IsAttribute = true,
                    Property = "task",
                    Value = timeEntry.TaskId
                });
                entryRequest.KeyValues.Add(new UplandKeyValue
                {
                    IsAttribute = false,
                    Property = "EntryDate",
                    Value = timeEntry.Start.ToString(dateFormat)
                });
                entryRequest.KeyValues.Add(new UplandKeyValue
                {
                    IsAttribute = false,
                    Property = "RegularTime",
                    Value = timeEntry.TotalDuration
                });
                var body = "=" + Uri.EscapeDataString(JsonSerializer.ToJson(entryRequest));
                var content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = httpClient.PutAsync(ApiUrl.TimeEntryWithNote(TimeSheetId), content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    var Stringresp = response.Content.ReadAsStringAsync().Result;
                    Log.Error(timeEntry.Description);
                    Log.Error(Stringresp);
                }
                response.EnsureSuccessStatusCode();
                var uplandEntry = response.Content.ReadAsAsync<UplandTimeEntryReturn>().Result;
                Log.Info($"Created entry {uplandEntry.TimeEntry.UniqueId} on {uplandEntry.TimeEntry.EntryDate} for {uplandEntry.TimeEntry.RegularTime/60} mins {uplandEntry.TimeEntry.TaskName}");
                return uplandEntry.TimeEntry;
            }
        }
        private List<TimeEntry> PrepareEntries(List<TimeEntry> entries)
        {
            List<TimeEntry> result = new List<TimeEntry>();
            foreach (var entry in entries)
            {
                if (!entry.SkipIntegration)
                {
                    var r = result.Find(e => e.TaskId == entry.TaskId && e.Start.Date == entry.Start.Date);
                    if (r == null)
                    {
                        entry.TotalDuration = entry.DurationSec();
                        result.Add(entry);
                    }
                    else
                    {
                        result.Remove(r);
                        r.TotalDuration += entry.DurationSec();
                        if (!r.Description.Contains(entry.Description))
                            r.Description += "\r\n"+entry.Description;
                        result.Add(r);
                    }
                }
            }
            // round to 15 min
            foreach (var entry in result)
            {
                int duration = entry.TotalDuration;
                duration /= 900;
                duration *= 900;
                if (entry.TotalDuration % 900 > 400)
                    duration += 900;
                entry.TotalDuration = duration;
            }
            return result;
        }
        private void ValidateEntries(List<TimeEntry> entries)
        {
            bool hasError=false;
            foreach (var Entry in entries)
            {
                if (String.IsNullOrEmpty(Entry.TaskId))
                {
                    Log.Error($"{Entry.Start:d} - {Entry.Description}");
                    hasError = true;
                }
            }
            if (hasError)
            {
                throw new Exception("Writing terminated due to errors");
            }
        }
    }
}
