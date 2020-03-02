using ProjectManagement.Api.Models;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Services
{
    public interface IProjectService
    {
        IEnumerable<Project> List();
    }
}
