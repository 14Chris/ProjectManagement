using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Models;
using ProjectManagement.Api.Repositories;
using ProjectManagement.Api.Responses;
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

        public async Task<Response> CreateAsync(AddProjectModel addModel, int idCreator)
        {
            Project project = new Project();
            project.creation_date = DateTime.Now;
            project.name = addModel.name;
            project.id_creator = idCreator;

            Project res = await _projectRepository.CreateAsync(project);

            return new SuccessResponse(res);
        }

        public async Task<Response> DeleteAsync(int id)
        {
            Project project = GetById(id);

            if (project == null)
            {
                return new ErrorResponse("PROJECT_NOT_FOUND");
            }

            bool res = await _projectRepository.DeleteAsync(project);

            return new SuccessResponse(null);
        }

        public Project GetById(int id)
        {
            return _projectRepository.List().Where(x => x.id == id).Include(x => x.Creator).SingleOrDefault();
        }

        public IEnumerable<Project> List()
        {
            return _projectRepository.List().AsEnumerable();
        }

        public IEnumerable<Project> ListByUser(int idUser)
        {
            return _projectRepository.List().Where(x => x.id_creator == idUser).AsEnumerable();
        }

        public async Task<Response> UpdateAsync(UpdateProjectModel updateModel)
        {
            Project project = GetById(updateModel.id);

            if (project == null)
            {
                return new ErrorResponse("PROJECT_NOT_FOUND");
            }

            bool res = await _projectRepository.UpdateAsync(project);

            return new SuccessResponse(null);
        }
    }
}
