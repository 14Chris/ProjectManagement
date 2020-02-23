using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagment.Api.Models;
using ProjectManagment.Models;
using ProjectManagment.Models.Models;

namespace ProjectManagment.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ProjectManagmentContext _context;

        public ProjectsController(ProjectManagmentContext context)
        {
            _context = context;
        }

        // GET: Projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProject()
        {
            return await _context.Project.ToListAsync();
        }

        // GET: Projects
        [HttpGet("User")]
        public async Task<ActionResult<IEnumerable<ProjectModel>>> GetProjectsByUser()
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

            return await _context.Project.Where(x => x.id_creator == id).Select(x => new ProjectModel()
            {
                id = x.id,
                name = x.name,
                creation_date = x.creation_date,
                creator = new UserModel()
                {
                    id = x.Creator.id,
                    first_name = x.Creator.first_name,
                    last_name = x.Creator.last_name,
                    email = x.Creator.email,
                }
            }).ToListAsync();
        }

        // GET: Projects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Project.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // PUT: Projects/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.id)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: Projects
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(AddProjectModel model)
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

            Project project = new Project();
            project.creation_date = DateTime.Now;
            project.name = model.name;
            project.id_creator = id;

            _context.Project.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.id }, project);
        }

        // DELETE: Projects/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Project>> DeleteProject(int id)
        {
            var project = await _context.Project.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Project.Remove(project);
            await _context.SaveChangesAsync();

            return project;
        }

        private bool ProjectExists(int id)
        {
            return _context.Project.Any(e => e.id == id);
        }
    }
}
