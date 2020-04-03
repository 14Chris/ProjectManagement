using ProjectManagement.Api.Models;
using ProjectManagement.Api.Repositories;
using ProjectManagement.Api.Responses;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = ProjectManagement.Models.Models.Task;

namespace ProjectManagement.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Method to create a task
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<Response> CreateAsync(AddTaskModel t)
        {
            Task task = new Task();
            task.name = t.name;
            task.id_project = t.idProject;
            task.state = TaskState.ToDo;
            task.priority = TaskPriority.Normal;
            task.description = t.desc;
            task.creation_date = DateTime.Now;

            Task resTask = await _taskRepository.CreateAsync(task);

            if (resTask != null)
            {
                return new SuccessResponse(resTask);

            }
            else
            {
                return new ErrorResponse("Error");
            }
        }

        /// <summary>
        /// Method to get a task by his id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task GetById(int id)
        {
            return _taskRepository.List().Where(x => x.id == id).SingleOrDefault();
        }

        /// <summary>
        /// Method to delete a task
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<Response> DeleteAsync(int id)
        {
            Task task = GetById(id);

            bool res = await _taskRepository.DeleteAsync(task);

            if (res)
            {
                return new SuccessResponse(null);
            }
            else
            {
                return new ErrorResponse("");
            }

        }

        /// <summary>
        /// Method to list all tasks for a project
        /// </summary>
        /// <param name="idProject"></param>
        /// <returns></returns>
        public IEnumerable<Task> ListByProject(int idProject)
        {
            return _taskRepository.List().Where(x => x.id_project == idProject).ToList();
        }

        /// <summary>
        /// Method to update a task
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public async Task<Response> UpdateAsync(Task t)
        {
            bool res = await _taskRepository.UpdateAsync(t);

            if (res)
            {
                return new SuccessResponse(null);
            }
            else
            {
                return new ErrorResponse("");
            }
        }

        /// <summary>
        /// Method to update the state of a task
        /// </summary>
        /// <param name="idTask"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public async Task<Response> UpdateStateAsync(int idTask, TaskState newState)
        {
            Task task = GetById(idTask);

            task.state = newState;

            bool res = await _taskRepository.UpdateAsync(task);

            if (res)
            {
                return new SuccessResponse(null);
            }
            else
            {
                return new ErrorResponse("");
            }

            throw new System.NotImplementedException();
        }
    }
}
