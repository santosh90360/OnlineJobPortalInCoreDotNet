using JobSeeker.Models;
using Microsoft.EntityFrameworkCore;

namespace JobSeeker.DbContexts
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<JobType> JobTypes { get; set; }
    }
}
