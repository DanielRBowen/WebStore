using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tax
{
    public class TaxZone
    {
        public long Id { get; set; }
        public int ZipCode { get; set; }
        public decimal TaxRate { get; set; }

    }
}
