using Microsoft.EntityFrameworkCore;
using opms_server_core.Controllers;
using opms_server_core.Data;
using opms_server_core.Models;
using opms_server_core.Interfaces;

namespace opms_server_core.Repositories
{
    public class ProjectRepository : IProject
    {
        private readonly OPMSDbContext _context;

        public ProjectRepository(OPMSDbContext context)
        {
            _context = context;
        }

        public async Task<(int TotalRecords, List<Project> Projects)> GetAllProjects(string projectTheme, DateTime? startDate, DateTime? endDate, int page, int pageSize)
        {
            var query = _context.Projects.AsQueryable();

            // Filtering by string values
            if (!string.IsNullOrEmpty(projectTheme))
            {
                query = query.Where(p => p.ProjectTheme.Contains(projectTheme));
            }

            // Filtering by start and end date
            if (startDate.HasValue)
            {
                query = query.Where(p => p.StartDate >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.EndDate <= endDate);
            }

            // Pagination
            var totalRecords = await query.CountAsync();
            var projects = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (totalRecords, projects);
        }

        public async Task<(int TotalRecords, List<Project> Projects)> GetUserProjects(int userId, string projectTheme, DateTime? startDate, DateTime? endDate, int page, int pageSize)
        {
            var query = _context.Projects.Where(p => p.UserId == userId);

            if (!string.IsNullOrEmpty(projectTheme))
            {
                query = query.Where(p => p.ProjectTheme.Contains(projectTheme));
            }

            if (startDate.HasValue)
            {
                query = query.Where(p => p.StartDate >= startDate);
            }

            if (endDate.HasValue)
            {
                query = query.Where(p => p.EndDate <= endDate);
            }

            var totalRecords = await query.CountAsync();
            var projects = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (totalRecords, projects);
        }

        public async Task<bool> UpdateProjectStatus(int id, string status)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return false; // Project not found
            }

            var validStatuses = new List<string> { "Running", "Closed", "Cancelled" };
            if (!validStatuses.Contains(status))
            {
                throw new ArgumentException("Invalid status value. Valid statuses are: Running, Closed, Cancelled.");
            }

            project.Status = status;
            project.UpdatedAt = DateTime.Now;

            _context.Projects.Update(project);
            await _context.SaveChangesAsync();

            return true; // Update successful
        }

        public async Task<bool> CreateProject(CreateProjectRequest request)
        {
            var project = new Project
            {
                ProjectTheme = request.ProjectTheme,
                Reason = request.Reason,
                Type = request.Type,
                Division = request.Division,
                Category = request.Category,
                Priority = request.Priority,
                Department = request.Department,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Location = request.Location,
                UserId = request.UserId,
                CreatedAt = DateTime.Now,
            };

            try
            {
                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
                return true; // Creation successful
            }
            catch
            {
                return false; // Handle exceptions (e.g., database errors)
            }
        }


        public async Task<(int TotalRecords, List<Project> Projects)> GranularSearchProjects(string searchTerm, int page, int pageSize)
        {
            var query = _context.Projects.AsQueryable();

            DateTime parsedDate;
            bool isDate = DateTime.TryParse(searchTerm, out parsedDate);

            if (isDate)
            {
                query = query.Where(p =>
                    (p.StartDate.HasValue && p.StartDate.Value.Year == parsedDate.Year && p.StartDate.Value.Month == parsedDate.Month && p.StartDate.Value.Day == parsedDate.Day) ||
                    (p.EndDate.HasValue && p.EndDate.Value.Year == parsedDate.Year && p.EndDate.Value.Month == parsedDate.Month && p.EndDate.Value.Day == parsedDate.Day));
            }
            else
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(p =>
                        p.ProjectTheme.Contains(searchTerm) ||
                        p.Reason.Contains(searchTerm) ||
                        p.Type.Contains(searchTerm) ||
                        p.Division.Contains(searchTerm) ||
                        p.Category.Contains(searchTerm) ||
                        p.Priority.Contains(searchTerm) ||
                        p.Department.Contains(searchTerm) ||
                        p.Location.Contains(searchTerm));
                }
            }

            var totalRecords = await query.CountAsync();
            var projects = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (totalRecords, projects);
        }
    }
}
