using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Models
{
    public class AddTaskModel
    {
        public string name { get; set; }

        public int idProject { get; set; }
    }
}
