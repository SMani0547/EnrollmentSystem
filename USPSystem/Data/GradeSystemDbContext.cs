using Microsoft.EntityFrameworkCore;
using USPSystem.Models;

namespace USPSystem.Data
{
    public class GradeSystemDbContext : DbContext
    {
        public GradeSystemDbContext(DbContextOptions<GradeSystemDbContext> options) : base(options) { }

        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<GradeRecheckApplication> RecheckApplications { get; set; } = null!;

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
            
            // Configure RecheckApplication entity
            modelBuilder.Entity<GradeRecheckApplication>(entity =>
            {
                entity.ToTable("RecheckApplications");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StudentId).IsRequired();
                entity.Property(e => e.CourseCode).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Reason).IsRequired();
                entity.Property(e => e.PaymentReceiptNumber).IsRequired();
            });
        }
    }
} 