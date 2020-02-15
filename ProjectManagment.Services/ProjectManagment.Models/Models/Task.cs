using System.ComponentModel.DataAnnotations;

namespace ProjectManagment.Models.Models
{
    public class Task
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }

        public int id_project { get; set; }

        public Project Project { get; set; }
    }
}
