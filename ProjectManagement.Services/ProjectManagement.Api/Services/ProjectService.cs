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

        /// <summary>
        /// Method to create a project
        /// </summary>
        /// <param name="addModel"></param>
        /// <param name="idCreator"></param>
        /// <returns></returns>
        public async Task<Response> CreateAsync(AddProjectModel addModel, int idCreator)
        {
            Project project = new Project();
            project.creation_date = DateTime.Now;
            project.name = addModel.name;
            project.id_creator = idCreator;

            Project res = await _projectRepository.CreateAsync(project);

            return new SuccessResponse(res);
        }

        /// <summary>
        /// Method to delete a project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// MEthod to get one project by his id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Project GetById(int id)
        {
            return _projectRepository.List().Where(x => x.id == id).Include(x => x.Creator).SingleOrDefault();
        }

        /// <summary>
        /// Method to get all projects
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Project> List()
        {
            return _projectRepository.List().AsEnumerable();
        }

        /// <summary>
        /// Method to get all project for one user
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        public IEnumerable<Project> ListByUser(int idUser)
        {
            return _projectRepository.List().Where(x => x.id_creator == idUser).AsEnumerable();
        }

        /// <summary>
        /// Method tu update data of a project
        /// </summary>
        /// <param name="updateModel"></param>
        /// <returns></returns>
        public async Task<Response> UpdateAsync(UpdateProjectModel updateModel)
        {
            Project project = GetById(updateModel.id);

            if (project == null)
            {
                return new ErrorResponse("PROJECT_NOT_FOUND");
            }

            project.name = updateModel.name;
            project.description = updateModel.description;
            if(updateModel.image != null && updateModel.image.Length > 0)
            {
                project.image = Convert.FromBase64String(updateModel.image);
            }

            bool res = await _projectRepository.UpdateAsync(project);

            return new SuccessResponse(null);
        }
    }
}
