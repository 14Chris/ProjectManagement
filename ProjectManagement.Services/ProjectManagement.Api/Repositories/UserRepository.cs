using ProjectManagement.Models;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Repositories
{
    public class UserRepository : IUserRepository, BaseRepository
    {
        public UserRepository(ProjectManagementContext context) : base(context)
        {
        }

        public IQueryable<User> List()
        {
            return _context.User.AsQueryable();
        }
    }
}
