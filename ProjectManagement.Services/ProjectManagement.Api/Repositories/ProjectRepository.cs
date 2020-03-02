using ProjectManagement.Models;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Repositories
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository(ProjectManagementContext context) : base(context)
        {
        }

        public IEnumerable<Project> List()
        {
            return _context.Project.ToList();
        }
    }
}
