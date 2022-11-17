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
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<JobType> JobTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 1,
                Name = "Web Developers"
              
            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 2,
                Name = "Mobile Developers"

            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 3,
                Name = "Designers & Creatives"

            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 4,
                Name = "Writers"

            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 5,
                Name = "Virtual Assistants"

            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 6,
                Name = "Customer Service Agents"

            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 7,
                Name = "Sales & Marketing Experts"

            });
            modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 8,
                Name = "Accountants & Consultants"

            });

            //JobType
            modelBuilder.Entity<JobType>().HasData(new JobType
            {
                Id = 1,
                Name = "Full - Time"

            });
            modelBuilder.Entity<JobType>().HasData(new JobType
            {
                Id = 2,
                Name = "Part-Time"

            });
            modelBuilder.Entity<JobType>().HasData(new JobType
            {
                Id = 3,
                Name = "Internship"

            });
            modelBuilder.Entity<JobType>().HasData(new JobType
            {
                Id = 4,
                Name = "Freelance"

            });
        }       
    }
}
