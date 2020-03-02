using ProjectManagement.Models;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Repositories
{
    public class TokenRepository : BaseRepository, ITokenRepository
    {
        public TokenRepository(ProjectManagementContext context) : base(context)
        {
        }

        public async Task<Token> CreateAsync(Token token)
        {
            _context.Token.Add(token);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return null;
            }

            return token;
        }

        public async Task<bool> DeleteAsync(Token token)
        {
            _context.Remove(token);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
           

            return true;
        }

        public IQueryable<Token> List()
        {
            return _context.Token.AsQueryable();
        }
    }
}
