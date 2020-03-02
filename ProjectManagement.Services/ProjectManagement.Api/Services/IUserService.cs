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
        public Task<User> CreateAsync(User user);
        public Task<bool> UpdateAsync(UserProfileModel model);
        public Task<bool> UpdatePasswordAsync(ModifyPasswordModel model);
        public bool EmailExists(string email);
        public bool ActivateAccount(string token);
        public Task<bool> ModifyPasswordAsync(string token, string password);
        public Task<bool> ValidateToken(string token);
        public Task<bool> ForgotPassword(string email);
    }
}
