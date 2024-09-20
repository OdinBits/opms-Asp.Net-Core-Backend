using Microsoft.EntityFrameworkCore;
using opms_server_core.Models;

namespace opms_server_core.Data
{
    public class OPMSDbContext(DbContextOptions<OPMSDbContext> options) : DbContext(options)
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
 