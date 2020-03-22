using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models.Models;

namespace ProjectManagement.Models
{
    public class ProjectManagementContext : DbContext
    {
        public ProjectManagementContext(DbContextOptions<ProjectManagementContext> options)
                   : base(options) { }

        public ProjectManagementContext()
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Task> Task { get; set; }
        public DbSet<Event> Event { get; set; }
        public DbSet<Token> Token { get; set; }

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

            modelBuilder.Entity<Task>()
               .HasOne(x => x.MainTask)
               .WithMany(x => x.SubTasks)
               .HasForeignKey(x => x.id_main_task);

            modelBuilder.Entity<Token>()
             .HasOne(x => x.User)
             .WithMany(x => x.Tokens)
             .HasForeignKey(x => x.id_user);
        }
    }
}
