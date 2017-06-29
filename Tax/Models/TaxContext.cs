using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
