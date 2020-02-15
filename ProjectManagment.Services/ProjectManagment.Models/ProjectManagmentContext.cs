using Microsoft.EntityFrameworkCore;
using ProjectManagment.Models.Models;

namespace ProjectManagment.Models
{
    public class ProjectManagmentContext : DbContext
    {
        public ProjectManagmentContext(DbContextOptions<ProjectManagmentContext> options)
                   : base(options) { }

        public ProjectManagmentContext()
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<Event> Event { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Project>()
                .HasOne(x => x.Creator)
                .WithMany(x => x.Projects)
                .HasForeignKey(x => x.id_creator);

            modelBuilder.Entity<Event>()
                .HasOne(x => x.Project)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.id_project);

            modelBuilder.Entity<Task>()
               .HasOne(x => x.Project)
               .WithMany(x => x.Tasks)
               .HasForeignKey(x => x.id_project);
        }
    }
}
