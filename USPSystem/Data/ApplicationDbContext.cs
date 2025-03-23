using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using USPSystem.Models;

namespace USPSystem.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<SubjectArea> SubjectAreas { get; set; }
    public DbSet<StudentEnrollment> StudentEnrollments { get; set; }
    public DbSet<AcademicProgram> Programs { get; set; }
    public DbSet<ProgramRequirement> ProgramRequirements { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Course prerequisites relationship
        builder.Entity<Course>()
            .HasMany(c => c.Prerequisites)
            .WithMany(c => c.IsPrerequisiteFor)
            .UsingEntity(j => j.ToTable("CoursePrerequisites"));

        // Configure Course-Program relationships
        builder.Entity<AcademicProgram>()
            .HasMany(p => p.CoreCourses)
            .WithMany(c => c.IsCoreCourseFor)
            .UsingEntity(j => j.ToTable("ProgramCoreCourses"));

        builder.Entity<AcademicProgram>()
            .HasMany(p => p.ElectiveCourses)
            .WithMany(c => c.IsElectiveCourseFor)
            .UsingEntity(j => j.ToTable("ProgramElectiveCourses"));

        // Configure StudentEnrollment relationships
        builder.Entity<StudentEnrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<StudentEnrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.StudentEnrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure ProgramRequirement relationships
        builder.Entity<ProgramRequirement>()
            .HasOne(r => r.Program)
            .WithMany(p => p.Requirements)
            .HasForeignKey(r => r.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProgramRequirement>()
            .HasOne(r => r.SubjectArea)
            .WithMany()
            .HasForeignKey(r => r.SubjectAreaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ProgramRequirement>()
            .HasMany(r => r.RequiredCourses)
            .WithMany()
            .UsingEntity(j => j.ToTable("ProgramRequirementCourses"));
    }
} 

