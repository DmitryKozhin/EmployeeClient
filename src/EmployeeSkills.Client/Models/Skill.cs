﻿using Newtonsoft.Json;

namespace EmployeeSkills.Client.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
    }
}