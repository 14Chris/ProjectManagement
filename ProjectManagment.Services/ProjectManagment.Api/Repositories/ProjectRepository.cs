using ProjectManagment.Models;
using ProjectManagment.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagment.Api.Repositories
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository(ProjectManagmentContext context) : base(context)
        {
        }

        public IEnumerable<Project> List()
        {
            return _context.Project.ToList();
        }
    }
}
