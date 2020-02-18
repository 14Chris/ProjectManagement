using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagment.Models;
using ProjectManagment.Models.Models;

namespace ProjectManagment.Api.Controllers
{
    [Route("[controller]")]
   
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ProjectManagmentContext _context;

        public UsersController(ProjectManagmentContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users
        [HttpGet("session")]
        [Authorize]
        public async Task<ActionResult<User>> GetUserSession()
        {

            int id = -1;

            bool ok = Int32.TryParse(HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value, out id);
            if (!ok)
                return Unauthorized();

            var compte = await _context.User.FindAsync(id);

            if (compte == null)
            {
                return Unauthorized();
            }

            return compte;
        }

        // GET: api/Users
        [HttpGet("{id}/profile_picture")]
        [AllowAnonymous]
        public IActionResult GetUserProfilePicture(int id)
        {
            var compte = _context.User.Find(id);

            if (compte.profile_picture == null)
                return NotFound();

            return File(compte.profile_picture, "image/jpeg");
        }

        // GET: api/Users/5
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

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

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

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> PostUser(User user)
        {

            //Verify if any user has the same email than the parameter
            if(_context.User.Where(x=>x.email == user.email).Any())
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

            return CreatedAtAction("GetUser", new { id = user.id }, user);
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
            if(_context.User.Where(x => x.email == email).Any())
            {
                return Conflict();
            }
            else
            {
                return Ok();
            }
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.id == id);
        }
    }
}
