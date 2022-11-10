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
    }
}
