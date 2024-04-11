using DemoWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoWebApp.DataBaseEntity
{
    public class ProductDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-MFLCOI2;Initial Catalog=DemoDb;User ID=sa;Password=adk@1234;Encrypt=false;");
        }
    }
}
