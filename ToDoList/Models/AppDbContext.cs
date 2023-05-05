using Microsoft.EntityFrameworkCore;

namespace ToDoList.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobPriority> JobPriorities { get; set; }
        public DbSet<JobStatus> JobStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<JobPriority>().HasData(
                new JobPriority() { Id = 1, Name = "High" },
                new JobPriority() { Id = 2, Name = "Medium" },
                new JobPriority() { Id = 3, Name = "Low" }
                );

            modelBuilder.Entity<JobStatus>().HasData(
                new JobStatus() { Id = 1, Name = "Postponed" },
                new JobStatus() { Id = 2, Name = "Started" },
                new JobStatus() { Id = 3, Name = "In process" },
                new JobStatus() { Id = 4, Name = "Delayed" },
                new JobStatus() { Id = 5, Name = "Completed" }
                );
        }
    }
}
