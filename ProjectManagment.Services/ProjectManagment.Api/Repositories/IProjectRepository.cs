using ProjectManagment.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagment.Api.Repositories
{
    public interface IProjectRepository
    {
        IEnumerable<Project> List();
    }
}
