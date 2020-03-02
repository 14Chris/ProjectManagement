using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models.Models
{
    public class Event
    {
        [Key]
        public int id { get; set; }

        public DateTime start_date { get; set; }

        public DateTime end_date { get; set; }

        public string note { get; set; }

        public string description { get; set; }

        public int id_project { get; set; }

        public Project Project { get; set; }
    }
}
