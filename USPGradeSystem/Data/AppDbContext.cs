using Microsoft.EntityFrameworkCore;
using USPGradeSystem.Models;

namespace USPEducation.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Nullable or initialized DbSet
        public DbSet<Grade>? Grades { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Grade>().HasData(
                new Grade { Id = 1, StudentId = "S11111", CourseId = "CS101", Marks = 85, GradeLetter = "A" },
                new Grade { Id = 2, StudentId = "S22222", CourseId = "CS102", Marks = 76, GradeLetter = "B" }
            );
        }
    }
}
