using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Api.Models;
using ProjectManagement.Api.Responses;
using ProjectManagement.Api.Services;
using ProjectManagement.Models;
using ProjectManagement.Models.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = ProjectManagement.Models.Models.Task;

namespace ProjectManagement.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        //private readonly ProjectManagementContext _context;
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            //_context = context;
            _taskService = taskService;
        }

        /// <summary>
        /// Get all tasks of a project
        /// </summary>
        /// <param name="idProject"></param>
        /// <returns></returns>
        [HttpGet("project/{idProject}")]
        public ActionResult<IEnumerable<TaskModel>> GetTaskByProject(int idProject)
        {
            return _taskService.ListByProject(idProject).Select(x=>new TaskModel
            {
                id = x.id,
                name = x.name,
                creationDate= x.creation_date.ToShortDateString(),
                description = x.description,
                state = x.state.ToString(),
                priority = x.priority.ToString()
            }).ToList();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTask(int id)
        {
            return _taskService.GetById(id);
        }

        // PUT: task
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, Task task)
        {
            if (id != task.id)
            {
                return BadRequest();
            }

            Response res = await _taskService.UpdateAsync(task);

            if(res is SuccessResponse)
            {
                return NoContent();
            }
            else if(res is ErrorResponse)
            {
                return StatusCode(500, ((ErrorResponse)res).error);
            }
            else
            {
                return StatusCode(500);
            }

            
        }

        // PUT: Update state of a task
        [HttpPut("{id}/state")]
        public async Task<IActionResult> ChangeStateTask(int id, [FromBody]int stateTask)
        {
            Response res = await _taskService.UpdateStateAsync(id, (TaskState)stateTask);

            if (res is SuccessResponse)
            {
                return NoContent();
            }
            else if (res is ErrorResponse)
            {
                return StatusCode(500, ((ErrorResponse)res).error);
            }
            else
            {
                return StatusCode(500);
            }

            return NoContent();
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask(AddTaskModel model)
        {
            Response res = await _taskService.CreateAsync(model);

            if (res is SuccessResponse)
            {
                Task task = ((SuccessResponse)res).data;
                return CreatedAtAction("GetTask", new { id = task.id }, task);
            }
            else if (res is ErrorResponse)
            {
                return StatusCode(500, ((ErrorResponse)res).error);
            }
            else
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Task>> DeleteTask(int id)
        {
            Response res = await _taskService.DeleteAsync(id);

            if (res is SuccessResponse)
            {
                return Ok();
            }
            else if (res is ErrorResponse)
            {
                return StatusCode(500, ((ErrorResponse)res).error);
            }
            else
            {
                return StatusCode(500);
            }
        }

    }
}
