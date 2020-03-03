using ProjectManagement.Models;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.Api.Repositories
{
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository(ProjectManagementContext context) : base(context)
        {
        }

        public IQueryable<Project> List()
        {
            return _context.Project.AsQueryable();
        }

        public async Task<Project> CreateAsync(Project project)
        {
            _context.Project.Add(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<bool> DeleteAsync(Project project)
        {
            _context.Project.Remove(project);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(Project project)
        {
            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }
    }
}
