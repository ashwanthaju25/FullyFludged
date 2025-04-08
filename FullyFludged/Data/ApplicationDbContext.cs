using FullyFludged.Models;
using Microsoft.EntityFrameworkCore;

namespace FullyFludged.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}

