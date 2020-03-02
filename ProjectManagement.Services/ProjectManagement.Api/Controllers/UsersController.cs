using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Serializers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.MailUtilities;
using ProjectManagement.Api.Models;
using ProjectManagement.Models;
using ProjectManagement.Models.Models;

namespace ProjectManagement.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ProjectManagementContext _context;
        private readonly IEmailSender _emailSender;

        public UsersController(ProjectManagementContext context,IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
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

            var compte = _context.User.Where(x => x.id == id).Select(x => new UserProfileModel()
            {
                id = x.id,
                first_name = x.first_name,
                last_name =x.last_name,
                email = x.email
             
            }).SingleOrDefault();

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
            var compte = _context.User.Find(id);

            if (compte.profile_picture == null)
                return NotFound();

            return File(compte.profile_picture, "image/jpeg");
        }

        // GET: Users/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
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

            User user = _context.User.Find(id);

            user.last_name = model.last_name;
            user.first_name = model.first_name;
            user.profile_picture = Convert.FromBase64String(model.profile_picture);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(User user)
        {

            //Verify if any user has the same email than the parameter
            if (_context.User.Where(x => x.email == user.email).Any())
            {
                return BadRequest("EMAIL_ALREADY_REGISTRED");
            }

            //Verify that the password checked the requirements
            if (!PasswordUtilities.PasswordMatchRegex(user.password))
            {
                return BadRequest("PASSWORD_TOO_WEAK");
            }

            //Hash the password of the user
            user.password = PasswordUtilities.HashPassword(user.password);
            user.active = false;

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            // Génération d'un token pour l'activation du compte valable pendant 2h
            var token = new JwtBuilder()
                  .WithAlgorithm(new HMACSHA256Algorithm())
                  .WithSecret("tokenReset33-password&!")
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(24).ToUnixTimeSeconds())
                  .AddClaim("claim2", "claim2-value")
                  .Encode();

            //Ajout du token en base de données
            Token tokenActivation = new Token();
            tokenActivation.id_user = user.id;
            tokenActivation.type = TypeToken.AccountActivation;
            tokenActivation.token = token;
            _context.Token.Add(tokenActivation);

            await _context.SaveChangesAsync();

            if (tokenActivation.id > 0)
            {
                // Envoi d'un email avec un lien d'activation du compte
                ((EmailSender)_emailSender).SendAccountActivationMailAsync(user, tokenActivation.token);
            }

            return CreatedAtAction("GetUser", new { id = user.id }, user);
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

            var compte = _context.User.Where(x => x.id == id).SingleOrDefault();

            if (compte == null)
            {
                return Unauthorized();
            }

            if(compte.password != PasswordUtilities.HashPassword(model.oldPassword))
            {
                return BadRequest("INVALID_OLD_PASSWORD");
            }

            if (!PasswordUtilities.PasswordMatchRegex(model.newPassword))
            {
                return BadRequest("NEW_PASSWORD_TOO_WEAK");
            }

            compte.password = PasswordUtilities.HashPassword(model.newPassword);

            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        // GET: check if email already exists
        [HttpGet("email_exists/{email}")]
        [AllowAnonymous]
        public IActionResult CheckEmailExists(string email)
        {
            if (_context.User.Where(x => x.email == email).Any())
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
        public async Task<ActionResult> ActivateAccount(string token)
        {
            //Récupération du dernier token (par l'id)
            Token tokenActivation = _context.Token.Where(x => x.token == token && x.type == TypeToken.AccountActivation).OrderByDescending(x => x.id).FirstOrDefault();

            if (tokenActivation == null)
                return BadRequest();

            string secret = "tokenReset33-password&!";

            //Vérification du token (expiration, clé, ...)
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA256Algorithm());

                var json = decoder.Decode(token, secret, verify: true);
            }
            catch (TokenExpiredException)
            {
                _context.Token.Remove(tokenActivation);
                return BadRequest();
            }
            catch (SignatureVerificationException)
            {
                return BadRequest();
            }

            //Si token ok, on active le compte
            User user = _context.User.Find(tokenActivation.id_user);
            user.active = true;
            //On supprime le token d'activation
            _context.Token.Remove(tokenActivation);
            await _context.SaveChangesAsync();

            //et on envoie un email à l'utilisateur

            return Ok();
        }

        //Méthode permettant de générer un token de changement de mot de passe à la suite d'un oubli et d'envoyer un email
        //contenant un lien permettant de le changer
        [HttpPost("forgot_password")]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword([FromBody]string email)
        {
            //Récupération de l'utilisateur par email
            User user = _context.User.Where(x => x.email == email).SingleOrDefault();

            //Si pas d'utilisateur trouvé
            if (user == null)
                return BadRequest();

            //Génération d'un token pour réinitialiser le mot de passe de l'utilisateur
            var token = new JwtBuilder()
                  .WithAlgorithm(new HMACSHA256Algorithm())
                  .WithSecret("tokenReset33-password&!")
                  .AddClaim("exp", DateTimeOffset.UtcNow.AddHours(6).ToUnixTimeSeconds())
                  .AddClaim("claim2", "claim2-value")
                  .Encode();

            //Ajout du token en base de données
            Token tokenActivation = new Token();
            tokenActivation.id_user = user.id;
            tokenActivation.type = TypeToken.ForgotPassword;
            tokenActivation.token = token;
            _context.Token.Add(tokenActivation);

            await _context.SaveChangesAsync();

            if (tokenActivation.id > 0)
            {
                //Envoi d'un email contenant un lien pour réinitialier le mot de passe
                ((EmailSender)_emailSender).SendResetPasswordMailAsync(user, tokenActivation.token);
            }

            return Ok();
        }

        //Méthode permettant de valider le token fourni pour changer de mot de passe
        [HttpGet("reset_password/{token}")]
        [AllowAnonymous]
        public ActionResult ValidateTokenResetPassword(string token)
        {
            //Si le token est bien valide pour changer le mot de passe
            Token tokenReset = _context.Token.Where(x => x.token == token && x.type == TypeToken.ForgotPassword).OrderByDescending(x => x.id).FirstOrDefault();

            if (tokenReset == null)
                return BadRequest("BAD_TOKEN");

            string secret = "tokenReset33-password&!";

            //Vérification du token (expiration, clé, ...)
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA256Algorithm());

                var json = decoder.Decode(token, secret, verify: true);
            }
            catch (TokenExpiredException)
            {
                _context.Token.Remove(tokenReset);
                return BadRequest("TOKEN_EXPIRED");
            }
            catch (SignatureVerificationException)
            {
                return BadRequest("BAD_SIGNATURE");
            }

            //Si Ok
            return Ok();
        }

        //Méthode permettant de modifier le mot de passe d'un utilisateur à la suite d'un oublie
        [HttpPost("reset_password/{token}")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword(string token, [FromBody]string password)
        {
            //Si le token est bien valide pour changer le mot de passe
            Token tokenReset = _context.Token.Where(x => x.token == token && x.type == TypeToken.ForgotPassword).OrderByDescending(x => x.id).FirstOrDefault();

            if (tokenReset == null)
                return BadRequest("BAD_TOKEN");

            string secret = "tokenReset33-password&!";

            //Vérification du token (expiration, clé, ...)
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new HMACSHA256Algorithm());

                var json = decoder.Decode(token, secret, verify: true);
            }
            catch (TokenExpiredException)
            {
                _context.Token.Remove(tokenReset);
                return BadRequest();
            }
            catch (SignatureVerificationException)
            {
                return BadRequest();
            }

            User user = _context.User.Find(tokenReset.id_user);

            if (user == null)
                return BadRequest("NO_USER");

            //test si le nouveau mot de passe respect les conditions (regex)
            if (!PasswordUtilities.PasswordMatchRegex(password))
                return BadRequest("PASSWORD_TOO_WEAK");

            user.password = PasswordUtilities.HashPassword(password);

            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.id == id);
        }
    }
}
