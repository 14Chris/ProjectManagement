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
    public class TasksController : ControllerBase
    {
        private readonly ProjectManagmentContext _context;

        public TasksController(ProjectManagmentContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectManagment.Models.Models.Task>>> GetTask()
        {
            return await _context.Task.ToListAsync();
        }


        /// <summary>
        /// Get all tasks of a project
        /// </summary>
        /// <param name="idProject"></param>
        /// <returns></returns>
        [HttpGet("project/{idProject}")]
        public async Task<ActionResult<IEnumerable<ProjectManagment.Models.Models.Task>>> GetTaskByProject(int idProject)
        {
            return await _context.Task.Where(x=>x.id_project == idProject).ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectManagment.Models.Models.Task>> GetTask(int id)
        {
            var task = await _context.Task.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, ProjectManagment.Models.Models.Task task)
        {
            if (id != task.id)
            {
                return BadRequest();
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ProjectManagment.Models.Models.Task>> PostTask(AddTaskModel model)
        {
            ProjectManagment.Models.Models.Task task = new ProjectManagment.Models.Models.Task();
            task.name = model.name;
            task.id_project = model.idProject;
            task.state = TaskState.ToDo;

            _context.Task.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTask", new { id = task.id }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProjectManagment.Models.Models.Task>> DeleteTask(int id)
        {
            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Task.Remove(task);
            await _context.SaveChangesAsync();

            return task;
        }

        private bool TaskExists(int id)
        {
            return _context.Task.Any(e => e.id == id);
        }
    }
}
