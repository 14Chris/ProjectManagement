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
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public ProjectsController(IProjectService projectService, IUserService userService)
        {
            _projectService = projectService;
            _userService = userService;
        }

        /// <summary>
        /// Get all projects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProject()
        {
            return _projectService.List().ToList();
        }

        /// <summary>
        /// Get all project by a user
        /// </summary>
        /// <returns></returns>
        [HttpGet("User")]
        public ActionResult<IEnumerable<ProjectModel>> GetProjectsByUser()
        {
            int id = -1;

            bool ok = Int32.TryParse(HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value, out id);
            if (!ok)
                return Unauthorized();

            var compte = _userService.GetById(id);

            if (compte == null)
            {
                return Unauthorized();
            }

            return _projectService.ListByUser(id).Select(x => new ProjectModel()
            {
                id = x.id,
                name = x.name,
                creation_date = x.creation_date.ToString("dd/MM/yyyy"),
                description = x.description,
                creator = new UserModel()
                {
                    id = x.Creator.id,
                    first_name = x.Creator.first_name,
                    last_name = x.Creator.last_name,
                    email = x.Creator.email,
                }
            }).ToList();
        }

        /// <summary>
        /// Get one project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<ProjectModel> GetProject(int id)
        {
            Project p = _projectService.GetById(id);


            var project = new ProjectModel()
            {
                id = p.id,
                name = p.name,
                creation_date = p.creation_date.ToString("dd/MM/yyyy"),
                description = p.description,
                creator = new UserModel()
                {
                    id = p.Creator.id,
                    first_name = p.Creator.first_name,
                    last_name = p.Creator.last_name,
                    email = p.Creator.email,
                }
            };

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        /// <summary>
        /// Update informations of a project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, UpdateProjectModel model)
        {
            if (id != model.id)
            {
                return BadRequest();
            }

            await _projectService.UpdateAsync(model);

            return NoContent();
        }

        /// <summary>
        /// Create a new project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(AddProjectModel model)
        {
            int id = -1;

            bool ok = Int32.TryParse(HttpContext.User.Identities.FirstOrDefault().Claims.FirstOrDefault().Value, out id);

            if (!ok)
                return Unauthorized();

            var compte = _userService.GetById(id);

            if (compte == null)
            {
                return Unauthorized();
            }

            Project project = await _projectService.CreateAsync(model, id);

            return CreatedAtAction("GetProject", new { id = project.id }, project);
        }

        /// <summary>
        /// Delete a project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProject(int id)
        {
            return await _projectService.DeleteAsync(id);
        }
    }
}
