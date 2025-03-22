using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using USPEducation.Models;

namespace USPEducation.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
  
    public DbSet<StudentAddress> StudentAddresses { get; set; }
    public DbSet<EmergencyContact> EmergencyContacts { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    public DbSet<StudentFinance> StudentFinances { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure one-to-one relationship between ApplicationUser and StudentAddress
        builder.Entity<ApplicationUser>()
            .HasOne(u => u.StudentAddress)
            .WithOne(a => a.User)
            .HasForeignKey<StudentAddress>(a => a.UserId);

        // Configure one-to-one relationship between ApplicationUser and EmergencyContact
        builder.Entity<ApplicationUser>()
            .HasOne(u => u.EmergencyContact)
            .WithOne(e => e.User)
            .HasForeignKey<EmergencyContact>(e => e.UserId);

        // Configure one-to-many relationship between ApplicationUser and Enrollment
        builder.Entity<ApplicationUser>()
            .HasMany(u => u.Enrollments)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship between Course and Enrollment
        builder.Entity<Course>()
            .HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<StudentFinance>()
        .HasKey(sf => sf.Id);
    }
} 