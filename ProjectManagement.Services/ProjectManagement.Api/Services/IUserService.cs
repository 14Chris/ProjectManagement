using ProjectManagement.Api.Models;
using ProjectManagement.Api.Responses;
using ProjectManagement.Models.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Services
{
    public interface IUserService
    {
        User GetById(int id);
        IEnumerable<User> List();
        Task<Response> CreateAsync(User user);
        Task<Response> UpdateAsync(UserProfileModel model);
        Task<Response> UpdatePasswordAsync(ModifyPasswordModel model);
        bool EmailExists(string email);
        bool ActivateAccount(string token);
        Task<Response> ModifyPasswordAsync(string token, string password);
        Task<Response> ValidateToken(string token);
        Task<bool> ForgotPassword(string email);
        Response LoginUser(string email, string password);
    }
}
