using ProjectManagement.Api.Models;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Services
{
    public interface IUserService
    {
        public User GetById(int id);
        public IEnumerable<User> List();
        public Task<Responses.Response> CreateAsync(User user);
        public Task<Responses.Response> UpdateAsync(UserProfileModel model);
        public Task<Responses.Response> UpdatePasswordAsync(ModifyPasswordModel model);
        public bool EmailExists(string email);
        public bool ActivateAccount(string token);
        public Task<Responses.Response> ModifyPasswordAsync(string token, string password);
        public Task<Responses.Response> ValidateToken(string token);
        public Task<bool> ForgotPassword(string email);
    }
}
