using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Repositories
{
    public class UserRepository : BaseRepository,IUserRepository
    {
        public UserRepository(ProjectManagementContext context) : base(context)
        {
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteAsync(User user)
        {
            _context.Remove(user);

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

        public IQueryable<User> List()
        {
            return _context.User.AsQueryable();
        }

        public async Task<bool> UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;

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

        public bool EmailExists(string email)
        {
            return _context.User.Where(x => x.email == email).Any();
        }
    }
}
