using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ProjectManagementContext _context;

        public BaseRepository(ProjectManagementContext context)
        {
            _context = context;
        }


    }
}
