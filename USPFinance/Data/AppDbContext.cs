using Microsoft.EntityFrameworkCore;

namespace USPFinance.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Add your DbSet properties here for your entities
    // Example: public DbSet<Student> Students { get; set; }
} 