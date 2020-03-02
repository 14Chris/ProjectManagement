using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Repositories
{
    public interface IProjectRepository
    {
        IQueryable<Project> List();
        Task<Project> CreateAsync(Project project);
        Task<bool> UpdateAsync(Project project);
        Task<bool> DeleteAsync(Project project);
    }
}
