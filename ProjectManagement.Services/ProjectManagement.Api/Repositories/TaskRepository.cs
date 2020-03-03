using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;
using ProjectManagement.Models.Models;
using System;
using System.Linq;

namespace ProjectManagement.Api.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository(ProjectManagementContext context) : base(context)
        {
        }

        public async System.Threading.Tasks.Task<Task> CreateAsync(Task t)
        {
            _context.Add(t);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return t;
        }

        public async System.Threading.Tasks.Task<bool> UpdateAsync(Task t)
        {
            _context.Entry(t).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async System.Threading.Tasks.Task<bool> DeleteAsync(Task t)
        {
            _context.Remove(t);

            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IQueryable<Task> List()
        {
            return _context.Task.AsQueryable();
        }
    }
}
