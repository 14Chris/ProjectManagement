using ProjectManagment.Api.Models;
using ProjectManagment.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagment.Api.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> List();
    }
}
