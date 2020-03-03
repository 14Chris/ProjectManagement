using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Repositories
{
    public interface ITokenRepository
    {
        Task<Token> CreateAsync(Token token);
        IQueryable<Token> List();
        Task<bool> DeleteAsync(Token token);
    }
}
