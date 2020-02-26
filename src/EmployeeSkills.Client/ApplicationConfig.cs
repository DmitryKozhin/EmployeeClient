using System;

using Newtonsoft.Json;

namespace EmployeeSkills.Client
{
    public class ApplicationConfig
    {
        [JsonProperty("serverUrl")]
        public string ServerUrl { get; set; }
    }
}