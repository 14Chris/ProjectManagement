using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }

        public string first_name { get; set; }

        public string last_name { get; set; }

        public string email { get; set; }

        public string password { get; set; }

        public bool active { get; set; }

        public byte[] profile_picture {get;set;}

        public List<Project> Projects { get; set; }

        public List<Token> Tokens { get; set; }
    }
}
