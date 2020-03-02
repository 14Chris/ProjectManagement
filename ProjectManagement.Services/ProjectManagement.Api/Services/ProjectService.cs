using ProjectManagement.Api.Models;
using ProjectManagement.Api.Repositories;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Project> CreateAsync(AddProjectModel addModel, int idCreator)
        {
            Project project = new Project();
            project.creation_date = DateTime.Now;
            project.name = addModel.name;
            project.id_creator = idCreator;

            return await _projectRepository.CreateAsync(project);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Project project = GetById(id);

            if(project == null)
            {
                return false;
            }

            return await _projectRepository.DeleteAsync(project);
        }

        public Project GetById(int id)
        {
            return _projectRepository.List().Where(x => x.id == id).SingleOrDefault();
        }

        public IEnumerable<Project> List()
        {
           return _projectRepository.List().AsEnumerable();
        }

        public IEnumerable<Project> ListByUser(int idUser)
        {
            return _projectRepository.List().Where(x => x.id_creator == idUser).AsEnumerable();
        }

        public async Task<bool> UpdateAsync(UpdateProjectModel updateModel)
        {
            Project project = GetById(updateModel.id); 

            if(project == null)
            {
                return false;
            }

            return await _projectRepository.UpdateAsync(project);
        }
    }
}
