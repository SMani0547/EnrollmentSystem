using Microsoft.EntityFrameworkCore;
using USPFinance.Models;

namespace USPFinance.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<StudentFinance> StudentFinances { get; set; }
    }
} 