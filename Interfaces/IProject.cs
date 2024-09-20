using opms_server_core.Models;

namespace opms_server_core.Interfaces
{
    public class CreateProjectRequest
    {
        public string? ProjectTheme { get; set; }
        public string? Reason { get; set; }
        public string? Type { get; set; }
        public string? Division { get; set; }
        public string? Category { get; set; }
        public string? Priority { get; set; }
        public string? Department { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Location { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now; // Defaults to current time
        public DateTime? UpdatedAt { get; set; } = null; // Will be updated when necessary
    }

    public class UpdateStatusRequest
    {
        public string? Status { get; set; }
    }
    public interface IProject
    {
        Task<(int TotalRecords, List<Project> Projects)> GetAllProjects(string projectTheme, DateTime? startDate, DateTime? endDate, int page, int pageSize);
        Task<(int TotalRecords, List<Project> Projects)> GetUserProjects(int userId, string projectTheme, DateTime? startDate, DateTime? endDate, int page, int pageSize);
        Task<bool?> UpdateProjectStatus(int id, string status);
        Task<bool?> CreateProject(CreateProjectRequest request);
        Task<(int TotalRecords, List<Project> Projects)> GranularSearchProjects(string searchTerm, int page, int pageSize);
    }
}
