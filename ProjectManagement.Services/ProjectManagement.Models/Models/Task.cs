using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models.Models
{
    public class Task
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }

        public int id_project { get; set; }

        public TaskState state { get; set; }

        public Project Project { get; set; }

    }
}
