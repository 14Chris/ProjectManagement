using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models.Models
{
    public class Project
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public DateTime creation_date { get; set; }

        public byte[] image { get; set; }

        public int id_creator { get; set; }

        public User Creator { get; set; }

        public List<Event> Events { get; set; }

        public List<Task> Tasks { get; set; }
    }
}
