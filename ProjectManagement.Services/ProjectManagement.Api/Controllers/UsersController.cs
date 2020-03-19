using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Models;
using ProjectManagement.Api.Responses;
using ProjectManagement.Api.Services;
using ProjectManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<User>> GetUser()
        {
            return _userService.List().ToList();
        }

        // GET: Users
        [HttpGet("session")]
        [Authorize]
        public ActionResult<UserProfileModel> GetUserSession()
        {

            int id = -1;

            bool ok = Int32.TryParse(HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value, out id);
            if (!ok)
                return Unauthorized();

            User user = _userService.GetById(id);

            var compte = new UserProfileModel()
            {
                id = user.id,
                first_name = user.first_name,
                last_name = user.last_name,
                email = user.email

            };

            if (compte == null)
            {
                return Unauthorized();
            }

            return compte;
        }

        // GET: Users
        [HttpGet("{id}/profile_picture")]
        [AllowAnonymous]
        public IActionResult GetUserProfilePicture(int id)
        {
            var compte = _userService.GetById(id);

            if (compte.profile_picture == null)
                return NotFound();

            return File(compte.profile_picture, "image/jpeg");
        }

        // PUT: Users/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(int id, UserProfileModel model)
        {
            if (id != model.id)
            {
                return BadRequest();
            }

            Response res = await _userService.UpdateAsync(model);

            if (res is ErrorResponse)
            {
                return StatusCode(500, ((ErrorResponse)res).error);
            }
            else if (res is SuccessResponse)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500);
            }


        }

        // POST: Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            Response resUser = await _userService.CreateAsync(user);

            if (resUser is ErrorResponse)
            {
                return StatusCode(500, ((ErrorResponse)resUser).error);
            }
            else if (resUser is SuccessResponse)
            {
                User u = (User)((SuccessResponse)resUser).data;

                return CreatedAtAction("GetUser", new { id = u.id }, u);
            }
            else
            {
                return StatusCode(500);
            }


        }

        // POST: Users/password
        [HttpPost("password")]
        [Authorize]
        public async Task<IActionResult> ModifyPasswordUser(ModifyPasswordModel model)
        {
            int id = -1;

            bool ok = Int32.TryParse(HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value, out id);
            if (!ok)
                return Unauthorized();

            Response res = await _userService.UpdatePasswordAsync(model);

            if (res is ErrorResponse)
            {
                return StatusCode(500, ((ErrorResponse)res).error);
            }
            else if (res is SuccessResponse)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }

         
        }

        // GET: check if email already exists
        [HttpGet("email_exists/{email}")]
        [AllowAnonymous]
        public IActionResult CheckEmailExists(string email)
        {
            if (_userService.EmailExists(email))
            {
                return Conflict();
            }
            else
            {
                return Ok();
            }
        }

        // GET to activate account
        [HttpGet("account_activation/{token}")]
        [AllowAnonymous]
        public ActionResult ActivateAccount(string token)
        {
            bool b = _userService.ActivateAccount(token);

            return Ok();
        }

        //Post to register token for forgot password
        [HttpPost("forgot_password")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword([FromBody]string email)
        {
            bool b = await _userService.ForgotPassword(email);
            
            if(b)
                return Ok();
            else
                return StatusCode(400);
        }

        // GET to validate reset password token
        [HttpGet("reset_password/{token}")]
        [AllowAnonymous]
        public async Task<ActionResult> ValidateTokenResetPasswordAsync(string token)
        {
            Responses.Response res = await _userService.ValidateToken(token);

            if (res is ErrorResponse)
            {
                return StatusCode(500, ((ErrorResponse)res).error);
            }
            else if (res is SuccessResponse)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
      
        }

        // POST to reset password
        [HttpPost("reset_password/{token}")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string token, [FromBody]string password)
        {
            Responses.Response res = await _userService.ModifyPasswordAsync(token, password);

            if (res is ErrorResponse)
            {
                return StatusCode(500, ((ErrorResponse)res).error);
            }
            else if (res is SuccessResponse)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }

    }
}
