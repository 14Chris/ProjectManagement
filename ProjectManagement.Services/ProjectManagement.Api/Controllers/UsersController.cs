using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Api.Models;
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

            bool res = await _userService.UpdateAsync(model);

            return NoContent();
        }

        // POST: Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(User user)
        { 
            User resUser = await _userService.CreateAsync(user);

            return CreatedAtAction("GetUser", new { id = resUser.id }, resUser);
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

            bool b = await _userService.UpdatePasswordAsync(model);

            return Ok();
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

        //Méthode permettant d'activer un compte utilisateur à l'aide d'un token fourni
        [HttpGet("account_activation/{token}")]
        [AllowAnonymous]
        public ActionResult ActivateAccount(string token)
        {
            bool b = _userService.ActivateAccount(token);

            return Ok();
        }

        //Méthode permettant de générer un token de changement de mot de passe à la suite d'un oubli et d'envoyer un email
        //contenant un lien permettant de le changer
        [HttpPost("forgot_password")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword([FromBody]string email)
        {
            bool b = await _userService.ForgotPassword(email);

            return Ok();
        }

        //Méthode permettant de valider le token fourni pour changer de mot de passe
        [HttpGet("reset_password/{token}")]
        [AllowAnonymous]
        public async Task<ActionResult> ValidateTokenResetPasswordAsync(string token)
        {
            bool b = await _userService.ValidateToken(token);

            //Si Ok
            return Ok();
        }

        //Méthode permettant de modifier le mot de passe d'un utilisateur à la suite d'un oublie
        [HttpPost("reset_password/{token}")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string token, [FromBody]string password)
        {
            bool b = await _userService.ModifyPasswordAsync(token, password);

            return Ok();
        }

    }
}
