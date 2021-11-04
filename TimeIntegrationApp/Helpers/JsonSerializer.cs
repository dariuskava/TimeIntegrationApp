
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TimeIntegrationApp
{
    class JsonSerializer
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DefaultValueHandling = DefaultValueHandling.Include,
            NullValueHandling = NullValueHandling.Ignore,
        };
        public static string ToJson(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.None, JsonSettings);
        }

        public static object FromJson(string json, System.Type type)
        {
            return JsonConvert.DeserializeObject(json, type, JsonSettings);
        }
    }
}
