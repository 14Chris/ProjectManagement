using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models.Models
{
    public class Task
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public DateTime creation_date { get; set; }

        public DateTime? start_date { get; set; }

        public DateTime? due_date { get; set; }

        public int id_project { get; set; }

        public int? id_main_task { get; set; }

        public TaskState state { get; set; }

        public Project Project { get; set; }

        public Task MainTask { get; set; }

        public List<Task> SubTasks { get; set; }

    }
}
