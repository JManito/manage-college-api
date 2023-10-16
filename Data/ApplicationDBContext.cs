using ManageCollege.Models.Domain;
using ManageCollege.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging.Debug;

namespace ManageCollege.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Authentication> Auth { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Disciplines> Disciplines { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<Professors> Professors { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
    }
}
