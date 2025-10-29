using BestStoreMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace BestStoreMvc
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        // This property is of type DbSet<Product> to manage Product entities in the database. Products would be the name of the table in the database
        public DbSet<Product> Products{ get; set; }
    }
}