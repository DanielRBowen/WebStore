using Microsoft.EntityFrameworkCore;
using WebStore.Models;

namespace WebStore.Data
{
    public class TaxDbContext : DbContext
    {
        public TaxDbContext(DbContextOptions<TaxDbContext> options) : 
            base(options)
        {
        }

        public DbSet<TaxZone> TaxZones { get; set; }
    }
}
