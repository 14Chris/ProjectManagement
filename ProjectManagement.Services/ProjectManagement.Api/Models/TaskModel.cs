using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Models
{
    public class TaskModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string creationDate { get; set; }
    }
}
