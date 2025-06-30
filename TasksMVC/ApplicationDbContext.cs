using Microsoft.EntityFrameworkCore;
using TasksMVC.Models;

namespace TasksMVC
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Assignment>().Property(t => t.Title).HasMaxLength(250).IsRequired();
        }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<SubAssignment> SubAssigments { get; set; }
    }
}
