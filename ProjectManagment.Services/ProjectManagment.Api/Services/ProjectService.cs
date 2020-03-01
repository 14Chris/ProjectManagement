using ProjectManagment.Api.Models;
using ProjectManagment.Api.Repositories;
using ProjectManagment.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagment.Api.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IEnumerable<Project> List()
        {
           return _projectRepository.List();
        }
    }
}
