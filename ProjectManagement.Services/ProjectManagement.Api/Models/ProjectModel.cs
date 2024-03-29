﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Models
{
    public class ProjectModel
    {
        public int id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string creation_date { get; set; }

        public UserModel creator { get; set; }
    }
}
