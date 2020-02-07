using System;

namespace EmployeeSkills.Client.Models
{
    public class Skill
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte Level { get; set; }
    }
}