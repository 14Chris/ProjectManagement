
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectManagement.Api;
using ProjectManagement.Api.Models;
using ProjectManagement.Api.Responses;
using ProjectManagement.Api.Services;
using ProjectManagement.Models;
using ProjectManagement.Models.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ProjectManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IUserService _userService;

        public LoginController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Login([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
 
            Response res =  _userService.LoginUser(login.email, login.password);

            if (res is ErrorResponse)
            {
                return Unauthorized(((ErrorResponse)res).error);
            }
            else if (res is SuccessResponse)
            {
                return Ok(((SuccessResponse)res).data);
            }
            else
            {
                return StatusCode(500);
            }
        }

        
    }
}

