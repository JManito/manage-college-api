using ManageCollege.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ManageCollege.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<Disciplines> Disciplines { get; set; }
        public DbSet<Grades> Grades { get; set; }
        public DbSet<Professors> Professors { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
    }
}
