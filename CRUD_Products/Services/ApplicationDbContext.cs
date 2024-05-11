using CRUD_Products.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Products.Services
{
    public class ApplicationDbContext : DbContext

    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        { 

        }

        public DbSet<Product> Products { get; set; }
    }
}
