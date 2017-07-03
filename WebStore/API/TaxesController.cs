using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebStore.Data;
using WebStore.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebStore.API
{
    [Route("api/[controller]")]
    public class TaxesController : Controller
    {
        private readonly TaxDbContext _context;

        public TaxesController(TaxDbContext taxDbContext)
        {
            _context = taxDbContext;

            if (_context.TaxZones.Count() == 0)
            {
                _context.TaxZones.Add(new TaxZone { ZipCode = 84037, TaxRate = (decimal)0.1 });
                _context.SaveChanges();
            }
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("taxrate/{zipcode}")]
        public IActionResult GetTaxRate(int zipcode)
        {
            var taxzone = _context.TaxZones.FirstOrDefault(taxzone0 => taxzone0.ZipCode == zipcode);

            if (taxzone == null)
            {
                return NotFound();
            }

            return Ok(taxzone.TaxRate);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
