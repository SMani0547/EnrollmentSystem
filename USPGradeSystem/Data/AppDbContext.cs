using Microsoft.EntityFrameworkCore;
using USPGradeSystem.Models;

namespace USPEducation.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Grade> Grades { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Grade entity
            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("Grades");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StudentId).IsRequired();
                entity.Property(e => e.CourseId).IsRequired();
                entity.Property(e => e.GradeLetter).HasMaxLength(2).IsRequired();
                entity.Property(e => e.Marks).IsRequired();
            });

            // Seed sample data for actual students from DbSeeder
            modelBuilder.Entity<Grade>().HasData(
                // IT Students
                // John Doe (S12345678)
                new Grade { Id = 1, StudentId = "S12345678", CourseId = "CS101", Marks = 85, GradeLetter = "A" },
                new Grade { Id = 2, StudentId = "S12345678", CourseId = "CS102", Marks = 92, GradeLetter = "A+" },
                new Grade { Id = 3, StudentId = "S12345678", CourseId = "MATH101", Marks = 78, GradeLetter = "B+" },
                
                // Jane Smith (S12345679)
                new Grade { Id = 4, StudentId = "S12345679", CourseId = "CS101", Marks = 65, GradeLetter = "C" },
                new Grade { Id = 5, StudentId = "S12345679", CourseId = "CS102", Marks = 88, GradeLetter = "A" },
                new Grade { Id = 6, StudentId = "S12345679", CourseId = "MATH101", Marks = 75, GradeLetter = "B" },

                // Business Students
                // Bob Wilson (S12345680)
                new Grade { Id = 7, StudentId = "S12345680", CourseId = "ACC101", Marks = 90, GradeLetter = "A+" },
                new Grade { Id = 8, StudentId = "S12345680", CourseId = "FIN101", Marks = 85, GradeLetter = "A" },
                new Grade { Id = 9, StudentId = "S12345680", CourseId = "MGT101", Marks = 82, GradeLetter = "A" },

                // Mary Jones (S12345681)
                new Grade { Id = 10, StudentId = "S12345681", CourseId = "MGT101", Marks = 78, GradeLetter = "B+" },
                new Grade { Id = 11, StudentId = "S12345681", CourseId = "ACC101", Marks = 72, GradeLetter = "B" },
                new Grade { Id = 12, StudentId = "S12345681", CourseId = "ECO101", Marks = 68, GradeLetter = "C+" },

                // Science Students
                // Alex Brown (S12345682)
                new Grade { Id = 13, StudentId = "S12345682", CourseId = "BIO101", Marks = 95, GradeLetter = "A+" },
                new Grade { Id = 14, StudentId = "S12345682", CourseId = "CHE101", Marks = 88, GradeLetter = "A" },
                new Grade { Id = 15, StudentId = "S12345682", CourseId = "MATH101", Marks = 82, GradeLetter = "A" },

                // Sarah Davis (S12345683)
                new Grade { Id = 16, StudentId = "S12345683", CourseId = "CHE101", Marks = 92, GradeLetter = "A+" },
                new Grade { Id = 17, StudentId = "S12345683", CourseId = "PHY101", Marks = 85, GradeLetter = "A" },
                new Grade { Id = 18, StudentId = "S12345683", CourseId = "MATH101", Marks = 78, GradeLetter = "B+" },

                // Economics Students
                // Mike Taylor (S12345684)
                new Grade { Id = 19, StudentId = "S12345684", CourseId = "ECO101", Marks = 88, GradeLetter = "A" },
                new Grade { Id = 20, StudentId = "S12345684", CourseId = "MGT101", Marks = 85, GradeLetter = "A" },
                new Grade { Id = 21, StudentId = "S12345684", CourseId = "MATH101", Marks = 75, GradeLetter = "B" },

                // Lisa Anderson (S12345685)
                new Grade { Id = 22, StudentId = "S12345685", CourseId = "ECO101", Marks = 92, GradeLetter = "A+" },
                new Grade { Id = 23, StudentId = "S12345685", CourseId = "FIN101", Marks = 85, GradeLetter = "A" },
                new Grade { Id = 24, StudentId = "S12345685", CourseId = "MATH101", Marks = 78, GradeLetter = "B+" }
            );
        }
    }
}
