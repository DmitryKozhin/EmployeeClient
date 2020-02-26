using System;

using Newtonsoft.Json;

namespace EmployeeSkills.Client
{
    public class ApplicationConfig
    {
        public string ServerUrl
        {
            get { return Environment.GetEnvironmentVariable("PERSONS_SERVER_URL"); }
        }
    }
}