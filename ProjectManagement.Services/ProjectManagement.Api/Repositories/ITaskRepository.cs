using ProjectManagement.Api.Responses;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagement.Api.Repositories
{
    public interface ITaskRepository
    {
        IQueryable<Task> List();
        System.Threading.Tasks.Task<Task> CreateAsync(Task t);
        System.Threading.Tasks.Task<bool> UpdateAsync(Task t);
        System.Threading.Tasks.Task<bool> DeleteAsync(Task t);

    }
}
