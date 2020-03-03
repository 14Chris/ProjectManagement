using ProjectManagement.Api.Models;
using ProjectManagement.Api.Responses;
using ProjectManagement.Models.Models;
using System.Collections.Generic;

namespace ProjectManagement.Api.Services
{
    public interface ITaskService
    {
        IEnumerable<Task> ListByProject(int idProject);
        Task GetById(int id);
        System.Threading.Tasks.Task<Response> CreateAsync(AddTaskModel t);
        System.Threading.Tasks.Task<Response> UpdateAsync(Task t);
        System.Threading.Tasks.Task<Response> UpdateStateAsync(int idTask, TaskState newState);
        System.Threading.Tasks.Task<Response> DeleteAsync(int id);

    }
}
