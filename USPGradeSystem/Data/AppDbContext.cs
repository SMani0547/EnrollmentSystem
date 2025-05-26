using Microsoft.EntityFrameworkCore;
using USPGradeSystem.Models;

namespace USPEducation.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<RecheckApplication> RecheckApplications { get; set; } = null!;

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
            modelBuilder.Entity<RecheckApplication>(entity =>
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
