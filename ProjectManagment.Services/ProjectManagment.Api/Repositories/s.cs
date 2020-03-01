using ProjectManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagment.Api.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ProjectManagmentContext _context;

        public BaseRepository(ProjectManagmentContext context)
        {
            _context = context;
        }


    }
}
