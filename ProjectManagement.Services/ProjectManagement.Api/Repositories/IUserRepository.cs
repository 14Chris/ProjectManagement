using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Repositories
{
    public interface IUserRepository
    {
        IQueryable<User> List();
        Task<User> CreateAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(User user);
        bool EmailExists(string email);
    }
}
