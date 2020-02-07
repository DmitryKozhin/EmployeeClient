using System;
using System.Collections.Generic;

namespace EmployeeSkills.Client.Models
{
    public class Employee
    {
        public Employee()
        {
            Skills = new List<Skill>();
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
        public List<Skill> Skills { get; set; }
    }
}