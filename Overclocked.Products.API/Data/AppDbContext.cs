using Microsoft.EntityFrameworkCore;
using Overclocked.Products.API.Models;

namespace Overclocked.Products.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}