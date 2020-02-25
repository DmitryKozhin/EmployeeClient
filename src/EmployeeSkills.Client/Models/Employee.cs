using System.Collections.Generic;

using Newtonsoft.Json;

namespace EmployeeSkills.Client.Models
{
    public class Employee
    {
        public Employee()
        {
            Skills = new List<Skill>();
        }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("skills")]
        public List<Skill> Skills { get; set; }
    }
}