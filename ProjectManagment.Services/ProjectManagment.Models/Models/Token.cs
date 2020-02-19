using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagment.Models.Models
{
    public class Token
    {
        public int id { get; set; }
        public string token { get; set; }
        public TypeToken type { get; set; }
        public int id_user { get; set; }
        public User User { get; set; }
    }
}
