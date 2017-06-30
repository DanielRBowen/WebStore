using Microsoft.EntityFrameworkCore;

namespace Tax.Models
{
    public class TaxContext : DbContext
    {
        public TaxContext(DbContextOptions<TaxContext> options)
            : base(options)
        {
        }

        public DbSet<TaxZone> TaxZones { get; set; }

    }
}
