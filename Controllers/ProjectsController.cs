using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using opms_server_core.Interfaces;

namespace opms_server_core.Controllers
{
    [Authorize]
    [Route("api/opms/v1/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProject _projectService;

        public ProjectsController(IProject projectService)
        {
            _projectService = projectService;
        }

        // Get all projects with optional search, filtering, and pagination
        [HttpGet("all")]
        public async Task<IActionResult> GetAllProjects(
            [FromQuery] string projectTheme = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var result = await _projectService.GetAllProjects(projectTheme, startDate, endDate, page, pageSize);
            return Ok(new
            {
                result.TotalRecords,
                result.Projects
            });
        }

        // Get user-specific projects with pagination and optional search filters
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserProjects(
            [FromRoute] int userId,
            [FromQuery] string projectTheme = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var result = await _projectService.GetUserProjects(userId, projectTheme, startDate, endDate, page, pageSize);
            return Ok(new
            {
                result.TotalRecords,
                result.Projects
            });
        }

        // Update the status of a project (only accepts 'Running', 'Closed', 'Cancelled')
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateProjectStatus([FromRoute] int id, [FromBody] UpdateStatusRequest request)
        {
            try
            {
                var updatedProject = await _projectService.UpdateProjectStatus(id, request.Status);
                if (updatedProject == null)
                {
                    return NotFound("Project not found.");
                }

                return Ok("Project status updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Invalid status
            }
        }

        // Create new project
        [HttpPost("create")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProject = await _projectService.CreateProject(request);
            if (createdProject == null)
            {
                return BadRequest("Failed to create project.");
            }

            return Ok(new { message = "Project created successfully", status = true });
        }

        // Search projects with granular filtering (by date or text)
        [HttpGet("search")]
        public async Task<IActionResult> GranularSearchProjects(
            [FromQuery] string searchTerm = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 5)
        {
            var result = await _projectService.GranularSearchProjects(searchTerm, page, pageSize);
            return Ok(new
            {
                result.TotalRecords,
                result.Projects
            });
        }
    }
}
